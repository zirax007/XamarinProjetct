using MyClass.Tables;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyClass.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStudentPage : ContentPage
    {
        public AddStudentPage()
        {
            InitializeComponent();
            var db = DBConnection.connect();
            db.CreateTable<LessonTable>();

            LessonsPicker.ItemsSource = db.Table<LessonTable>().ToList();
            LessonsPicker.SelectedIndexChanged += delegate
            {
                var item = (LessonTable) LessonsPicker.SelectedItem;
                if (item != null)
                    StudentFacultyEntry.Text = item.Faculty;
                else
                    StudentFacultyEntry.Text = "";
            };
        }

        private async void CancelAction(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private void AddStudentAction(object sender, EventArgs e)
        {
            if (verifyEntredData())
            {
                var db = DBConnection.connect();
                db.CreateTable<StudentTable>();
                db.CreateTable<LessonTable>();
                db.CreateTable<StudentLessonTable>();

                LessonTable selectedLesson = (LessonTable)LessonsPicker.SelectedItem;

                StudentTable student = db.Table<StudentTable>().Where(s => s.LastName.Equals(StudentLastNameEntry.Text)).FirstOrDefault();

                if(student == null)
                {
                    student = new StudentTable()
                    {
                        CIN = StudentCINEntry.Text,
                        FirstName = StudentFirstNameEntry.Text,
                        LastName = StudentLastNameEntry.Text,
                        Email = StudentEmailEntry.Text,
                        Phone = StudentPhoneEntry.Text,
                        Faculty = StudentFacultyEntry.Text
                    };
                    student.Lessons = new List<LessonTable> { selectedLesson };
                    db.Insert(student);
                } 

                StudentLessonTable studentLesson = new StudentLessonTable()
                {
                    StudentId = student.Id,
                    LessonId = selectedLesson.Id,
                    Absent = false
                };

                db.Insert(studentLesson);


                StudentTable studentStored = db.GetWithChildren<StudentTable>(student.Id);
                LessonTable lessonStored = db.GetWithChildren<LessonTable>(selectedLesson.Id);
                StudentLessonTable studentLessonStored = db.GetWithChildren<StudentLessonTable>(studentLesson.Id);

                var ss = new StudentTable();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await this.DisplayAlert("Congratulations", "Student Inscription Successfull", null, "Ok");
                });
            }
        }

        private bool verifyEntredData()
        {
            string errorMsg = "";
            bool error = false;

            if (LessonsPicker.SelectedItem == null)
            {
                errorMsg = "Lesson is required!";
                error = true;
            }
            else if (StudentLastNameEntry.Text == null || "".Equals(StudentLastNameEntry.Text))
            {
                errorMsg = "Student's last name is required!";
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