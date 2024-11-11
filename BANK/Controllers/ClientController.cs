using BANK.Data;
using BANK.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BANK.Controllers
{
    public class ClientController : Controller
    {
        private readonly BankDbContext _context;

        public ClientController(BankDbContext context) 
        {
            this._context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Client client)
        {
            if (ModelState.IsValid)
            {
                
                bool emailExists = _context.Clients.Any(c => c.Email == client.Email);
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    return View(client);
                }
                
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(client);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }


            var client = this._context.Clients.FirstOrDefault(c => c.Email == email && c.Password == password);

            if (client != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, client.FullName),
                    new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
                    new Claim(ClaimTypes.Email, client.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authenticationProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var client = await _context.Clients
                .Include(c => c.Accounts)
                .ThenInclude(a => a.Cards)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null) return NotFound();

            return View(client);
        }
    }
}
