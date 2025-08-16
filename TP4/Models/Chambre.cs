using System.ComponentModel.DataAnnotations;

namespace TP4.Models
{
    public class Chambre : IReservable
    {
        [Key]
        [Display(Name = "Identifiant")]
        [Required(ErrorMessage = "Le champ Identifiant est obligatoire")]
        public int Id { get; set; }

        [Required(ErrorMessage = "La Description est obligatoire.")]
        [MaxLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères.")]
        public required string Description { get; set; }


        [Required(ErrorMessage ="Le Prix Journalier est obligatoire.")]
        [Range(0, 500, ErrorMessage = "Le prix journalier doit-être compris entre 0 et 500.")]
        public required int PrixJournalier { get; set; }
    }
}
