using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json; // Required for JSON serialization
using System.IO; // Required for file handling
using System.Numerics;

internal class Transaction
{
    public string TransactionId { get; set; }
    public DateTime DateOfTransaction { get; set; }
    public string IBANNumber { get; set; }
    public string Name { get; set; }
    public float Amount { get; set; }
    public string Currency { get; set; }
    public string Title { get; set; }
}

class Program
{
    // A dictionary to store IBAN length, bank code length, and account number length for each country
    private static readonly Dictionary<string, (int ibanLength, int bankCodeLength, int accountNumberLength)> countryData =
        new Dictionary<string, (int, int, int)>()
    {
        { "DE", (22, 8, 10) },  // Germany
        { "GB", (22, 4, 14) },  // United Kingdom
        { "FR", (27, 5, 11) },  // France
        { "IT", (27, 5, 12) },  // Italy
        { "ES", (24, 4, 16) },  // Spain
        { "NL", (18, 4, 10) },  // Netherlands
        { "BE", (16, 3, 9) },   // Belgium
        { "GR", (27, 7, 16) },  // Greece
        { "PT", (25, 4, 13) },  // Portugal
        { "DK", (18, 4, 10) },  // Denmark
        { "SE", (24, 3, 17) },  // Sweden
        { "FI", (18, 6, 8) },   // Finland
        { "PL", (28, 8, 16) },  // Poland
        { "CZ", (24, 4, 16) },  // Czech Republic
        { "HU", (28, 3, 16) },  // Hungary
        { "NO", (15, 4, 6) },   // Norway
        { "IE", (22, 4, 14) },  // Ireland
        { "CH", (21, 5, 12) },  // Switzerland
        { "AT", (20, 5, 11) },  // Austria
        { "LU", (20, 3, 13) }   // Luxembourg
    };

    // A dictionary to store country codes and their corresponding full country names
    private static readonly Dictionary<string, string> countryNames =
        new Dictionary<string, string>()
    {
        { "DE", "Germany" },
        { "GB", "United Kingdom" },
        { "FR", "France" },
        { "IT", "Italy" },
        { "ES", "Spain" },
        { "NL", "Netherlands" },
        { "BE", "Belgium" },
        { "GR", "Greece" },
        { "PT", "Portugal" },
        { "DK", "Denmark" },
        { "SE", "Sweden" },
        { "FI", "Finland" },
        { "PL", "Poland" },
        { "CZ", "Czech Republic" },
        { "HU", "Hungary" },
        { "NO", "Norway" },
        { "IE", "Ireland" },
        { "CH", "Switzerland" },
        { "AT", "Austria" },
        { "LU", "Luxembourg" }
    };

    // Generate a random IBAN number for a given country code
    public static string GenerateIBAN(string countryCode)
    {
        if (!countryData.ContainsKey(countryCode))
        {
            throw new ArgumentException("Invalid country code");
        }

        var (ibanLength, bankCodeLength, accountNumberLength) = countryData[countryCode];

        string bankCode = GenerateRandomNumberString(bankCodeLength);
        string accountNumber = GenerateRandomNumberString(accountNumberLength);

        string bban = bankCode + accountNumber;
        string ibanWithoutCheckDigits = countryCode + "00" + bban;

        string numericIBAN = ConvertToNumericIBAN(ibanWithoutCheckDigits);
        int checkDigits = 98 - (int)(BigInteger.Parse(numericIBAN) % 97);
        return countryCode + checkDigits.ToString("D2") + bban;
    }

