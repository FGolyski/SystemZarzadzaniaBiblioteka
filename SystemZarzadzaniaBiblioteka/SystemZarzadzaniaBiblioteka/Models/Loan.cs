using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SystemZarzadzaniaBiblioteka.Models
{
    public class Loan
    {
        public int Id { get; set; }

        [Display(Name = "Data wypożyczenia")]
        public DateTime LoanDate { get; set; } = DateTime.Now;

        [Display(Name = "Data zwrotu")]
        public DateTime? ReturnDate { get; set; }

        [Display(Name = "Termin zwrotu")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Czy przedłużono?")]
        public bool IsExtended { get; set; } = false;
        // --------------------------------

        public int BookId { get; set; }
        public virtual Book? Book { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
    }
}