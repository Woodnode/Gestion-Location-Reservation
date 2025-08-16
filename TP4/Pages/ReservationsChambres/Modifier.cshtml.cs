using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;
using System.ComponentModel.DataAnnotations;

namespace TP4.Pages.ReservationsChambres
{
    public class ModifierModel : PageModel
    {
        [BindProperty]
        public required Reservation Reservation { get; set; }

        public required List<Chambre> ListeDeChambres { get; set; }

        public required List<Reservation> ListeDeReservations { get; set; }

        [Required(ErrorMessage = "Veuillez sélectionner une chambre.")]
        [BindProperty]
        public int IdChambre { get; set; }

        public ActionResult OnGet(int? id)
        {
            if (id == null ||
                GestionReservable.ObtenirReservableParId("Reservation", id.Value) is not Reservation reservation || 
                reservation is null ||
                reservation.ObjetDeLaReservation is null || 
                GestionReservable.ObtenirReservableParId("Chambre", reservation.ObjetDeLaReservation.Id) is not Chambre)
                return NotFound();

            Reservation = reservation;
            IdChambre = reservation.ObjetDeLaReservation.Id;
            ListeDeChambres = [.. GestionReservable.ObtenirListeReservable("Chambre").Cast<Chambre>()];

            return Page();
        }

        public ActionResult OnPost()
        {
            ListeDeChambres = [.. GestionReservable.ObtenirListeReservable("Chambre").Cast<Chambre>()];
            ListeDeReservations = [.. GestionReservable.ObtenirListeReservable("Reservation").Cast<Reservation>()];

            Chambre chambre = ListeDeChambres.First(v => v.Id == IdChambre);
            if (chambre == null) ModelState.AddModelError("IdChambre", "Veuillez sélectionner une chambre.");
            else
            {
                Reservation.ObjetDeLaReservation = chambre;
                Reservation.PrixJournalier = chambre.PrixJournalier;
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
                ModelState.AddModelError("Reservation.ObjetDeLaReservation", "Cette chambre est déjà réservée pour ces dates.");
            }

            if (!ModelState.IsValid) return Page();

            GestionReservable.ModifierReservable(Reservation);
            return RedirectToPage("Index");
        }
    }
}
