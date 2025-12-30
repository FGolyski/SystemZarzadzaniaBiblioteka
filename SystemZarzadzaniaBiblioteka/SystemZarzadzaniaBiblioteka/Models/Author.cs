using System.ComponentModel.DataAnnotations;

namespace SystemZarzadzaniaBiblioteka.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(50)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(50)]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Autor")]
        public string FullName => $"{FirstName} {LastName}";

  
        public virtual ICollection<Book>? Books { get; set; }
    }
}
