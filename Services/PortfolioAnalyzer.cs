using FinancePortfolioAnalyzer.Models;

namespace FinancePortfolioAnalyzer.Services;

public static class PortfolioAnalyzer
{
    public static List<decimal> ComputeReturns(List<StockPrice> prices)
    {
        var returns = new List<decimal>();
        for (int i = 1; i < prices.Count; i++)
        {
            decimal ret = (prices[i].Close - prices[i - 1].Close) / prices[i - 1].Close;
            returns.Add(ret);
        }
        return returns;
    }

    public static (decimal DailyVolatility, decimal AnnualVolatility) ComputeVolatility(List<decimal> returns)
    {
        var avg = returns.Average();
        var variance = returns.Average(r => (double)Math.Pow((double)(r - avg), 2));
        decimal dailyVolatility = (decimal)Math.Sqrt(variance);
        decimal annualVolatility = dailyVolatility * (decimal)Math.Sqrt(252);
        
        return (dailyVolatility, annualVolatility);
    }

    public static decimal ComputeVaR(List<decimal> returns, double confidenceLevel = 0.05)
    {
        var sorted = returns.OrderBy(r => r).ToList();
        int index = Math.Max((int)(confidenceLevel * sorted.Count), 0);
        return sorted[index];
    }
}

// Classe séparée au même niveau d'espace de noms
public static class EnumerableExtensions
{
    public static decimal Median(this IEnumerable<decimal> source)
    {
        var sorted = source.OrderBy(x => x).ToArray();
        int count = sorted.Length;
        
        if (count == 0) return 0;
        
        if (count % 2 == 0)
        {
            return (sorted[count / 2 - 1] + sorted[count / 2]) / 2;
        }
        else
        {
            return sorted[count / 2];
        }
    }
}