using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TravelPlanner;

namespace travelplaner
{
    public partial class MainWindow : Window 
    {
        private List<string> cities = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnCountryChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (CountryCombo.SelectedItem is ComboBoxItem item)
            {
                var country = item.Content?.ToString() ?? "";
                try
                {
                    // Sprawdzamy różne możliwe ścieżki do zdjęć
                    var possiblePaths = new[]
                    {
                        $"Assets/Images/{country}.jpg",
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Images", $"{country}.jpg"),
                        Path.Combine("Assets", "Images", $"{country}.jpg")
                    };

                    var existingPath = possiblePaths.FirstOrDefault(File.Exists);
            
                    if (existingPath != null)
                    {
                        using var stream = File.OpenRead(existingPath);
                        CountryImage.Source = new Bitmap(stream);
                    }
                }
                catch (Exception ex)
                {
                    // Można dodać jakieś logowanie błędu
                    Console.WriteLine($"Błąd wczytywania obrazu: {ex.Message}");
                }
            }
        }




        private void OnAddCity(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var text = CityBox.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(text))
            {
                cities.Add(text);
                CityBox.Text = "";
                CityList.ItemsSource = null;
                CityList.ItemsSource = cities.ToList();
            }
        }

        private async void OnShowDetails(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var name = NameBox.Text?.Trim() ?? "";
            var country = (CountryCombo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
            var attractions = new List<string>();
            if (MuseumCheck.IsChecked == true) attractions.Add("Muzea");
            if (ParksCheck.IsChecked == true) attractions.Add("Parki Narodowe");
            if (MonumentsCheck.IsChecked == true) attractions.Add("Zabytki");
            if (RestaurantsCheck.IsChecked == true) attractions.Add("Restauracje");
            if (GalleriesCheck.IsChecked == true) attractions.Add("Galerie sztuki");
            if (FestivalsCheck.IsChecked == true) attractions.Add("Festiwale i koncerty");

            var transport = PlaneRadio.IsChecked == true ? "Samolot" :
                            CarRadio.IsChecked == true ? "Samochód" :
                            TrainRadio.IsChecked == true ? "Pociąg" :
                            ShipRadio.IsChecked == true ? "Statek" : "Nie wybrano";

            var date = TravelCalendar.SelectedDate?.ToString("d") ?? "Brak";
            var summary = $"Podróżujący: {name}\nKraj: {country}\nData: {date}\nTransport: {transport}\n" +
                          $"Atrakcje: {string.Join(", ", attractions)}\nMiasta: {string.Join(", ", cities)}";

            File.WriteAllText("TravelData.txt", summary);

            var dialog = new SummaryWindow(summary);
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            await dialog.ShowDialog(this);
        }
    }
}