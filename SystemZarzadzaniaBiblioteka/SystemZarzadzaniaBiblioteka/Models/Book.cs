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
        [Display(Name = "Autor")]
        public virtual Author? Author { get; set; }

        [Display(Name = "Całkowity nakład")]
        [Required(ErrorMessage = "Musisz podać ilość zakupionych książek")]
        [Range(1, 1000, ErrorMessage = "Nakład musi wynosić min. 1 sztukę")]
        public int TotalCopies { get; set; }

        [Display(Name = "Dostępne sztuki")]
        public int CopiesAvailable { get; set; } 

        public virtual ICollection<Loan>? Loans { get; set; }
    }
}