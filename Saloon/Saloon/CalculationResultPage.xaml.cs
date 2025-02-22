using Saloon.Models;
using System.Collections.Generic;
using System.Linq;

namespace Saloon
{
    public partial class CalculationResultPage : ContentPage
    {
        private Calculation _calculation;

        public CalculationResultPage(Calculation calculation)
        {
            InitializeComponent();
            _calculation = calculation;
            CalculateAndDisplayResults(calculation);
        }

        private void CalculateAndDisplayResults(Calculation calculation)
        {
            var payerDebtors = new List<PayerDebtors>();

            foreach (var establishmentExpense in calculation.EstablishmentExpenses)
            {
                foreach (var friendExpense in establishmentExpense.FriendExpenses)
                {
                    if (friendExpense.Friend.Id != establishmentExpense.Payer.Friend.Id)
                    {
                        var debtAmount = friendExpense.Amount;

                        var payerDebtor = payerDebtors.FirstOrDefault(pd => pd.PayerId == establishmentExpense.Payer.Friend.Id);
                        if (payerDebtor == null)
                        {
                            payerDebtor = new PayerDebtors
                            {
                                PayerId = establishmentExpense.Payer.Friend.Id,
                                PayerName = establishmentExpense.Payer.Friend.Name,
                                Debtors = new List<FriendExpense>()
                            };
                            payerDebtors.Add(payerDebtor);
                        }

                        var debtor = payerDebtor.Debtors.FirstOrDefault(d => d.Friend.Id == friendExpense.Friend.Id);
                        if (debtor == null)
                        {
                            debtor = new FriendExpense { Friend = friendExpense.Friend, Amount = 0 };
                            payerDebtor.Debtors.Add(debtor);
                        }
                        debtor.Amount += debtAmount;
                    }
                }
            }

            payersListView.ItemsSource = payerDebtors;
        }

        private async void OnBackToCalculationClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new CalculationPage());
                await Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnBackToCalculationClicked: {ex.Message}");
                await DisplayAlert("Ошибка", "Не удалось вернуться к странице расчета. Пожалуйста, попробуйте еще раз.", "OK");
            }
        }

        public void UpdateResults(Calculation calculation)
        {
            _calculation = calculation;
            CalculateAndDisplayResults(calculation);
        }
    }

    public class PayerDebtors
    {
        public int PayerId { get; set; }
        public string PayerName { get; set; } = string.Empty;
        public List<FriendExpense> Debtors { get; set; } = new List<FriendExpense>();
    }
}