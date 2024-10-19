using Microsoft.AspNetCore.Mvc;
using TransactionGeneratorAPI.Models;

namespace TransactionGeneratorAPI.Controllers
{
    // Kontroler API
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        [HttpGet("country-report")]
        public IActionResult GetCountryReport([FromQuery(Name = "seed")] int seed)
        {
            var transactions = GenerateSampleTransactions(seed);

            string report = TransactionReportGenerator.GenerateReport(transactions);
            return Ok(report);
        }

        [HttpGet("returning-customers-report")]
        public IActionResult GetReturningCustomerReport([FromQuery(Name = "seed")] int seed)
        {
            var transactions = GenerateSampleTransactions(seed);

            string report = TransactionReportGenerator.GenerateReturningCustomerReport(transactions);
            return Ok(report);
        }

        // Funkcja pomocnicza do generowania przyk³adowych transakcji
        private List<Transaction> GenerateSampleTransactions(int seed = 50)
        {
            List<Customer> customers = new();
            Random random = new(seed);
            List<string> countryCodes = CountriesData.countryData.Keys.ToList();

            for (int i = 0; i < 20; i++)
            {
                string countryCode = countryCodes[random.Next(countryCodes.Count)];
                string iban = TransactionReportGenerator.GenerateIBAN(countryCode);
                Customer customer = TransactionReportGenerator.GenerateCustomer(iban);
                customers.Add(customer);
            }

            List<Transaction> transactions = new();
            for (int i = 0; i < 50; i++)
            {
                string countryCode = countryCodes[random.Next(countryCodes.Count)];
                int index = random.Next(customers.Count - 1);
                Transaction transaction = TransactionReportGenerator.GenerateTransaction(customers[index]);
                transactions.Add(transaction);
            }

            return transactions;
        }

    }
}
