using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace TP4.Models
{
    public class Reservation : IReservable
    {
        [Key]
        [Display(Name = "Identifiant")]
        [Required(ErrorMessage = "Le champ Identifiant est obligatoire")]
        public int Id { get; set; }

        [Display(Name = "Date de début")]
        [Required(ErrorMessage = "La date de début est obligatoire.")]
        public DateTime DateDebut { get; set; }

        [Display(Name = "Date de fin")]
        [Required(ErrorMessage = "La date de fin est obligatoire.")]
        public DateTime DateFin { get; set; }

        [Display(Name = "Objet de la réservation")]
        public IReservable? ObjetDeLaReservation { get; set; }

        [Required(ErrorMessage = "Le Prix Journalier est obligatoire.")]
        public int Prix { get; set; }

        public int PrixJournalier { get; set; }
    }
}
