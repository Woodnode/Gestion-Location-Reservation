using System.ComponentModel.DataAnnotations;
using System.Formats.Asn1;

namespace TP4.Models
{
    public class Voiture : IReservable
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le champ Marque est obligatoire")]
        public required string Marque { get; set; }

        [Required(ErrorMessage = "Le champ Prix Journalier est obligatoire")]
        [Range(0, 500, ErrorMessage = "Le prix journalier doit-être compris entre 0 et 500.")]
        public required int PrixJournalier { get; set; }

        [MaxLength(500, ErrorMessage = "La taille maximale du champ est de 500 caractères")]
        [Required(ErrorMessage = "Le champ Description est obligatoire")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Le champ Année de Fabrication est obligatoire.")]
        public required int AnneeFabrication { get; set; }

    }
}
