using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;

namespace TP4.Pages.Voitures
{
    public class ConsulterModel : PageModel
    {
        [BindProperty]
        public required Voiture Voiture { get; set; }

        public ActionResult OnGet(int? id)
        {
            if (id == null) return NotFound();

            Voiture = (Voiture)GestionReservable.ObtenirReservableParId("Voiture", id.Value);

            return Page();
        }
        public bool VerifierReservation(Voiture voiture) => GestionReservable.EstReserve(voiture.Id, voiture.GetType().Name);

    }
}

