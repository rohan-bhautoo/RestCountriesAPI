using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    public static async Task Main(string[] args)
    {
        List<Country> countries = await GetCountriesAsync();

        /*var filteredCountries = FilterCountriesByRegion(countries, new List<string> { "Europe", "North America" });
        Console.WriteLine("Countries in Europe and North America:");
        foreach (var country in filteredCountries)
        {
            Console.WriteLine(country.Name.Common);
        }*/

        var europeCountries = countries.FindAll(c => c.Region == "Europe");
        var centralAmericaCountries = countries.FindAll(c => c.Subregion == "Central America");

        Console.WriteLine("Europe Countries:");
        foreach (var country in europeCountries)
        {
            Console.WriteLine(country.Name.Common);
        }

        Console.WriteLine("\nCentral America Countries:");
        foreach (var country in centralAmericaCountries)
        {
            Console.WriteLine(country.Name.Common);
        }

        
    }

    public static async Task<List<Country>> GetCountriesAsync()
    {
        try
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync("https://restcountries.com/v3/all");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var countries = JsonSerializer.Deserialize<List<Country>>(responseContent);

            return countries;
        } 
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<Country>();
        }
    }

    /*public static IEnumerable<Country> FilterCountriesByRegion(List<Country> countries, List<string> regions)
    {
        return countries.Where(country => regions.Contains(country.Region));
    }*/
}

public class Country
{
    [JsonPropertyName("name")]
    public Name Name { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }

    [JsonPropertyName("subregion")]
    public string Subregion { get; set; }
    // Add other properties as needed
}

public class Name
{
    [JsonPropertyName("common")]
    public string Common { get; set; }

    [JsonPropertyName("official")]
    public string Official { get; set; }

    [JsonPropertyName("nativeName")]
    public Dictionary<string, Translation> NativeName { get; set; }
}

public class Translation
{
    [JsonPropertyName("official")]
    public string Official { get; set; }

    [JsonPropertyName("common")]
    public string Common { get; set; }
}