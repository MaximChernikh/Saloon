using Saloon.Models;
using System;
using System.Collections.ObjectModel;

namespace Saloon
{
    public partial class FriendsPage : ContentPage
    {
        private ObservableCollection<Friend> _friends;

        public FriendsPage()
        {
            InitializeComponent();
            _friends = new ObservableCollection<Friend>();
            friendsListView.ItemsSource = _friends;
        }

        private void OnAddFriendClicked(object sender, EventArgs e)
        {
            string friendName = friendNameEntry.Text;
            if (string.IsNullOrWhiteSpace(friendName))
            {
                DisplayAlert("Ошибка", "Введите имя друга", "OK");
                return;
            }

            var friend = new Friend(friendName);
            _friends.Add(friend);

            friendNameEntry.Text = string.Empty;
        }
    }
}