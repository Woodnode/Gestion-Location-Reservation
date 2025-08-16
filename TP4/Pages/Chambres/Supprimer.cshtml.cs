using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;

namespace TP4.Pages.Chambres
{
    public class SupprimerModel : PageModel
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

        public ActionResult OnPost(int id)
        {

            if (GestionReservable.ObtenirReservableParId("Chambre", id) is not Chambre chambre) return NotFound();
            Chambre = chambre;

            if (VerifierReservation(Chambre)) ModelState.AddModelError("Chambre", "La chambre est réservée et ne peut pas être supprimée.");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            GestionReservable.SupprimerReservable(Chambre);

            return RedirectToPage("Index");
        }
        public bool VerifierReservation(Chambre chambre) => GestionReservable.EstReserve(chambre.Id, chambre.GetType().Name);

    }
}
