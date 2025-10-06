using System.Net.Http;
using System.Text.Json;
using FinancePortfolioAnalyzer.Models;

namespace FinancePortfolioAnalyzer.Services;

public class AlphaVantageService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient = new();
    private readonly string _dataPath = "Data";

    public AlphaVantageService(string apiKey)
    {
        _apiKey = apiKey;
        Directory.CreateDirectory(_dataPath); // crée le dossier s'il n'existe pas
    }

    public async Task<List<StockPrice>> GetDailyPricesAsync(string symbol)
    {
        string cacheFile = Path.Combine(_dataPath, $"{symbol}.json");

        // ✅ Étape 1 : si le fichier de cache existe, on le lit
        if (File.Exists(cacheFile))
        {
            Console.WriteLine($"💾 Lecture des données en cache : {cacheFile}");
            string cachedJson = await File.ReadAllTextAsync(cacheFile);
            return ParseStockPrices(cachedJson);
        }

        // 🌐 Étape 2 : sinon, on appelle l'API
        Console.WriteLine("🌐 Téléchargement depuis Alpha Vantage...");
        
        // CORRECTION : Utiliser la clé API et le symbole dynamique
        string url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={_apiKey}";

        try
        {
            var response = await _httpClient.GetStringAsync(url);

            // Vérifier si la réponse contient une erreur
            if (response.Contains("Error Message") || response.Contains("Invalid API"))
            {
                Console.WriteLine("❌ Erreur API Alpha Vantage");
                return new List<StockPrice>();
            }

            // Sauvegarde du JSON brut dans le cache
            await File.WriteAllTextAsync(cacheFile, response);

            // Parsing et retour des données
            return ParseStockPrices(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erreur lors de l'appel API : {ex.Message}");
            return new List<StockPrice>();
        }
    }

    private List<StockPrice> ParseStockPrices(string json)
    {
        try
        {
            var jsonDoc = JsonDocument.Parse(json);

            // CORRECTION : Vérifier d'abord les erreurs API
            if (jsonDoc.RootElement.TryGetProperty("Error Message", out var errorMsg))
            {
                Console.WriteLine($"❌ Erreur API : {errorMsg.GetString()}");
                return new List<StockPrice>();
            }

            if (jsonDoc.RootElement.TryGetProperty("Note", out var note))
            {
                Console.WriteLine($"⚠️ Note API : {note.GetString()}");
                return new List<StockPrice>();
            }

            // CORRECTION : Vérifier la clé correcte pour les données quotidiennes
            if (!jsonDoc.RootElement.TryGetProperty("Time Series (Daily)", out var timeSeries))
            {
                Console.WriteLine("❌ Format de données non reconnu ou données manquantes");
                return new List<StockPrice>();
            }

            var prices = new List<StockPrice>();
            foreach (var day in timeSeries.EnumerateObject())
            {
                try
                {
                    var date = DateTime.Parse(day.Name);
                    
                    // CORRECTION : Gérer les valeurs nulles
                    var closeElement = day.Value.GetProperty("4. close");
                    if (closeElement.ValueKind == JsonValueKind.String)
                    {
                        var closeString = closeElement.GetString();
                        if (decimal.TryParse(closeString, out decimal close))
                        {
                            prices.Add(new StockPrice { Date = date, Close = close });
                        }
                        else
                        {
                            Console.WriteLine($"⚠️ Impossible de parser le prix pour {date:yyyy-MM-dd}: {closeString}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Erreur lors du parsing des données pour {day.Name}: {ex.Message}");
                }
            }

            return prices.OrderBy(p => p.Date).ToList();
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"❌ Erreur de parsing JSON : {ex.Message}");
            return new List<StockPrice>();
        }
    }
}