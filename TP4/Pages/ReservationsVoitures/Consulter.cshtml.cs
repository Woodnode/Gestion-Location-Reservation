using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Models;
using TP4.Services;

namespace TP4.Pages.ReservationsVoitures
{
    public class ConsulterModel : PageModel
    {
        [BindProperty]
        public required Reservation Reservation { get; set; }

        public required Voiture Voiture { get; set; }

        public ActionResult OnGet(int id)
        {
            if (id == null) return NotFound();

            Reservation = (Reservation)GestionReservable.ObtenirReservableParId("Reservation", id);

            Voiture = (Voiture)GestionReservable.ObtenirReservableParId("Voiture", Reservation.ObjetDeLaReservation.Id);

            return Page();
        }
    }
}
