using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;


namespace TP4.Pages.ReservationsChambres
{
    public class IndexModel : PageModel
    {
        public required List<Reservation> Reservations { get; set; } 

        public ActionResult OnGet(string tri)
        {
            if (tri != null)
            {
                Reservations = GestionReservable.TrierListeReservation(tri, "Chambre");
                return Page();
            }

            GestionReservable.ChargerListeDepuisFichier();

            var reservations = GestionReservable.ObtenirListeReservable("Reservation").Cast<Reservation>().Where(r => r.ObjetDeLaReservation.GetType().Name == "Chambre").ToList();

            Reservations = reservations.Count != 0 ? reservations : [];

            return Page();
        }
    }
}
