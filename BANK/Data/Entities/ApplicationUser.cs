using Microsoft.AspNetCore.Identity;

namespace BANK.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public ICollection<Account>? Accounts { get; set; }
    }
}
