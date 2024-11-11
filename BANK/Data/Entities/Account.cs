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

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public ICollection<Card> Cards { get; set;}
    }
}
