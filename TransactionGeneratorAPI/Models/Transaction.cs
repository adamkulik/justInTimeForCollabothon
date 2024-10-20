namespace TransactionGeneratorAPI.Models
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public Customer Customer { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public string Title { get; set; }
    }
}
