using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
namespace MyClass.Tables
{
    [Table("Professors")]
    class ProfessorTable
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    [Table("Lessons")]
    class LessonTable
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Faculty { get; set; }
        public DateTime Date { get; set; }
        [ManyToMany(typeof(StudentLessonTable))]
        public List<StudentTable> Students { get; set; }

    }

    [Table("Students")]
    class StudentTable
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string CIN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Faculty { get; set; }
        [ManyToMany(typeof(StudentLessonTable))]
        public List<LessonTable> Lessons { get; set; }
        public bool Absent { get; set; }

        internal object Where(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }

    class StudentLessonTable
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        [ForeignKey(typeof(LessonTable))]
        public long LessonId { get; set; }
        [ForeignKey(typeof(StudentTable))]
        public long StudentId { get; set; }
        public bool Absent { get; set; }

    }
}
