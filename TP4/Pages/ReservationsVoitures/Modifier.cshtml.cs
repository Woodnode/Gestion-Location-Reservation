using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;
using System.ComponentModel.DataAnnotations;

namespace TP4.Pages.ReservationsVoitures
{
    public class ModifierModel : PageModel
    {
        [BindProperty]
        public required Reservation Reservation { get; set; }

        public required List<Voiture> ListeDeVoitures { get; set; }

        public required List<Reservation> ListeDeReservations { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Veuillez sélectionner une voiture.")]
        public int IdVoiture { get; set; }

        public ActionResult OnGet(int? id)
        {
            if (id == null ||
                GestionReservable.ObtenirReservableParId("Reservation", id.Value) is not Reservation reservation || 
                reservation is null ||
                reservation.ObjetDeLaReservation is null || 
                GestionReservable.ObtenirReservableParId("Voiture", reservation.ObjetDeLaReservation.Id) is not Voiture)
                return NotFound();

            Reservation = reservation;
            IdVoiture = reservation.ObjetDeLaReservation.Id;

            ChargementDePage();

            return Page();
        }

        public ActionResult OnPost()
        {
            ChargementDePage();
            ListeDeReservations = [.. GestionReservable.ObtenirListeReservable("Reservation").Cast<Reservation>()];

            Voiture voiture = ListeDeVoitures.First(v => v.Id == IdVoiture);
            if (voiture == null) ModelState.AddModelError("IdVoiture", "Veuillez sélectionner une voiture.");
            else
            {
                Reservation.ObjetDeLaReservation = voiture;
                Reservation.PrixJournalier = voiture.PrixJournalier;
                Reservation.Prix = Reservation.PrixJournalier * ((Reservation.DateFin - Reservation.DateDebut).Days + 1);
            }

            if (Reservation.DateDebut < DateTime.Now.Date)
            {
                ModelState.AddModelError("Reservation.DateDebut", $"La date de début doit être supérieure ou égale à {DateTime.Now.Date:yyyy-MM-dd}.");
            }
            if (Reservation.DateFin < Reservation.DateDebut)
            {
                ModelState.AddModelError("Reservation.DateFin", $"La date de fin doit être supérieure ou égale à {Reservation.DateDebut.Date:yyyy-MM-dd}.");
            }

            if (ListeDeReservations
                    .Where(r => r.ObjetDeLaReservation.Id == Reservation.ObjetDeLaReservation.Id 
                    && r.ObjetDeLaReservation.GetType() == Reservation.ObjetDeLaReservation.GetType() 
                    && r.Id != Reservation.Id)
                    .Any(r => r.DateFin >= Reservation.DateDebut && r.DateDebut <= Reservation.DateFin))
            {
                ModelState.AddModelError("Reservation.ObjetDeLaReservation", "Cette voiture est déjà réservée pour ces dates.");
            }

            if (!ModelState.IsValid) return Page();

            GestionReservable.ModifierReservable(Reservation);
            return RedirectToPage("Index");
        }

        private void ChargementDePage()
        {
            ListeDeVoitures = [];
            List<Voiture> ListeToutesVoitures = [.. GestionReservable.ObtenirListeReservable("Voiture").Cast<Voiture>()];

            int anneeMax = DateTime.Now.Year;
            int anneeMin = anneeMax - 10;

            foreach (var voiture in ListeToutesVoitures)
            {
                if (voiture.AnneeFabrication > anneeMin)
                {
                    ListeDeVoitures.Add(voiture);
                }
            }
        }
    }
}