    // Generate a random numeric string
    private static string GenerateRandomNumberString(int length)
    {
        Random random = new Random();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            sb.Append(random.Next(0, 10));
        }
        return sb.ToString();
    }

    // Convert IBAN to numeric form for modulus calculation
    private static string ConvertToNumericIBAN(string iban)
    {
        StringBuilder numericIBAN = new StringBuilder();
        string rearrangedIBAN = iban.Substring(4) + iban.Substring(0, 4);

        foreach (char ch in rearrangedIBAN)
        {
            if (char.IsLetter(ch))
            {
                numericIBAN.Append((int)ch - 55); // A=10, B=11, ..., Z=35
            }
            else
            {
                numericIBAN.Append(ch);
            }
        }

        return numericIBAN.ToString();
    }

    // Generate a random transaction
    public static Transaction GenerateTransaction(string iban)
    {
        Random random = new Random();
        List<string> currencies = new List<string> { "USD", "EUR", "GBP", "JPY", "CAD" };
        List<string> transactionTitles = new List<string> { "Payment", "Refund", "Invoice", "Salary", "Transfer" };
        List<string> names = new List<string> { "John Doe", "Jane Smith", "Alice Brown", "Bob Johnson", "Charlie Davis" };

        return new Transaction
        {
            TransactionId = Guid.NewGuid().ToString(), // Unique transaction ID
            DateOfTransaction = DateTime.Now.AddDays(-random.Next(0, 365)), // Random date within the last year
            IBANNumber = iban,
            Name = names[random.Next(names.Count)], // Random name
            Amount = (float)(random.NextDouble() * 10000), // Random amount between 0 and 10,000
            Currency = currencies[random.Next(currencies.Count)], // Random currency
            Title = transactionTitles[random.Next(transactionTitles.Count)] // Random transaction title
        };
    }

    // Extract country code from IBAN
    public static string ExtractCountryCodeFromIBAN(string iban)
    {
        return iban.Substring(0, 2); // First two characters of IBAN are the country code
    }

    // Generate a report that summarizes transactions per country
    public static string GenerateReport(List<Transaction> transactions)
    {
        // Group transactions by country
        var reportData = transactions
            .GroupBy(t => ExtractCountryCodeFromIBAN(t.IBANNumber))
            .Select(group => new
            {
                Country = countryNames[group.Key],  // Use the country name instead of the code
                NumberOfTransactions = group.Count(),
                TotalSum = group.Sum(t => t.Amount)
            })
            .ToList();

        // Serialize the report to JSON format
        string jsonReport = JsonSerializer.Serialize(reportData, new JsonSerializerOptions { WriteIndented = true });
        return jsonReport;
    }

    // Save the JSON report to a file
    public static void SaveReportToFile(string jsonReport, string fileName)
    {
        File.WriteAllText(fileName, jsonReport);
    }

    static void Main(string[] args)
    {
        // Generate 10 transactions
        List<Transaction> transactions = new List<Transaction>();

        // List of country codes for generating IBAN numbers
        Random random = new Random();
        List<string> countryCodes = countryData.Keys.ToList();

        for (int i = 0; i < 100; i++)
        {
            // Generate a random IBAN from one of the country codes
            string countryCode = countryCodes[random.Next(countryCodes.Count)];
            string iban = GenerateIBAN(countryCode);

            // Generate a transaction with the IBAN
            Transaction transaction = GenerateTransaction(iban);
            transactions.Add(transaction);
        }

        // Print out the transactions
        foreach (var transaction in transactions)
        {
            Console.WriteLine($"Transaction ID: {transaction.TransactionId}");
            Console.WriteLine($"Date: {transaction.DateOfTransaction}");
            Console.WriteLine($"IBAN: {transaction.IBANNumber}");
            Console.WriteLine($"Name: {transaction.Name}");
            Console.WriteLine($"Amount: {transaction.Amount} {transaction.Currency}");
            Console.WriteLine($"Title: {transaction.Title}");
            Console.WriteLine();
        }

        // Generate and print the JSON report
        string report = GenerateReport(transactions);
        Console.WriteLine("JSON Report:");
        Console.WriteLine(report);

        // Save the JSON report to a file
        string fileName = "TransactionReport.json";
        SaveReportToFile(report, fileName);
        Console.WriteLine($"Report saved to file: {fileName}");
    }
}

