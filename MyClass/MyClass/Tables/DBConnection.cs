using SQLite;
using System;
using System.IO;

namespace MyClass.Tables
{
    class DBConnection
    {
        public static SQLiteConnection connect()
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyClassDatabase.db");
            var db = new SQLiteConnection(dbpath);
            return db;
        }
    }
}
