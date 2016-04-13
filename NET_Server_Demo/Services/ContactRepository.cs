using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NET_Server_Demo.Models;


namespace NET_Server_Demo.Services
{
    public class ContactRepository
    {
        private string CacheKey = "ContactStore";
        public ContactRepository()
        {
            /*
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var contacts = new Contact[]
                    {
                        new Contact
                        {
                            Id = 1, FirstName = "Glenn", LastName = "Test", 
                        },
                        new Contact
                        {
                            Id = 2, Name = "Dan Roth"
                        }
                    };

                    ctx.Cache[CacheKey] = contacts;
                }
            }
              */
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