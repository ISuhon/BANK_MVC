namespace BANK.Data.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Account>? Accounts { get; set; }
    }
}
