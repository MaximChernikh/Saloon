using Saloon.Models;

namespace Saloon
{
    public partial class CalculationPage : ContentPage
    {
        private List<Calculation> _calculations;

        public CalculationPage()
        {
            InitializeComponent();
            _calculations = new List<Calculation>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCalculations();
        }

        private async Task LoadCalculations()
        {
            var calculations = await App.DatabaseInstance.GetCalculationsAsync();
            calculationListView.ItemsSource = calculations;
        }


        private async void OnAddCalculationClicked(object sender, EventArgs e)
        {
            string calculationName = calculationNameEntry.Text;
            if (string.IsNullOrWhiteSpace(calculationName))
            {
                await DisplayAlert("������", "������� ��� �������", "OK");
                return;
            }

            var calculation = new Calculation(calculationName, partyingDate.Date);
            await App.DatabaseInstance.SaveCalculationAsync(calculation);

            calculationNameEntry.Text = string.Empty;
            partyingDate.Date = DateTime.Now;

            await LoadCalculations();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var calculation = button?.CommandParameter as Calculation;
            if (calculation != null)
            {
                bool answer = await DisplayAlert("�������������", $"�� �������, ��� ������ ������� ������ '{calculation.CalculationName}'?", "��", "���");
                if (answer)
                {
                    await App.DatabaseInstance.DeleteCalculationAsync(calculation);
                    await LoadCalculations();
                }
            }
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender is ListView listView)
            {
                listView.SelectedItem = null;
            }
        }
        private async void OnCalculateOrViewClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;

                if (button == null)
                    return;

                var calculation = button.CommandParameter as Calculation;

                if (calculation == null)
                    return;

                if (button.Text == "����������")
                {
                    var debtCalculationPage = new DebtCalculationPage(calculation);
                    await Navigation.PushAsync(debtCalculationPage);
                }
                else if (button.Text == "����������")
                {
                    await Navigation.PushAsync(new CalculationResultPage(calculation));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("������", $"��������� ������: {ex.Message}", "OK");
            }
        }

    }

    public class BoolToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string stringParameter)
            {
                var options = stringParameter.Split(',');
                return boolValue ? options[1] : options[0];
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}