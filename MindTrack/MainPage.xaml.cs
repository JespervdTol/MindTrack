using MindTrack.Module;
using System;
using Microsoft.Maui.Storage;

namespace MindTrack
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public MainPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();

            LoadCredentials();
        }

        private void LoadCredentials()
        {
            bool rememberMe = Preferences.Get("RememberMe", false);
            RememberMeCheckBox.IsChecked = rememberMe;

            if (rememberMe)
            {
                UsernameEntry.Text = Preferences.Get("username", string.Empty);
                PasswordEntry.Text = Preferences.Get("password", string.Empty);
            }
            else
            {
                UsernameEntry.Text = string.Empty;
                PasswordEntry.Text = string.Empty;
            }
        }

        private async void onLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please enter both username and password", "OK");
                return;
            }

            Account account = await _databaseService.ValidateLoginAsync(username, password);

            if (account != null)
            {
                if (RememberMeCheckBox.IsChecked)
                {
                    Preferences.Set("RememberMe", true);
                    Preferences.Set("username", username);
                    Preferences.Set("password", password);
                }
                else
                {
                    Preferences.Set("RememberMe", false);
                    Preferences.Remove("username");
                    Preferences.Remove("password");
                }

                PasswordEntry.Text = string.Empty;
                await Shell.Current.GoToAsync("//ScorePage");
            }
            else
            {
                await DisplayAlert("Error", "Invalid username or password", "OK");
                PasswordEntry.Text = string.Empty;
            }
        }
    }
}