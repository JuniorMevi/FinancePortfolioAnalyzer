# 📊 FinancePortfolioAnalyzer

Une application console .NET pour l'analyse quantitative de titres boursiers, développée dans le cadre d'un projet personnel démontrant des compétences en finance quantitative et en développement C#.

## 🚀 Fonctionnalités

### 📈 Analyse Quantitative
- **Calcul des rendements** : Rendements quotidiens et performance totale
- **Mesure du risque** : Volatilité quotidienne et annualisée
- **Value at Risk (VaR)** : Calcul du risque de perte à 95% de confiance
- **Ratio de Sharpe** : Mesure du rendement ajusté au risque
- **Statistiques avancées** : Rendement médian, min/max, écart-type

### 📊 Visualisation
- Génération automatique de graphiques des prix avec **ScottPlot**
- Configuration avancée des axes et légendes
- Export en format PNG pour reporting

### 💾 Gestion des Données
- Intégration avec l'API **Alpha Vantage** pour les données temps-réel
- Système de cache local pour éviter les limites d'API
- Support des principales bourses (NYSE, NASDAQ, Euronext)

## 🛠️ Architecture Technique

### Technologies Utilisées
- **.NET 8.0** - Runtime et framework
- **ScottPlot** - Bibliothèque de visualisation
- **Alpha Vantage API** - Source de données financières
- **System.Text.Json** - Sérialisation des données

## 📋 Métriques Calculées

| Métrique | Formule | Description |
|----------|---------|-------------|
| **Rendement Quotidien** | `(Pₜ - Pₜ₋₁) / Pₜ₋₁` | Performance journalière |
| **Volatilité** | `σ = √(Σ(r - μ)² / n)` | Mesure du risque historique |
| **VaR 95%** | `Percentile 5% des rendements` | Perte maximale attendue |
| **Ratio de Sharpe** | `(μ - rᶠ) / σ` | Rendement ajusté au risque |

## 🎯 Impact et Cas d'Usage

### Pour les Investisseurs
- **Analyse rapide** du profil risque/rendement d'un titre
- **Visualisation intuitive** de l'évolution des prix
- **Comparaison objective** entre différentes opportunités

### Pour les Développeurs
- **Exemple concret** d'intégration d'API financière
- **Architecture modulaire** et extensible
- **Code production-ready** avec gestion d'erreurs

## 🚀 Installation et Utilisation

### Prérequis
- .NET 8.0 SDK
- Clé API Alpha Vantage (gratuite)

### Configuration
```bash
# Cloner le repository
git clone https://github.com/JuniorMevi/FinancePortfolioAnalyzer.git
cd FinancePortfolioAnalyzer

# Configuration de la clé API
export ALPHA_VANTAGE_API_KEY="C69Z7VV1L21BQQOZ"
```

## Utilisation
```bash
dotnet run

# Suivre les prompts :
# → Entrer le symbole (ex: AAPL, MSFT, SAN.PA)
# → Visualiser l'analyse générée
# → Consulter le graphique dans Data/price_plot.png
```
## 📊 Exemple de Sortie
- 📈 Analyse de AAPL
- 📅 Période d'analyse : 100 jours de trading
- 📊 Dernière cotation : $185.50 (15/12/2024)
- 🚀 Performance totale : +22.45%
- 💰 Rendement moyen quotidien : 0.1542%
- ⚡ Volatilité quotidienne : 1.2435%
- 📈 Volatilité annualisée : 19.72%
- 📉 Value at Risk (95%) : -2.0150%
- ⭐ Ratio de Sharpe : 0.124

## Structure du Projet
FinancePortfolioAnalyzer/
- ├── Services/
- │   ├── AlphaVantageService.cs    # Intégration API
- │   ├── PortfolioAnalyzer.cs      # Calculs financiers
- │   └── ChartService.cs           # Génération graphiques
- ├── Models/
- │   └── StockPrice.cs            # Modèle de données
- ├── Data/                        # Cache et exports
- └── Program.cs                   # Point d'entrée

## 🔧 Extensibilité
Le projet est conçu pour être facilement étendu :

**Nouvelles métriques** : Ajout dans PortfolioAnalyzer

**Sources de données** : Implémentation d'interfaces supplémentaires

**Visualisations** : Extension de ChartService

**Export de rapports** : Génération PDF/Excel

## 📈 Résultats et Validation
### Tests sur Données Réelles
- **Actions US :** AAPL, MSFT, TSLA - Données cohérentes avec Yahoo Finance

- **Actions Européennes :** SAN.PA, AIR.PA - Support des marchés internationaux

- **Performance :** Calculs validés contre des références industry

## Points Forts
- **✅ Calculs financiers académiquement corrects**
- **✅ Gestion robuste des erreurs API**
- **✅ Interface utilisateur claire et informative**
- **✅ Code maintenable et documenté**

## 🤝 Contribution
Les contributions sont les bienvenues ! Zones d'amélioration possibles :

1. **Nouvelles métriques de risque** (Beta, Sharpe, Sortino)

2. **Analyse de portefeuille** (corrélation, diversification)

3. **Support multi-devises**

4. **Interface web** (Blazor/ASP.NET Core)


**Développé avec ❤️ en C#** - Un projet démontrant l'application du développement logiciel à la finance quantitative.

