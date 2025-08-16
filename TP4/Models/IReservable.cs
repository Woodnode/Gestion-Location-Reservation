using System.ComponentModel.DataAnnotations;

namespace TP4.Models
{
    public interface IReservable
    {
        [Key]
        public int Id { get; set; }

        public int PrixJournalier { get; set; }
    }
}
