using BANK.Data;
using BANK.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BANK.Controllers
{
    public class TransactionController : Controller
    {
        private readonly BankDbContext _context;

        public TransactionController(BankDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create(int cardId)
        {
            ViewBag.CardId = cardId;
            ViewBag.PayeeId = this._context.Cards.FirstOrDefault(c => c.Id == cardId)!.AccountId;
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Card", new { id = transaction.CardId });
            }
            return View(transaction);
        }
    }
}
