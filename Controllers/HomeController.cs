using EmailSender;
using EmailSender.Email;
using EmailSender.Models.Domains;
using EmailSender.Models.Domains.Repositories;
using EmailSender.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;

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
        private WebConfigRepository _webConfigRepository = new WebConfigRepository();

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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmailSend(EmailMessage emailMessage)
        {
            var userId = User.Identity.GetUserId();
            var emailRecipient = _emailRecipientRepository.GetEmailRecipient(emailMessage.EmailRecipientId, userId);
            var emailSettings = new EmailParamsViewModel();

            emailSettings = _webConfigRepository.GetParamsViewModel();

            var email = new EmailMsg(emailSettings.EmailParams);

            if (!ModelState.IsValid)
            {
                var vm = new SendEmailMessageViewModel();
                vm.EmailRecipients = _emailRecipientRepository.GetEmailRecipients(userId);
                vm.EmailMessages = _emailMessageRepository.GetMessages(userId);
                vm.Heading = "Wysyłanie wiadomości email";

                return View("EmailSend", vm);
            }
            emailMessage.UserId = userId;
            emailMessage.DateOfSending = DateTime.Now;

            await email.Send(emailMessage.Subject, emailMessage.Body, emailRecipient.EmailAddress);
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
        [ValidateAntiForgeryToken]
        public ActionResult EmailRecipient(EmailRecipient emailRecipient)
        {
            var userId = User.Identity.GetUserId();
            emailRecipient.UserId = userId;



            if (!ModelState.IsValid)
            {
                var vm = new EmailRecipient
                {
                    Name = emailRecipient.Name,
                    EmailAddress = emailRecipient.EmailAddress,
                    UserId = userId
                };
                //ViewBag.Message = ModelState.;

                return View("EmailRecipient", vm);
            }

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


        [HttpPost]
        [Authorize]
        public ActionResult DeleteEmailRecipient(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                _emailRecipientRepository.Delete(id, userId);
            }
            catch (Exception exception)
            {
                return Json(new { Success = false, Message = exception.Message });
            }

            return Json(new { Success = true });

        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteEmailMessage(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                _emailMessageRepository.Delete(id, userId);
            }
            catch (Exception exception)
            {
                return Json(new { Success = false, Message = exception.Message });
            }

            return Json(new { Success = true });

        }

        [Authorize]

        public ActionResult Configuration()
        {
            var emailSettings = new EmailParamsViewModel();

            emailSettings = _webConfigRepository.GetParamsViewModel();

            return View(emailSettings);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Configuration(EmailParams emailParams)
        {
            var emailParamVM = new EmailParamsViewModel();
            emailParamVM.EmailParams = new EmailParams();
            emailParamVM.EmailParams = emailParams;
            _webConfigRepository.SetParamsViewModel(emailParamVM);

            return RedirectToAction("Configuration");
        }
    }
}