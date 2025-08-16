using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;

namespace TP4.Pages.Voitures
{
    public class IndexModel : PageModel
    {
        public required List<Voiture> Voitures { get; set; }

        public ActionResult OnGet()
        {
            GestionReservable.ChargerListeDepuisFichier();

            var voitures = GestionReservable.ObtenirListeReservable("Voiture").Cast<Voiture>().ToList();
            Voitures = voitures.Count != 0 ? voitures : [];

            return Page();
        }

        public string ReduireDescription(Voiture voiture) => voiture.Description.Length > 50 ? 
            string.Concat(voiture.Description.AsSpan(0, 50), "...") : voiture.Description;

        public bool VerifierReservation(Voiture voiture) => GestionReservable.EstReserve(voiture.Id, voiture.GetType().Name);
    }
}
