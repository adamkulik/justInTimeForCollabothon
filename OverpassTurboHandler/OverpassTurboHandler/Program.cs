using OverpassTurboHandler;
using System.Diagnostics;

public class Program
{
    private static void Main(string[] args)
    {
        Dictionary<string, string> countryNames =
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
        string[] countryCodes = { "PL", "DE", "CZ", };
        var postRequest = new Level2PostRequest("localhost", "12345");
        Stopwatch sw = Stopwatch.StartNew();
        string a = postRequest.GetCountryBoundaries(countryNames.Keys.ToArray());
        sw.Stop();
        Console.WriteLine(sw.Elapsed);

    }
}