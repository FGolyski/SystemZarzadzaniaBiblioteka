using Microsoft.AspNetCore.Identity;

namespace SystemZarzadzaniaBiblioteka.Models
{
    public class ApplicationUser : IdentityUser
    {
    
        public virtual ICollection<Loan>? Loans { get; set; }
    }
}
