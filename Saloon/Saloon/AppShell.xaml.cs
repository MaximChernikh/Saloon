using Microsoft.Maui.Controls;

namespace Saloon
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CalculationPage), typeof(CalculationPage));
            Routing.RegisterRoute(nameof(DebtCalculationPage), typeof(DebtCalculationPage));
            Routing.RegisterRoute(nameof(CalculationResultPage), typeof(CalculationResultPage));
        }
    }
}