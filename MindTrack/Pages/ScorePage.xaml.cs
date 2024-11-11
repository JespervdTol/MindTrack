using MindTrack.Module;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MindTrack.Pages
{
    public partial class ScorePage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<Person> PersonItems { get; set; }
        public ObservableCollection<string> GameList { get; set; }

        private bool _isScoreboardVisible;
        public bool IsScoreboardVisible
        {
            get => _isScoreboardVisible;
            set
            {
                _isScoreboardVisible = value;
                OnPropertyChanged(nameof(IsScoreboardVisible));
            }
        }

        private bool _isPersonTableVisible;
        public bool IsPersonTableVisible
        {
            get => _isPersonTableVisible;
            set
            {
                _isPersonTableVisible = value;
                OnPropertyChanged(nameof(IsPersonTableVisible));
            }
        }

        public ScorePage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            PersonItems = new ObservableCollection<Person>();
            GameList = new ObservableCollection<string>();
            BindingContext = this;

            IsScoreboardVisible = true;
            IsPersonTableVisible = false;

            LoadGames();
        }

        private async void LoadGames()
        {
            var games = await _databaseService.GetGamesAsync();

            GameList.Clear();

            foreach (var game in games)
            {
                GameList.Add(game);
            }

            if (GameList.Count > 0)
            {
                await LoadDataByGame(GameList[0]);
            }
        }

        private async Task LoadDataByGame(string game)
        {
            try
            {
                var personData = await _databaseService.GetPersonDataByGameAsync(game);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PersonItems.Clear();
                    foreach (var item in personData)
                    {
                        PersonItems.Add(item);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data for game '{game}': {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to load data. Please try again.", "OK");
            }
        }

        private async void LoadPersonTableData()
        {
            try
            {
                var personData = await _databaseService.GetAllPersonsAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PersonItems.Clear();
                    foreach (var item in personData)
                    {
                        PersonItems.Add(item);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading person data: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to load person data. Please try again.", "OK");
            }
        }

        private async void OnGameSelected(object sender, EventArgs e)
        {
            if (sender is Picker picker && picker.SelectedItem is string selectedGame && !string.IsNullOrEmpty(selectedGame))
            {
                await LoadDataByGame(selectedGame);
            }
        }

        private void ShowScoreboardTable(object sender, EventArgs e)
        {
            IsScoreboardVisible = true;
            IsPersonTableVisible = false;

            LoadDataByGame(GameList[0]);
        }

        private void ShowPersonTable(object sender, EventArgs e)
        {
            IsScoreboardVisible = false;
            IsPersonTableVisible = true;

            LoadPersonTableData();
        }

        private async void OnAddPersonClicked(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Add Person", "Enter name:");
            if (string.IsNullOrEmpty(name)) return;

            string email = await DisplayPromptAsync("Add Person", "Enter email:");
            if (string.IsNullOrEmpty(email)) return;

            string birthdayStr = await DisplayPromptAsync("Add Person", "Enter birthday (dd/MM/yyyy):");
            if (string.IsNullOrEmpty(birthdayStr)) return;

            DateTime birthday;
            if (!DateTime.TryParseExact(birthdayStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out birthday))
            {
                await DisplayAlert("Error", "Invalid date format. Please use dd/MM/yyyy.", "OK");
                return;
            }

            bool success = await _databaseService.AddPersonService(name, email, birthday);
            if (success)
            {
                await DisplayAlert("Success", "Person added successfully", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to add person. Try again later.", "OK");
            }
        }
    }
}