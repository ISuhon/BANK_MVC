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
        public async Task<IActionResult> Details(int id, int page = 1, int pageSize = 10, string search = "", string sortBy = "ExpirationDate", bool sortDescending = false)
        {
            var account = await _context.Accounts
                .Include(a => a.Cards)
                .Include(a => a.Client)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account == null) return NotFound();

            var filteredCards = account.Cards.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                filteredCards = filteredCards.Where(c =>
                    c.Fortune.ToString().Contains(search) ||
                    c.ExpirationDate.ToString("MM/yy").Contains(search));
            }

            filteredCards = sortBy switch
            {
                "ExpirationDate" => sortDescending
                    ? filteredCards.OrderByDescending(c => c.ExpirationDate)
                    : filteredCards.OrderBy(c => c.ExpirationDate),
                "Fortune" => sortDescending
                    ? filteredCards.OrderByDescending(c => c.Fortune)
                    : filteredCards.OrderBy(c => c.Fortune),
                _ => filteredCards
            };

            var totalCards = filteredCards.Count();
            filteredCards = filteredCards.Skip((page - 1)* pageSize).Take(pageSize);

            account.Cards = filteredCards.ToList();

            var totalFortune = account.Cards.Sum(c => c.Fortune);
            ViewBag.TotalFortune = totalFortune;
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDescending = sortDescending;
            ViewBag.CurrentPage = page; 
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCards / pageSize);

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
