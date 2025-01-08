using Saloon.Models;
using System;
using System.Collections.ObjectModel;

namespace Saloon
{
    public partial class EstablishmentsPage : ContentPage
    {
        private ObservableCollection<Establishment> _establishments;

        public EstablishmentsPage()
        {
            InitializeComponent();
            _establishments = new ObservableCollection<Establishment>();
            establishmentsListView.ItemsSource = _establishments;
        }

        private void OnAddEstablishmentClicked(object sender, EventArgs e)
        {
            string establishmentName = establishmentNameEntry.Text;
            string location = establishmentLocationEntry.Text;

            if (string.IsNullOrWhiteSpace(establishmentName) || string.IsNullOrWhiteSpace(location))
            {
                DisplayAlert("Ошибка", "Введите название и местоположение заведения", "OK");
                return;
            }

            var establishment = new Establishment(establishmentName, location);
            _establishments.Add(establishment);

            establishmentNameEntry.Text = string.Empty;
            establishmentLocationEntry.Text = string.Empty;
        }
    }
}