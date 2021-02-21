using MyClass.Tables;
using SQLite;
using System;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyClass.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private async void RegisterNavigationAction(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage());
        }

        private void LoginAction(object sender, EventArgs e)
        {
            if (verifyEntredData())
            {
                var db = DBConnection.connect();
                db.CreateTable<ProfessorTable>();

                var query = db.Table<ProfessorTable>().Where(u => u.UserName.Equals(UsernameEntry.Text) 
                                                && u.Password.Equals(PasswordEntry.Text)).FirstOrDefault();

                if (query != null)
                {
                    App.Current.MainPage = new NavigationPage(new MainPage());
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await this.DisplayAlert("Error", "Username or Password incorrect", "Ok", "Cancel");

                        if (result)
                            await Navigation.PushAsync(new LoginPage());
                        else
                        {
                            await Navigation.PushAsync(new LoginPage());
                        }
                    });
                }
            }
        }

        private bool verifyEntredData()
        {
            string errorMsg = "";
            bool error = false;

            if (UsernameEntry.Text == null || "".Equals(UsernameEntry.Text))
            {
                errorMsg = "Username is required!";
                error = true;
            }
            else if (PasswordEntry.Text == null || "".Equals(PasswordEntry.Text))
            {
                errorMsg = "Password is required!";
                error = true;
            }

            if (error)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await this.DisplayAlert("Error", errorMsg, null, "Ok");
                });

                return false;
            }

            return true;
        }

        
    }
}