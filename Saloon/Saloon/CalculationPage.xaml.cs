using Saloon.Models;

namespace Saloon
{
    public partial class CalculationPage : ContentPage
    {
        public CalculationPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCalculations();
        }

        private async Task LoadCalculations()
        {
            calculationListView.ItemsSource = await App.DatabaseInstance.GetCalculationsAsync();
        }

        private async void OnAddCalculationClicked(object sender, EventArgs e)
        {
            string calculationName = calculationNameEntry.Text;
            if (string.IsNullOrWhiteSpace(calculationName))
            {
                await DisplayAlert("Ошибка", "Введите имя расчета", "OK");
                return;
            }

            if (!decimal.TryParse(totalAmountEntry.Text, out decimal totalAmount))
            {
                await DisplayAlert("Ошибка", "Введите корректную сумму", "OK");
                return;
            }

            var calculation = new Calculation(calculationName, DateTime.Now, totalAmount);
            await App.DatabaseInstance.SaveCalculationAsync(calculation);

            calculationNameEntry.Text = string.Empty;
            totalAmountEntry.Text = string.Empty;

            await LoadCalculations();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var calculation = button?.CommandParameter as Calculation;
            if (calculation != null)
            {
                bool answer = await DisplayAlert("Подтверждение", $"Вы уверены, что хотите удалить расчет '{calculation.CalculationName}'?", "Да", "Нет");
                if (answer)
                {
                    await App.DatabaseInstance.DeleteCalculationAsync(calculation);
                    await LoadCalculations();
                }
            }
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Сбрасываем выделение
            if (sender is ListView listView)
            {
                listView.SelectedItem = null;
            }
        }
    }
}