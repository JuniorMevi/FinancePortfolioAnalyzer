using FinancePortfolioAnalyzer.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Configuration
string apiKey = "C69Z7VV1L21BQQOZ";
var api = new AlphaVantageService(apiKey);

Console.WriteLine("=== Finance Portfolio App ===");
Console.Write("Entrez le symbole boursier (ex: AAPL, MSFT, TSLA) : ");
string symbol = Console.ReadLine()?.Trim().ToUpper() ?? "AAPL";

// Validation du symbole améliorée
if (string.IsNullOrWhiteSpace(symbol) || symbol.Length > 5)
{
    Console.WriteLine("❌ Symbole invalide. Utilisez 1-5 caractères (ex: AAPL, MSFT).");
    return;
}

Console.WriteLine($"⏳ Récupération des données pour {symbol}...");

try
{
    var prices = await api.GetDailyPricesAsync(symbol);

    if (prices == null || prices.Count == 0)
    {
        Console.WriteLine("❌ Aucune donnée disponible. Vérifiez :");
        Console.WriteLine("   - La validité du symbole boursier");
        Console.WriteLine("   - Votre connexion internet");
        Console.WriteLine("   - Les limites d'utilisation de l'API");
        return;
    }

    // ✅ CALCULS FINANCIERS CORRIGÉS
    var returns = PortfolioAnalyzer.ComputeReturns(prices);
    
    if (returns == null || returns.Count == 0)
    {
        Console.WriteLine("❌ Impossible de calculer les rendements (données insuffisantes).");
        return;
    }

    var (dailyVol, annualVol) = PortfolioAnalyzer.ComputeVolatility(returns);
    var var95 = PortfolioAnalyzer.ComputeVaR(returns);
    
    // ✅ CALCULS MANUELS (car les méthodes n'existent pas dans PortfolioAnalyzer)
    decimal medianReturn = returns.Median(); // Utilise l'extension
    decimal totalReturn = (prices.Last().Close - prices.First().Close) / prices.First().Close;
    decimal sharpeRatio = returns.Average() / dailyVol; // Calcul simplifié

    // ✅ AFFICHAGE MIS À JOUR
    Console.WriteLine($"\n📈 Analyse de {symbol}");
    Console.WriteLine($"📅 Période d'analyse : {prices.Count} jours de trading");
    Console.WriteLine($"📊 Dernière cotation : {prices.Last().Close:C} ({prices.Last().Date:dd/MM/yyyy})");
    Console.WriteLine($"📈 Première cotation : {prices.First().Close:C} ({prices.First().Date:dd/MM/yyyy})");
    Console.WriteLine($"🚀 Performance totale : {totalReturn:P2}");
    Console.WriteLine($"💰 Rendement moyen quotidien : {returns.Average():P4}");
    Console.WriteLine($"📊 Rendement médian quotidien : {medianReturn:P4}");
    Console.WriteLine($"⚡ Volatilité quotidienne : {dailyVol:P4}");
    Console.WriteLine($"📈 Volatilité annualisée : {annualVol:P2}");
    Console.WriteLine($"📉 Value at Risk (95%) quotidienne : {var95:P2}");
    Console.WriteLine($"⭐ Ratio de Sharpe (quotidien) : {sharpeRatio:F3}");

    // Informations supplémentaires simplifiées
    Console.WriteLine($"\n📋 Statistiques des rendements :");
    Console.WriteLine($"   • Rendement maximum : {returns.Max():P2}");
    Console.WriteLine($"   • Rendement minimum : {returns.Min():P2}");
    Console.WriteLine($"   • Écart-type des rendements : {dailyVol:P4}");

    // Génération du graphique
    ChartService.PlotPrices(prices, symbol);

    Console.WriteLine("\n✅ Analyse terminée !");
    Console.WriteLine($"💾 Données sauvegardées dans le dossier 'Data/'");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Une erreur s'est produite : {ex.Message}");
    Console.WriteLine($"🔍 Détails : {ex.GetType().Name}");
}