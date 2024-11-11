using BANK.Data.Entities;
using BANK.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace BANK.Controllers
{
    public class AccountController : Controller
    {
        private readonly BankDbContext _context;

        public AccountController(BankDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Currency currency)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var account = new Account
                {
                    ClientId = userId,
                    Currency = currency
                };

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = account.Id });
            }
            return View(currency);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.Cards)
                .Include(a => a.Client)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account == null) return NotFound();

            var totalFortune = account.Cards.Sum(c => c.Fortune);

            ViewBag.TotalFortune = totalFortune;
            return View(account);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            int clientId = account.ClientId;
            if (account == null) return NotFound();

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Client", new {id = clientId});
        }
    }
}
