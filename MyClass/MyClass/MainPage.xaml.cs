using System;
using Xamarin.Forms;

namespace MyClass
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void AbsenceNavigationAction(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.AbsencePage());
        }
        private async void AddStudentNavigationAction(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.AddStudentPage());
        }
        private async void NewLessonNavigationAction(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.NewLessonPage());
        }
        private async void SearchNavigationAction(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.SearchStudent());
        }

        private async void LogoutNavigationAction(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.LoginPage());
        }
    }
}
