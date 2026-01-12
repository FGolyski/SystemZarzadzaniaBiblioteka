using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemZarzadzaniaBiblioteka.Data;
using SystemZarzadzaniaBiblioteka.Models;

namespace SystemZarzadzaniaBiblioteka.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> UserLoans(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return NotFound();

            var loans = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();

            var user = await _userManager.FindByIdAsync(userId);
            ViewData["UserName"] = user?.Email ?? "Użytkownika";

            return View(loans);
        }
    }
}