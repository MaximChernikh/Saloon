using Saloon.Models;

namespace Saloon
{
    public partial class FriendsPage : ContentPage
    {
        public FriendsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            friendsList.ItemsSource = await App.DatabaseInstance.GetFriendsAsync();
        }

        private async Task LoadFriends()
        {
            friendsList.ItemsSource = await App.DatabaseInstance.GetFriendsAsync();
        }

        private async void OnAddFriendClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(nameEntry.Text))
            {
                await App.DatabaseInstance.SaveFriendAsync(new Friend
                {
                    Name = nameEntry.Text
                });

                nameEntry.Text = string.Empty;
                friendsList.ItemsSource = await App.DatabaseInstance.GetFriendsAsync();
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var friend = button?.CommandParameter as Friend;
            if (friend != null)
            {
                bool answer = await DisplayAlert("Подтверждение", $"Вы уверены, что хотите удалить расчет '{friend.Name}'?", "Да", "Нет");
                if (answer)
                {
                    await App.DatabaseInstance.DeleteFriendAsync(friend);
                    await LoadFriends();
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