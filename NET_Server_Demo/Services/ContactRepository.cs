using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NET_Server_Demo.Models;


namespace NET_Server_Demo.Services
{
    public class ContactRepository
    {
        public ContactRepository()
        {
        }

        public bool SaveContact(Contact contact)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    DBProxy.saveContact(contact);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;
        }

        public Contact[] GetAllContacts()
        {
            return DBProxy.getAllContacts();
        }
    }
}