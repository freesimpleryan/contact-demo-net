using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using NET_Server_Demo.Models;
using System.Data.Objects;
using System.Diagnostics;

namespace NET_Server_Demo.Services
{
    public static class DBProxy
    {
        //private static string CON_STRING = "Data Source=call-notes-dev.database.windows.net;Initial Catalog=CALL_NOTES_DEV;User ID=calldev;Password=H7rpaBxJbBW2FcWF;";
        private static string CON_STRING;
        private static string FILE_PATH = "C:\\local\\sql_con.txt";

        private static SqlConnection SQL;

        static DBProxy(){
            // Read in the connection string
            Debug.WriteLine("INSIDE DBPROXY CONSTRUCTOR");
            try
            {
                if (System.IO.File.Exists(@FILE_PATH))
                {
                    Debug.WriteLine("------ FILE EXISTS AT PATH");
                }
                else {
                    Debug.WriteLine("FILE NOT FOUND!!!!!");
                }
                Debug.WriteLine("Attempting file read...");
                System.IO.StreamReader iFile = new System.IO.StreamReader(@FILE_PATH);
                string line;
                while ((line = iFile.ReadLine()) != null)
                {
                    CON_STRING = line;
                }
                iFile.Close();
                SQL = new SqlConnection(CON_STRING);
            }
            catch (Exception e) {
                Debug.WriteLine("ERROR WITH DATABASE SETTINGS FILE");
                Debug.WriteLine(e.ToString());
                throw new Exception();
            }
            Debug.WriteLine("Attempting test connection...");
            initTestConnection();
        }

        private static void initTestConnection() {
            if (!testConnection()) {
                Debug.WriteLine("TEST CONNECTIN FAILED");
                throw new Exception();
            }
        }

        public static bool testConnection(){
            try
            {
                SQL = new SqlConnection(CON_STRING);
                SQL.Open();
                SQL.Close();
                return true;
            }
            catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return false;
            }
        }

        public static Dictionary<String, Object> query(string qString) {
            Dictionary<String, Object> returnDict = new Dictionary<string,object>();    
            SqlDataReader result;
            try
            {
                SQL = new SqlConnection(CON_STRING);
                SQL.Open();
                SqlCommand cmd = new SqlCommand(qString, SQL);
                result = cmd.ExecuteReader();
                returnDict = Enumerable.Range(0, result.FieldCount).ToDictionary(result.GetName, result.GetValue);
                result.Close();
                SQL.Close();
            }
            catch (Exception e) {
                Debug.WriteLine(e.ToString());
                returnDict.Add("error", e);
            }
            return returnDict;
        }

                  

        public static Contact[] getAllContacts() {
            string qString = "SELECT * FROM Contact;";
            SqlDataReader result;
            Contact[] validation = new Contact[1];
            try
            {
                SQL = new SqlConnection(CON_STRING);
                SQL.Open();
                SqlCommand cmd = new SqlCommand(qString, SQL);
                result = cmd.ExecuteReader();
                int counter = 0;
                validation = new Contact[result.VisibleFieldCount];
                while(result.Read()){
                    validation[counter] = readRow((IDataRecord)result);
                    counter++;
                }
                result.Close();
                SQL.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            return validation;
        }

        private static Contact readRow(IDataRecord row) {
            Contact temp = new Contact();
            try
            {
                temp.Id = row[0].ToString();
                temp.FirstName = row[1].ToString();
                temp.LastName = row[2].ToString();
                temp.PhoneNumber = row[3].ToString();
                temp.Email = row[4].ToString();
            }
            catch (Exception e) {
                return new Contact();
            }
            return temp;
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
            SqlDataReader result;
            try
            {
                SQL = new SqlConnection(CON_STRING);
                SQL.Open();
                SqlCommand cmd = new SqlCommand(qString, SQL);
                result = cmd.ExecuteReader();
                validation = newContact;
                result.Close();
                SQL.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            return validation;
        }

    }
}