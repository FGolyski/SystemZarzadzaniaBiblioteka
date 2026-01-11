using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemZarzadzaniaBiblioteka.Data;
using SystemZarzadzaniaBiblioteka.Models;

namespace SystemZarzadzaniaBiblioteka.Controllers
{
    [Authorize]
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoansController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Lista wypożyczeń (Admin widzi wszystko, User swoje)
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (User.IsInRole("Admin"))
            {
                return View(await _context.Loans.Include(l => l.Book).Include(l => l.User).ToListAsync());
            }
            else
            {
                return View(await _context.Loans.Include(l => l.Book).Where(l => l.UserId == user.Id).ToListAsync());
            }
        }

        // Akcja tworzenia wypożyczenia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            var loan = new Loan { BookId = bookId, UserId = user.Id, LoanDate = DateTime.Now };
            _context.Add(loan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}