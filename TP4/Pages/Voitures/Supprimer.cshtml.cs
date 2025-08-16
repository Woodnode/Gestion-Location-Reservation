using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;

namespace TP4.Pages.Voitures
{
    public class SupprimerModel : PageModel
    {
        [BindProperty]
        public required Voiture Voiture { get; set; }

        public ActionResult OnGet(int? id)
        {
            if (id == null) return NotFound();

            Voiture = (Voiture)GestionReservable.ObtenirReservableParId("Voiture", id.Value);

            return Page();
        }

        public ActionResult OnPost()
        {
            if (VerifierReservation(Voiture)) ModelState.AddModelError("Voiture", "La voiture est réservée et ne peut pas être supprímée.");
            if (!ModelState.IsValid) return Page();
            GestionReservable.SupprimerReservable(Voiture);

            return RedirectToPage("Index");
        }

        public bool VerifierReservation(Voiture voiture) => GestionReservable.EstReserve(voiture.Id, voiture.GetType().Name);
    }
}

