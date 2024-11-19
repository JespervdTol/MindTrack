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

        private const string LastSelectedGameKey = "LastSelectedGame";

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

            var lastSelectedGame = Preferences.Get(LastSelectedGameKey, string.Empty);

            if (!string.IsNullOrEmpty(lastSelectedGame) && GameList.Contains(lastSelectedGame))
            {
                gamePicker.SelectedItem = lastSelectedGame;

                await LoadDataByGame(lastSelectedGame);
            }
            else if (GameList.Count > 0)
            {
                await LoadDataByGame(GameList[0]);
            }
        }

        // Sorted for startevent
        private async Task LoadDataByGame(string game)
        {
            try
            {
                var personData = await _databaseService.GetPersonDataByGameAsync(game);

                if (game.Equals("Reaction Test", StringComparison.OrdinalIgnoreCase))
                {
                    personData = personData.OrderBy(p => p.Score).ToList();
                    foreach (var person in personData)
                    {
                        person.ScoreFormatted = person.Score.HasValue ? $"{person.Score}ms" : "N/A";
                    }
                }
                else if (game.Equals("Simon", StringComparison.OrdinalIgnoreCase))
                {
                    personData = personData.OrderByDescending(p => p.Score).ToList();
                    foreach (var person in personData)
                    {
                        person.ScoreFormatted = person.Score.HasValue ? $"Level {person.Score}" : "N/A";
                    }
                }

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

        private async Task LoadPersonTableData()
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
                Preferences.Set(LastSelectedGameKey, selectedGame);

                await LoadDataByGame(selectedGame);
            }
        }

        private async void ShowScoreboardTable(object sender, EventArgs e)
        {
            IsScoreboardVisible = true;
            IsPersonTableVisible = false;

            if (gamePicker.SelectedItem is string selectedGame && !string.IsNullOrEmpty(selectedGame))
            {
                await LoadDataByGame(selectedGame);
            }
        }


        private void ShowPersonTable(object sender, EventArgs e)
        {
            IsScoreboardVisible = false;
            IsPersonTableVisible = true;

            LoadPersonTableData();
        }

        private async void OnAddPersonClicked(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Gebruiker Toevoegen", "Vul naam in:");
            if (string.IsNullOrEmpty(name)) return;


            //string email = await DisplayPromptAsync("Add Person", "Enter email:");
            //if (string.IsNullOrEmpty(email)) return;

            //string birthdayStr = await DisplayPromptAsync("Add Person", "Enter birthday (dd/MM/yyyy):");
            //if (string.IsNullOrEmpty(birthdayStr)) return;

            //DateTime birthday;
            //if (!DateTime.TryParseExact(birthdayStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out birthday))
            //{
            //    await DisplayAlert("Error", "Invalid date format. Please use dd/MM/yyyy.", "OK");
            //    return;
            //}

            //int accountId = 0;

            //email, birthday temp removed: AddPersonService(name, email, birthday)
            bool success = await _databaseService.AddPersonService(name);

            if (success)
            {
                await DisplayAlert("Success", "Person added successfully", "OK");
                //await LoadPersonTableData();
            }
            else
            {
                await DisplayAlert("Error", "Failed to add person. Try again later.", "OK");
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool rememberMe = Preferences.Get("RememberMe", false);

            if (!rememberMe)
            {
                Preferences.Remove("username");
                Preferences.Remove("password");
            }

            Preferences.Remove("SomeSessionKey");


            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}