using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations;

namespace TP4.Pages.ReservationsChambres
{
    public class AjouterModel : PageModel
    {
        [BindProperty]
        public required Reservation Reservation { get; set; }
        
        public int ReservationId { get; set; }

        public required List<Reservation> ListeDeReservations { get; set; }

        public required List<Chambre> ListeDeChambres { get; set; }

        [Required(ErrorMessage = "Veuillez sélectionner une chambre.")]
        [BindProperty]
        public required int IdChambre { get; set; }

        public ActionResult OnGet()
        {
            ChargementDePage();
            return Page();
        }

        public ActionResult OnPost()
        {
            ChargementDePage();

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
                ModelState.AddModelError("Reservation.DateDebut", $"La date de début doit être supérieure ou égale à {DateTime.Now.Date:yyyy-MM -dd}.");
            }
            if (Reservation.DateFin < Reservation.DateDebut)
            {
                ModelState.AddModelError("Reservation.DateFin", $"La date de fin doit être supérieure ou égale à {Reservation.DateDebut.Date:yyyy-MM -dd}.");
            }

            if (ListeDeReservations.Where(r => r.ObjetDeLaReservation.Id == Reservation.ObjetDeLaReservation.Id 
            && r.ObjetDeLaReservation.GetType() == Reservation.ObjetDeLaReservation.GetType())
                .Any(r => r.DateFin >= Reservation.DateDebut && r.DateDebut <= Reservation.DateFin))
            {
                ModelState.AddModelError("Reservation.ObjetDeLaReservation", "Cette chambre est déjà réservée pour ces dates.");
            }

            if (!ModelState.IsValid) return Page();

            GestionReservable.AjouterReservable(Reservation);

            return RedirectToPage("Index");
        }

        private void ChargementDePage()
        {
            ReservationId = GestionReservable.GenererId("Reservation");
            ListeDeReservations = [.. GestionReservable.ObtenirListeReservable("Reservation").Cast<Reservation>()];
            ListeDeChambres = [.. GestionReservable.ObtenirListeReservable("Chambre").Cast<Chambre>()];
        }
    }
}
