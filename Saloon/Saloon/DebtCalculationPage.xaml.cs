using Saloon.Models;
using System.Collections.ObjectModel;

namespace Saloon
{
    public partial class DebtCalculationPage : ContentPage
    {
        private Calculation _calculation;
        private ObservableCollection<EstablishmentExpense> _establishmentExpenses;

        public DebtCalculationPage(Calculation calculation)
        {
            InitializeComponent();
            _calculation = calculation;
            _establishmentExpenses = new ObservableCollection<EstablishmentExpense>(_calculation.EstablishmentExpenses);
            establishmentsListView.ItemsSource = _establishmentExpenses;
        }

        private async void OnAddEstablishmentClicked(object sender, EventArgs e)
        {
            string establishmentName = await DisplayPromptAsync("Новое заведение", "Введите название заведения");
            if (!string.IsNullOrWhiteSpace(establishmentName))
            {
                var newEstablishment = new Establishment { Name = establishmentName };
                var newEstablishmentExpense = new EstablishmentExpense { Establishment = newEstablishment };
                _establishmentExpenses.Add(newEstablishmentExpense);
            }
        }

        private async void OnAddFriendToEstablishmentClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var establishmentExpense = button?.CommandParameter as EstablishmentExpense;

            if (establishmentExpense == null)
                return;

            var allFriends = await App.DatabaseInstance.GetFriendsAsync();
            var friendsNotInEstablishment = allFriends.Where(f => !establishmentExpense.FriendExpenses.Any(fe => fe.Friend.Id == f.Id)).ToList();

            if (friendsNotInEstablishment.Count == 0)
            {
                await DisplayAlert("Информация", "Все друзья уже добавлены в это заведение", "OK");
                return;
            }

            var friend = await DisplayActionSheet("Выберите друга", "Отмена", null, friendsNotInEstablishment.Select(f => f.Name).ToArray());

            if (friend != "Отмена" && friend != null)
            {
                var selectedFriend = friendsNotInEstablishment.First(f => f.Name == friend);
                var newFriendExpense = new FriendExpense { Friend = selectedFriend, Amount = 0 };
                establishmentExpense.FriendExpenses.Add(newFriendExpense);

                if (establishmentExpense.Payer == null)
                {
                    establishmentExpense.Payer = newFriendExpense;
                }

                RefreshListView();
            }
        }

        private void OnEstablishmentSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (establishmentsListView.SelectedItem != null)
                establishmentsListView.SelectedItem = null;
        }
        private void OnPayerSelected(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var establishmentExpense = picker?.BindingContext as EstablishmentExpense;
            if (establishmentExpense != null && picker.SelectedItem is FriendExpense selectedFriendExpense)
            {
                establishmentExpense.Payer = selectedFriendExpense;
            }
        }

        private async void OnCalculateClicked(object sender, EventArgs e)
        {
            var invalidEstablishments = _establishmentExpenses.Where(ee =>
                ee.Payer == null ||
                ee.FriendExpenses == null ||
                !ee.FriendExpenses.Any() ||
                ee.FriendExpenses.All(fe => fe.Amount == 0)
            ).ToList();

            if (invalidEstablishments.Any())
            {
                string errorMessage = "Следующие заведения заполнены некорректно:\n";
                foreach (var ee in invalidEstablishments)
                {
                    errorMessage += $"- {ee.Establishment.Name}: ";
                    if (ee.Payer == null)
                        errorMessage += "не выбран плательщик; ";
                    if (ee.FriendExpenses == null || !ee.FriendExpenses.Any())
                        errorMessage += "не добавлены друзья; ";
                    if (ee.FriendExpenses != null && ee.FriendExpenses.All(fe => fe.Amount == 0))
                        errorMessage += "все суммы равны нулю; ";
                    errorMessage += "\n";
                }

                await DisplayAlert("Ошибка", errorMessage, "OK");
                return;
            }

            _calculation.EstablishmentExpenses = _establishmentExpenses.ToList();
            _calculation.CalculateDebt();

            await App.DatabaseInstance.SaveCalculationAsync(_calculation);

            await Navigation.PushAsync(new CalculationResultPage(_calculation));
        }
        private void RefreshListView()
        {
            establishmentsListView.ItemsSource = null;
            establishmentsListView.ItemsSource = _establishmentExpenses;
        }
    }
}