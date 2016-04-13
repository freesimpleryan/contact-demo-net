using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NET_Server_Demo.Models;
using NET_Server_Demo.Services;

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
            if (contact.Id == null || contact.Id == "") {
                contact.Id = Guid.NewGuid().ToString();
            }
            this.contactRepository.SaveContact(contact);

            var response = Request.CreateResponse<Contact>(System.Net.HttpStatusCode.Created, contact);

            return response;
        }
    }
}
