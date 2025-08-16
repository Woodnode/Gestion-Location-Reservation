using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Models;
using TP4.Services;

namespace TP4.Pages.ReservationsChambres
{
    public class SupprimerModel : PageModel
    {
        [BindProperty]
        public required Reservation Reservation { get; set; }

        public required Chambre Chambre { get; set; }

        public ActionResult OnGet(int id)
        {
            if (id == null) return NotFound();

            Reservation = (Reservation)GestionReservable.ObtenirReservableParId("Reservation", id);

            Chambre = (Chambre)GestionReservable.ObtenirReservableParId("Chambre", Reservation.ObjetDeLaReservation.Id);

            return Page();
        }

        public ActionResult OnPost()
        {
            GestionReservable.SupprimerReservable(Reservation);
            return RedirectToPage("Index");
        }
    }
}
