namespace TransactionGeneratorAPI.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string IBANNumber { get; set; }

        public override string ToString()
        {
            return CustomerId + ":" + Name + ":" + IBANNumber;
        }
    }
}
