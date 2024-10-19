using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using OverpassTurboHandler;
using System.Diagnostics;
using System.Text.Json;
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

        [HttpGet("getSVGMap")]
        public IActionResult GetSVGMap([FromQuery(Name = "seed")] int seed)
        {
            var postRequest = new Level2PostRequest("localhost", "12345");
            var transactions = GenerateSampleTransactions(seed);
            string report = TransactionReportGenerator.GenerateReport(transactions);
            string[] countryCodes = transactions.Select(x => TransactionReportGenerator.ExtractCountryCodeFromIBAN(x.Customer.IBANNumber)).Distinct().ToArray();
            string osmResult = postRequest.GetCountryBoundaries(countryCodes);
            System.IO.File.WriteAllText("/home/azureuser/workingDir/mapData.osm", osmResult);
            BashHelper.RunCommandWithBash("/home/azureuser/osmToSvg.sh");
            string svgImage = System.IO.File.ReadAllText("/home/azureuser/workingDir/outImage.svg");
            return Ok(svgImage);
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
