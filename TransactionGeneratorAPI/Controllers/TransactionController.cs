using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using OverpassTurboHandler;
using System.Diagnostics;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
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
            var postRequest = new Level2PostRequest("172.17.0.2", "80");
            //var postRequest = new Level2PostRequest("localhost", "80");
            var transactions = GenerateSampleTransactions(seed);
            string report = TransactionReportGenerator.GenerateReport(transactions);
            string[] countryCodes = transactions.Select(x => TransactionReportGenerator.ExtractCountryCodeFromIBAN(x.Customer.IBANNumber)).Distinct().ToArray();
            string osmResult = postRequest.GetCountryBoundaries(countryCodes);
            System.IO.File.WriteAllText("/home/azureuser/workingDir/mapData.osm", osmResult);
            BashHelper.RunCommandWithBash("/home/azureuser/osmToSvg.sh");
            //string svgImage = System.IO.File.ReadAllText("C:\\Users\\Kinga\\source\\repos\\justInTimeForCollabothon\\TransactionGeneratorAPI\\outImage.svg");
            string svgImage = System.IO.File.ReadAllText("/home/azureuser/outImage.svg");
            List<CountryTransactions> countryTransactions = JsonSerializer.Deserialize<List<CountryTransactions>>(report);
            List<float> percentageOfAllTransactions = new List<float>();
            for(int i = 0; i < countryCodes.Length; i++)
            {
                float percentage = (float)countryTransactions[i].NumberOfTransactions / (float)transactions.Count * 100.0f;
                percentageOfAllTransactions.Add(percentage);
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("/home/azureuser/outImage.svg");
            // Przestrzeń nazw SVG

            // Znalezienie wszystkich elementów 'path' i ustawienie atrybutu 'fill' na 'blue'
            var pathElements = xmlDocument.SelectNodes("/*[name()=\"svg\"]/*[name()=\"g\"]/*[name()=\"path\"]");
            foreach (var path in pathElements)
            {
                XmlElement pathCasted;
                pathCasted = (XmlElement)path;
                pathCasted.SetAttribute("fill", "blue");
            }
            // Zapisanie zmodyfikowanego pliku SVG
            string newFilePath = "/home/azureuser/outImage.svg\"modified_Image.svg";
            xmlDocument.Save(newFilePath);
            return Ok(svgImage);
        }

        // Funkcja pomocnicza do generowania przykładowych transakcji
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
