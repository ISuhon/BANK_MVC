namespace BANK.Data.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int PayeeId{ get; set; }
        public string Receiver { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }


        public int CardId { get; set; }
        public Card? Card { get; set; }
    }
}
