namespace TransactionGeneratorAPI.Models
{
    public class CountryTransactions
    {
        public string Country { get; set; }
        public int NumberOfTransactions { get; set; }
        public float TotalSum { get; set; }
    }
}
