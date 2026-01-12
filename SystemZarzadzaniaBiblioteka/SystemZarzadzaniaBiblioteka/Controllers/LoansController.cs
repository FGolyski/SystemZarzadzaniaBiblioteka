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

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var user = await _userManager.GetUserAsync(User);

            var loans = _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .AsQueryable();

            if (User.IsInRole("Admin"))
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    loans = loans.Where(l => l.Book.Title.Contains(searchString)
                                           || l.User.Email.Contains(searchString));
                }

                return View(await loans.OrderByDescending(l => l.LoanDate).ToListAsync());
            }
            else
            {
                return View(await loans
                    .Where(l => l.UserId == user.Id)
                    .OrderByDescending(l => l.LoanDate)
                    .ToListAsync());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            var book = await _context.Books.FindAsync(bookId);

            if (book == null) return NotFound();

            bool alreadyBorrowed = await _context.Loans.AnyAsync(l =>
                l.BookId == bookId &&
                l.UserId == user.Id &&
                l.ReturnDate == null);

            if (alreadyBorrowed)
            {
                TempData["Error"] = "Masz już tę książkę! Nie możesz wypożyczyć drugiego egzemplarza tego samego tytułu.";
                return RedirectToAction("Index", "Books");
            }

            if (book.CopiesAvailable <= 0)
            {
                TempData["Error"] = "Przykro nam, wszystkie egzemplarze zostały wypożyczone.";
                return RedirectToAction("Index", "Books");
            }

            book.CopiesAvailable -= 1;

            var loan = new Loan
            {
                BookId = bookId,
                UserId = user.Id,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14),
                IsExtended = false
            };

            _context.Add(loan);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Pomyślnie wypożyczono książkę!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Extend(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin") && loan.UserId != user.Id)
            {
                return Forbid();
            }

            if (loan.IsExtended)
            {
                return RedirectToAction(nameof(Index));
            }

            loan.DueDate = loan.DueDate.AddDays(7);
            loan.IsExtended = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var loan = await _context.Loans.Include(l => l.Book).FirstOrDefaultAsync(l => l.Id == id);
            if (loan == null) return NotFound();

            if (loan.ReturnDate == null)
            {
                loan.ReturnDate = DateTime.Now;

                if (loan.Book != null)
                {
                    loan.Book.CopiesAvailable += 1;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}