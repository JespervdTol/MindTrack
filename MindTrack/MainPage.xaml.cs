using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MindTrack
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void onLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ScorePage");
        }
    }
}
