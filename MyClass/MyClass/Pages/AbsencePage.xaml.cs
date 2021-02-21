using MyClass.Tables;
using SQLite;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyClass.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AbsencePage : ContentPage
    {
        SQLiteConnection db = DBConnection.connect();
        public AbsencePage()
        {
            InitializeComponent();
            db.CreateTable<StudentTable>();
            db.CreateTable<LessonTable>();
            db.CreateTable<StudentLessonTable>();

            List<LessonTable> lessons = db.Table<LessonTable>().ToList();

            LessonsPicker.ItemsSource = lessons;

            if (lessons != null && lessons.Count > 0)
            {
                LessonTable firstLesson = lessons[0];

                LessonsPicker.SelectedItem = firstLesson;
                LessonFacultyEntry.Text = firstLesson.Faculty;

                StudentsLV.ItemsSource = StudentsRegistredInLesson(firstLesson);
            }

            LessonsPicker.SelectedIndexChanged += delegate
            {
                var item = (LessonTable)LessonsPicker.SelectedItem;
                if (item != null)
                {
                    LessonFacultyEntry.Text = item.Faculty;
                    StudentsLV.ItemsSource = StudentsRegistredInLesson(item);
                }
                else
                    LessonFacultyEntry.Text = "";
            };

        }

        private List<StudentTable> StudentsRegistredInLesson(LessonTable lesson)
        {
            var studentsOfLesson = db.Table<StudentLessonTable>().Where(sl => sl.LessonId == lesson.Id).ToList();

            List<StudentTable> students = new List<StudentTable>();
            if (studentsOfLesson != null)
            {
                foreach (StudentLessonTable slt in studentsOfLesson)
                {
                    StudentTable student = db.Table<StudentTable>().Where(s => s.Id == slt.StudentId).FirstOrDefault();
                    student.Absent = slt.Absent;
                    students.Add(student);
                }
            }

            return students;
        }

        private async void CancelAction(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private void SaveAction(object sender, System.EventArgs e)
        {
            foreach(StudentTable s in StudentsLV.ItemsSource)
            {
                var selectedLesson = (LessonTable)LessonsPicker.SelectedItem;
                StudentLessonTable studentLesson = db.Table<StudentLessonTable>().Where(sl => sl.StudentId == s.Id && sl.LessonId == selectedLesson.Id).FirstOrDefault();
                studentLesson.Absent = s.Absent;
                db.Update(studentLesson);
            }
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Congratulations", "Operation Success!", null, "Ok");
            });
        }
    }
}