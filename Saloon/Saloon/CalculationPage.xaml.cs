using Saloon.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Saloon
{
    public partial class CalculationPage : ContentPage
    {
        private ObservableCollection<Calculation> _calculations;

        public CalculationPage()
        {
            InitializeComponent();
            _calculations = new ObservableCollection<Calculation>();
            calculationListView.ItemsSource = _calculations;
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

            // Здесь должна быть логика выбора друзей
            List<Friend> selectedFriends = new List<Friend>(); // Заглушка

            var calculation = new Calculation(calculationName, DateTime.Now, selectedFriends, totalAmount);
            calculation.CalculateDebt();

            _calculations.Add(calculation);

            calculationNameEntry.Text = string.Empty;
            totalAmountEntry.Text = string.Empty;
        }
    }
}