using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SystemZarzadzaniaBiblioteka.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [StringLength(100)]
        [Display(Name = "Tytuł Książki")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Rok wydania")]
        [Range(1000, 2100, ErrorMessage = "Podaj poprawny rok")]
        public int ReleaseYear { get; set; }

        [Display(Name = "Opis")]
        public string? Description { get; set; }

        [Display(Name = "Autor")]
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Author? Author { get; set; }

        public virtual ICollection<Loan>? Loans { get; set; }
    }
}
