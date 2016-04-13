using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NET_Server_Demo.Models;
using NET_Server_Demo.Services;
using System.Text.RegularExpressions;

namespace NET_Server_Demo.Controllers
{
    public class ContactController : ApiController
    {
        private ContactRepository contactRepository;

        public ContactController()
        {
            this.contactRepository = new ContactRepository();
        } 

        public Contact[] Get()
        {
            return contactRepository.GetAllContacts();
        }

        public HttpResponseMessage Post(Contact contact)
        {
            contact.Id = Guid.NewGuid().ToString();
            Regex name = new Regex("[a-z|A-Z]{3,}");
            Regex email = new Regex("[a-z|A-Z|0-9]{3,}@[a-z|A-Z|0-9]{3,}\\.[a-z|A-Z|0-9]{3,}");
            Regex phone = new Regex("[2-9]{1}[0-9]{9}");
            if (name.IsMatch(contact.FirstName) &&
                name.IsMatch(contact.LastName) &&
                email.IsMatch(contact.Email) &&
                phone.IsMatch(contact.PhoneNumber)
                )
            {
                this.contactRepository.SaveContact(contact);
                var response = Request.CreateResponse<Contact>(System.Net.HttpStatusCode.Created, contact);
                return response;
            }
            else {
                var response = Request.CreateResponse<Contact>(System.Net.HttpStatusCode.NotAcceptable, contact);
                return response;
            }
        }
    }
}
