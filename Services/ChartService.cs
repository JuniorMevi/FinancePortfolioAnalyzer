using FinancePortfolioAnalyzer.Models;
using ScottPlot;

namespace FinancePortfolioAnalyzer.Services;

public static class ChartService
{
    public static void PlotPrices(List<StockPrice> prices, string symbol)
    {
        if (prices == null || prices.Count == 0)
        {
            Console.WriteLine("❌ Impossible de générer le graphique : aucune donnée disponible.");
            return;
        }

        // Extraction des données
        double[] xs = prices.Select(p => p.Date.ToOADate()).ToArray();
        double[] ys = prices.Select(p => (double)p.Close).ToArray();

        // Création du graphique
        var plt = new ScottPlot.Plot();

        // Tracé des prix
        var scatter = plt.Add.Scatter(xs, ys);
        scatter.LegendText = $"{symbol} Prix de clôture";
        scatter.Color = Colors.Blue;

        // Mise en forme
        plt.Title($"Évolution du prix de {symbol}");
        
        // CORRECTION : Configuration simplifiée des dates
        plt.Axes.Bottom.TickLabelStyle.Rotation = 45;
        plt.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleRight;
        
        // Alternative : Utiliser le générateur de ticks par défaut pour les dates
        plt.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.DateTimeAutomatic();

        plt.YLabel("Prix de clôture ($)");
        plt.ShowLegend();

        // Sauvegarde
        string folder = "Data";
        Directory.CreateDirectory(folder);
        string filePath = Path.Combine(folder, "price_plot.png");

        plt.SavePng(filePath, 800, 400);
        Console.WriteLine($"📊 Graphique généré : {filePath}");
    }
}