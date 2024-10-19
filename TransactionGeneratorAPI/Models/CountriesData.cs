namespace TransactionGeneratorAPI.Models
{
    public class CountriesData
    {
        public static readonly Dictionary<string, (int ibanLength, int bankCodeLength, int accountNumberLength)> countryData =
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
        public static readonly Dictionary<string, string> countryNames =
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
    }
}
