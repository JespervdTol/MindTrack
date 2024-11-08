using MindTrack.Module;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MindTrack.Pages
{
    public partial class ScorePage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<Person> PersonItems { get; set; }
        public ObservableCollection<string> GameList { get; set; }

        public ScorePage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            PersonItems = new ObservableCollection<Person>();
            GameList = new ObservableCollection<string>();
            BindingContext = this;

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

        private async void OnGameSelected(object sender, EventArgs e)
        {
            if (sender is Picker picker && picker.SelectedItem is string selectedGame && !string.IsNullOrEmpty(selectedGame))
            {
                await LoadDataByGame(selectedGame);
            }
        }
    }
}