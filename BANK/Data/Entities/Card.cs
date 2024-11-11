namespace BANK.Data.Entities
{
    public class Card
    {
        public int Id { get; set; }
        public long CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public short CVVcode { get; set; }
        public short PIN { get; set; }
        public decimal Fortune { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }

        public int AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
