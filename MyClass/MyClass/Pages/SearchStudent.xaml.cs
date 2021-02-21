using MyClass.Tables;
using Rg.Plugins.Popup.Services;
using SQLite;
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
    public partial class SearchStudent : ContentPage
    {
        SQLiteConnection db = DBConnection.connect();
        public SearchStudent()
        {
            InitializeComponent();
            db.CreateTable<StudentTable>();
            db.CreateTable<LessonTable>();
            db.CreateTable<StudentLessonTable>();

            List<LessonTable> lessons = db.Table<LessonTable>().ToList();
            LessonsPicker.ItemsSource = lessons;
        }
        private void StudentLastNameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue;
            StudentTable student = db.Table<StudentTable>().Where(s => s.LastName.Equals(searchText)).FirstOrDefault();
            if (student != null)
            {
                StudentFacultyEntry.Text = student.Faculty;
                LessonsPicker.ItemsSource = StudentLessons(student.LastName);
            } else
            {
                StudentFacultyEntry.Text = "";
                LessonsPicker.ItemsSource = null;
            }
        }
        
         private List<LessonTable> StudentLessons(string name)
         {
             StudentTable studentsWithName = db.Table<StudentTable>().Where(s => s.LastName.Equals(name)).FirstOrDefault();

             List < StudentLessonTable> studentLessonT = db.Table<StudentLessonTable>().Where(slt => slt.StudentId == studentsWithName.Id).ToList();
            
             List<LessonTable> lessons = new List<LessonTable>();
             foreach (StudentLessonTable ls in studentLessonT)
             {
                LessonTable lesson = db.Table<LessonTable>().Where(l => l.Id == ls.LessonId).FirstOrDefault();
                lessons.Add(lesson);
             }

             return lessons;
         }

        private async void CancelAction(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private void SearchStudentAction(object sender, EventArgs e)
        {
            var sname = StudentLastNameEntry.Text;
            LessonTable selectedLesson = (LessonTable)LessonsPicker.SelectedItem;
            string PopUpAbsence = null;
            string PopUpPresence = null;
            if(sname != null && !"".Equals(sname))
            {
                StudentTable studentsWithName = db.Table<StudentTable>().Where(s => s.LastName.Equals(sname)).FirstOrDefault();
                
                if(selectedLesson != null)
                {
                    StudentLessonTable studentLesson = db.Table<StudentLessonTable>().Where(sl => sl.LessonId == selectedLesson.Id 
                                                                && sl.StudentId == studentsWithName.Id).FirstOrDefault();
                    if(studentLesson != null && studentLesson.Absent)
                    {
                        PopUpAbsence = "Absent";
                    } else
                    {
                        PopUpPresence = "Present";
                    }
                } else
                {
                    List<StudentLessonTable> studentLessonT = db.Table<StudentLessonTable>().Where(slt => slt.StudentId == studentsWithName.Id).ToList();

                    var totalAbsent = 0; var totalPresent = 0;
                    foreach (StudentLessonTable ls in studentLessonT)
                    {
                        if (ls.Absent)
                            totalAbsent++;
                        else
                            totalPresent++;
                    }
                    PopUpAbsence = "Absent : " + totalAbsent;
                    PopUpPresence = "Present : " + totalPresent;
                }

                string PopUpTitle = studentsWithName.LastName + " " + studentsWithName.FirstName;
                PopupNavigation.Instance.PushAsync(new InfosPopupView(PopUpTitle, PopUpAbsence, PopUpPresence));
            } else
            {
                if (selectedLesson != null)
                {
                    List<StudentLessonTable> studentLessonT = db.Table<StudentLessonTable>().Where(sl => sl.LessonId == selectedLesson.Id).ToList();
                    var totalAbsent = 0; var totalPresent = 0;
                    foreach (StudentLessonTable ls in studentLessonT)
                    {
                        if (ls.Absent)
                            totalAbsent++;
                        else
                            totalPresent++;
                    }

                    PopUpAbsence = "Absent : " + totalAbsent;
                    PopUpPresence = "Present : " + totalPresent;
                    string studentName = selectedLesson.Name;
                    PopupNavigation.Instance.PushAsync(new InfosPopupView(studentName, PopUpAbsence, PopUpPresence));
                }
            }
        }
    }
}