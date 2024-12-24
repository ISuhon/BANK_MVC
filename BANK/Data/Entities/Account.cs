namespace BANK.Data.Entities
{
    public enum Currency
    {
        USD,
        EUR,
        UAH,
        PLN
    }
    public class Account
    {
        public int Id { get; set; }

        public Currency Currency { get; set; }

        public string ClientId { get; set; }
        public ApplicationUser? Client { get; set; }

        public ICollection<Card>? Cards { get; set;}
    }
}
