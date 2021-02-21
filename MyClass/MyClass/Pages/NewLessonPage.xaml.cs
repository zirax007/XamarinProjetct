using MyClass.Tables;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyClass.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewLessonPage : ContentPage
    {
        public NewLessonPage()
        {
            InitializeComponent();
        }
        private async void CancelAction(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
        private void AddNewLessonAction(object sender, EventArgs e)
        {
            if (verifyEntredData())
            {
                var db = DBConnection.connect();
                db.CreateTable<LessonTable>();

                var lesson = new LessonTable()
                {
                    Name = LessonNameEntry.Text,
                    Faculty = LessonFacultyEntry.Text
                };

                db.Insert(lesson);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await this.DisplayAlert("Congratulations", "New Lesson created", null, "ok");
                    await Navigation.PushAsync(new NewLessonPage());
                });
            }
        }

        private bool verifyEntredData()
        {
            string errorMsg = "";
            bool error = false;

            if (LessonNameEntry.Text == null || "".Equals(LessonNameEntry.Text))
            {
                errorMsg = "Lesson's name is required!";
                error = true;
            }
            else if (LessonFacultyEntry.Text == null || "".Equals(LessonFacultyEntry.Text))
            {
                errorMsg = "Lesson's faculty is required!";
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