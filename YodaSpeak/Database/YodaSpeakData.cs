using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.Data;
using System.IO;

namespace YodaSpeak.Database
{
    public class YodaSpeakDBHelper
    {
        SQLiteConnection mConn;
        SQLiteDataAdapter mAdapter;
        DataTable mTable;

        private static YodaSpeakDBHelper authInstance;
        public static YodaSpeakDBHelper Instance
        {
            get
            {
                if (authInstance == null)
                    authInstance = new YodaSpeakDBHelper();

                return authInstance;
            }
        }

        public YodaSpeakDBHelper()
        {
            string mDbPath = Path.Combine(Environment.CurrentDirectory, @"YodaDB.db");
            mConn = new SQLiteConnection("Data Source=" + mDbPath);
            CreateTable();
        }
        private void CreateTable()
        {
            mConn.Open();
            using (SQLiteCommand mCmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS [YodaSpeak] (id INTEGER PRIMARY KEY AUTOINCREMENT, 'OriginalText' TEXT, 'TransalatedText' TEXT);", mConn))
            {
                mCmd.ExecuteNonQuery();
            }
            mConn.Close();
        }
        public void addRecord(string orignal, string translate)
        {
            if (!checkExistRecord(orignal))
            {
                string myQuery = $"insert into YodaSpeak (OriginalText, TransalatedText) values('{orignal}','{translate}')";
                using (SQLiteCommand command = new SQLiteCommand(myQuery, mConn))
                {
                    mConn.Open();
                    command.ExecuteNonQuery();
                    mConn.Close();
                }
            }
        }
        public string getRecord(string orignal)
        {
            string TransalatedText = string.Empty;
            string myQuery = $"select TransalatedText from YodaSpeak where OriginalText='{orignal}'";
            using (SQLiteCommand command = new SQLiteCommand(myQuery, mConn))
            {
                mConn.Open();
                SQLiteDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                    TransalatedText = dataReader["TransalatedText"].ToString();
                dataReader.Close();
                mConn.Close();
            }
            return TransalatedText;
        }
        private bool checkExistRecord(string orignal)
        {
            bool checkRows;
            string myQuery = $"select * from YodaSpeak where OriginalText='{orignal}'";
            using (SQLiteCommand command = new SQLiteCommand(myQuery, mConn))
            {
                mConn.Open();
                SQLiteDataReader dataReader = command.ExecuteReader();
                checkRows = dataReader.HasRows;
                mConn.Close();
            }
            return checkRows;
        }

    }
}
