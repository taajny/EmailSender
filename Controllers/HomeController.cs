using EmailSender.Models.Domains;
using EmailSender.Models.Domains.Repositories;
using EmailSender.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmailSender.Controllers
{
    public class HomeController : Controller
    {
        // min 44:23 - Widoki -> boostarp date picker
        // min 49 - widok partial 

        // KONTROLERY
        // min.5 - przekierowania do innych akcji
        // min 37 - akcje post do usuwania z użyciem Ajax
        private EmailMessageRepository _emailMessageRepository = new EmailMessageRepository();
        private EmailRecipientRepository _emailRecipientRepository = new EmailRecipientRepository();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        
        [Authorize]
        public ActionResult EmailSend()
        {
            var emailMessage = new SendEmailMessageViewModel();
            var userId = User.Identity.GetUserId();

            emailMessage.EmailRecipients = _emailRecipientRepository.GetEmailRecipients(userId);
            emailMessage.EmailMessages = _emailMessageRepository.GetMessages(userId);
            emailMessage.Heading = "Wysyłanie wiadomości email";
            
            emailMessage.EmailMessage = new EmailMessage
            {
                UserId = userId,
                
            };

            return View(emailMessage);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EmailSend(EmailMessage emailMessage)
        {
            var userId = User.Identity.GetUserId();
            emailMessage.UserId = userId;
            emailMessage.DateOfSending = DateTime.Now;

            _emailMessageRepository.Add(emailMessage);
            return RedirectToAction("EmailSend", "Home");
        }

        [Authorize]
        public ActionResult AddressBook()
        {
            var userId = User.Identity.GetUserId();
            var emailRecipients = _emailRecipientRepository.GetEmailRecipients(userId);

            return View(emailRecipients);
        }

        [Authorize]
        public ActionResult EmailRecipient(int id = 0)
        {
            var userId = User.Identity.GetUserId();

            EmailRecipient emailRecipient = null;
            
            if (id == 0)
            {
                ViewBag.Message = "Dodawanie nowego Odbiorcy";
                emailRecipient = new EmailRecipient
                {
                    UserId = userId,
                };
            }
            else
            {
                ViewBag.Message = "Edycja Odbiorcy";
                emailRecipient = _emailRecipientRepository.GetEmailRecipient(id, userId);
            }
            
            return View(emailRecipient);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EmailRecipient(EmailRecipient emailRecipient)
        {
            var userId = User.Identity.GetUserId();
            emailRecipient.UserId = userId;

            /*if (!ModelState.IsValid)
            {
                //var vm = PrepareInvoiceViewModel(invoice, userId);
                return View("EmailRecipient", emailRecipient);
            }*/

            if (emailRecipient.Id == 0)
            {
                _emailRecipientRepository.Add(emailRecipient);
            }
            else
            {
                _emailRecipientRepository.Update(emailRecipient, userId);
            }
            return RedirectToAction("AddressBook");
        }

        [Authorize]
        public ActionResult Configuration()
        {
            ViewBag.Message = "Konfiguracja serwera smtp.";

            return View();
        }
    }
}