using MyClass.Tables;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyClass.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private async void LoginNavigationAction(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private void RegisterAction(object sender, EventArgs e)
        {
           if(verifyEntredData())
            {
                var db = DBConnection.connect();
                db.CreateTable<ProfessorTable>();

                var existedProf = db.Table<ProfessorTable>().Where(u => u.UserName.Equals(UsernameEntry.Text) 
                    && u.Password.Equals(PasswordEntry.Text)).FirstOrDefault();

                if(existedProf != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await this.DisplayAlert("Error", "Username already taken!", "Ok", "Cancel");
                        if (result)
                            await Navigation.PushAsync(new RegistrationPage());
                    });
                } else
                {
                    var prof = new ProfessorTable()
                    {
                        UserName = UsernameEntry.Text,
                        Password = PasswordEntry.Text
                    };

                    db.Insert(prof);
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await this.DisplayAlert("Congratulations", "Professor Registration Successfull", "Ok", "Cancel");
                        if (result)
                            await Navigation.PushAsync(new LoginPage());
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
            } else if(PasswordEntry.Text == null || "".Equals(PasswordEntry.Text))
            {
                errorMsg = "Password is required!";
                error = true;
            } else if(PasswordConfirmationEntry.Text == null || "".Equals(PasswordConfirmationEntry.Text)
                || !PasswordEntry.Text.Equals(PasswordConfirmationEntry.Text))
            {
                errorMsg = "Confirm your password please!";
                error = true;
            }

            if(error)
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