using MindTrack.Module;
using System;

namespace MindTrack.Pages
{
    public partial class AddPersonPage : ContentPage
    {
        //private readonly DatabaseService _databaseService;

        public AddPersonPage()
        {
            InitializeComponent();
            //_databaseService = new DatabaseService();
        }

        //private async void OnSubmitClicked(object sender, EventArgs e)
        //{
        //    string name = NameEntry.Text;
        //    string email = EmailEntry.Text;
        //    string birthday = BirthdayEntry.Text;

        //    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(birthday))
        //    {
        //        await DisplayAlert("Error", "All fields are required", "OK");
        //        return;
        //    }

        //    DateTime parsedBirthday;
        //    if (!DateTime.TryParseExact(birthday, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out parsedBirthday))
        //    {
        //        await DisplayAlert("Error", "Invalid date format. Please use dd/MM/yyyy.", "OK");
        //        return;
        //    }

        //    bool success = await _databaseService.AddPersonService(name, email, parsedBirthday);
        //    if (success)
        //    {
        //        await DisplayAlert("Success", "Person added successfully", "OK");
        //        await Navigation.PopModalAsync();
        //    }
        //    else
        //    {
        //        await DisplayAlert("Error", "Failed to add person. Try again later.", "OK");
        //    }
        //}

        //private async void OnCancelClicked(object sender, EventArgs e)
        //{
        //    await Navigation.PopModalAsync(); 
        //}
    }
}