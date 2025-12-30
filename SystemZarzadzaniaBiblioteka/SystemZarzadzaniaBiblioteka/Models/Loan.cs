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


        public int BookId { get; set; }
        public virtual Book? Book { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
    }
}
