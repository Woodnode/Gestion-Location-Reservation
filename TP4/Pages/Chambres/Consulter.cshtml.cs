using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;

namespace TP4.Pages.Chambres
{
    public class ConsulterModel : PageModel
    {
        [BindProperty]
        public required Chambre Chambre { get; set; }

        public ActionResult OnGet(int? id)
        {
            if (id == null) return NotFound();

            if (GestionReservable.ObtenirReservableParId("Chambre", id.Value) is not Chambre chambre) return NotFound();

            Chambre = chambre;

            return Page();
        }

        public bool VerifierReservation(Chambre chambre) => GestionReservable.EstReserve(chambre.Id, chambre.GetType().Name);
    }
}
