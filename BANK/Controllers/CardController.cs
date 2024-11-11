using BANK.Data.Entities;
using BANK.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BANK.Controllers
{
    public class CardController : Controller
    {
        private readonly BankDbContext _context;

        public CardController(BankDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(int accountId)
        {
            ViewBag.Account = accountId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Card card)
        {
            if (ModelState.IsValid)
            {
                _context.Cards.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = card.Id });
            }
            return View(card);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var card = await _context.Cards
                .Include(c => c.Account)
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (card == null) return NotFound();

            return View(card);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            int accountId = card.AccountId;

            if (card == null) return NotFound();

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Account", new { id = accountId });
        }
    }
}
