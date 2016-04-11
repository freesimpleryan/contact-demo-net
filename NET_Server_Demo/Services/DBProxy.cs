using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using NET_Server_Demo.Models;

namespace NET_Server_Demo.Services
{
    public static class DBProxy
    {
        //private static string CON_STRING = "Data Source=call-notes-dev.database.windows.net;Initial Catalog=CALL_NOTES_DEV;User ID=calldev;Password=H7rpaBxJbBW2FcWF;";
        private static string CON_STRING;

        private static SqlConnection SQL = new SqlConnection(CON_STRING);

        static DBProxy(){
            // Read in the connection string
            try
            {
                System.IO.StreamReader iFile = new System.IO.StreamReader("..\\..\\..\\local\\sql_con.txt");
                string line;
                while ((line = iFile.ReadLine()) != null)
                {
                    CON_STRING = line;
                }
                iFile.Close();

            }
            catch (Exception e) {
                Console.WriteLine("ERROR WITH DATABASE SETTINGS FILE");
                Console.WriteLine(e.ToString());
                throw new Exception();
            }
            initTestConnection();
        }

        private static void initTestConnection() {
            if (!testConnection()) {
                throw new Exception();
            }
        }

        public static bool testConnection(){
            try
            {
                SQL.Open();
                SQL.Close();
                return true;
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public static Dictionary<String, Object> query(string qString) {
            Dictionary<String, Object> returnDict = new Dictionary<string,object>();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader result;
            cmd.CommandText = qString;
            try
            {
                SQL.Open();
                result = cmd.ExecuteReader();
                returnDict = Enumerable.Range(0, result.FieldCount).ToDictionary(result.GetName, result.GetValue);
                SQL.Close();
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                returnDict.Add("error", e);
            }
            return returnDict;
        }

        public static Contact[] getAllContacts() {
            Contact[] validation = new Contact()[];
            string qString = "SELECT * FROM Contact WHERE RowNumber BETWEEN 1 AND 10;";
            SqlCommand cmd = new SqlCommand();
            SqlDataReader result;
            cmd.CommandText = qString;

            try
            {
                SQL.Open();
                result = cmd.ExecuteReader();
                // TODO: Do stuff here
                SQL.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return validation;
        }

        public static Contact saveContact(Contact newContact){
            Contact validation = new Contact();
            newContact.Id = Guid.NewGuid().ToString();
            string qString = "INSERT INTO Contact (Id, FirstName, LastName, PhoneNumber, Email) VALUES ('"
                + newContact.Id + "','"
                + newContact.FirstName + "','"
                + newContact.LastName + "','"
                + newContact.PhoneNumber + "','"
                + newContact.Email + "');";
            SqlCommand cmd = new SqlCommand();
            SqlDataReader result;
            cmd.CommandText = qString;

            try
            {
                SQL.Open();
                result = cmd.ExecuteReader();
                validation = newContact;
                SQL.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return validation;
        }

    }
}