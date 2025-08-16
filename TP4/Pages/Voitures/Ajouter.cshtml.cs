using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TP4.Services;
using TP4.Models;

namespace TP4.Pages.Voitures
{
    public class AjouterModel : PageModel
    {
        [BindProperty]
        public required Voiture Voiture { get; set; }

        public required List<string> ListeDeMarques { get; set; }

        public ActionResult OnGet()
        {
            ListeDeMarques = GestionReservable.ObtenirListeDeMarques();
            return Page();
        }

        public ActionResult OnPost()
        {
            ListeDeMarques = GestionReservable.ObtenirListeDeMarques();

            int anneeMax = DateTime.Now.Year;
            int anneeMin = anneeMax - 10;

            if (Voiture.AnneeFabrication < anneeMin || Voiture.AnneeFabrication > anneeMax)
            {
                ModelState.AddModelError("Voiture.AnneeFabrication", $"L'année de fabrication doit être compris entre {anneeMin} et {anneeMax}");
            }
            if (!ModelState.IsValid) return Page();

            Voiture.Description = Voiture.Description.Trim().Replace(";", "");
            Voiture.Id = GestionReservable.GenererId("Voiture");
            GestionReservable.AjouterReservable(Voiture);

            return RedirectToPage("Index");
        }
    }
}
