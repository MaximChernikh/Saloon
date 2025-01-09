using Saloon.Models;

namespace Saloon
{
    public partial class EstablishmentsPage : ContentPage
    {
        public EstablishmentsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadEstablishments();
        }

        private async Task LoadEstablishments()
        {
            establishmentsListView.ItemsSource = await App.DatabaseInstance.GetEstablishmentsAsync();
        }

        private async void OnAddEstablishmentClicked(object sender, EventArgs e)
        {
            string establishmentName = establishmentNameEntry.Text;
            string location = establishmentLocationEntry.Text;

            if (string.IsNullOrWhiteSpace(establishmentName) || string.IsNullOrWhiteSpace(location))
            {
                await DisplayAlert("Ошибка", "Введите название и местоположение заведения", "OK");
                return;
            }

            var establishment = new Establishment(establishmentName, location);
            await App.DatabaseInstance.SaveEstablishmentAsync(establishment);

            establishmentNameEntry.Text = string.Empty;
            establishmentLocationEntry.Text = string.Empty;

            await LoadEstablishments();
        }


        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var establishment = button?.CommandParameter as Establishment;
            if (establishment != null)
            {
                bool answer = await DisplayAlert("Подтверждение", $"Вы уверены, что хотите удалить расчет '{establishment.Name}'?", "Да", "Нет");
                if (answer)
                {
                    await App.DatabaseInstance.DeleteEstablishmentAsync(establishment);
                    await LoadEstablishments();
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