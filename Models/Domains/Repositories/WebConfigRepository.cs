using EmailSender.Email;
using EmailSender.Models.ViewModels;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services.Description;
using static System.Net.Mime.MediaTypeNames;

namespace EmailSender.Models.Domains.Repositories
{
    public class WebConfigRepository
    {
        public EmailParamsViewModel GetParamsViewModel()
        {
            var emailParamsVM = new EmailParamsViewModel();
            emailParamsVM.EmailParams = new EmailParams();           

            emailParamsVM.EmailParams.HostSmtp = WebConfigurationManager.AppSettings["HostSmtp"].ToString();
            emailParamsVM.EmailParams.EnableSsl = Convert.ToBoolean(WebConfigurationManager.AppSettings["EnableSsl"]);
            emailParamsVM.EmailParams.Port = Convert.ToInt32(WebConfigurationManager.AppSettings["Port"]);
            emailParamsVM.EmailParams.SenderEmail = WebConfigurationManager.AppSettings["SenderEmail"].ToString();
            emailParamsVM.EmailParams.SenderEmailPassword = WebConfigurationManager.AppSettings["SenderEmailPassword"].ToString();
            emailParamsVM.EmailParams.SenderName = WebConfigurationManager.AppSettings["SenderName"].ToString();

            
            return emailParamsVM;
        }

        public void SetParamsViewModel(EmailParamsViewModel emailParamsVM)
        {

            var configFile = WebConfigurationManager.OpenWebConfiguration("~/");
            
            configFile.AppSettings.Settings["HostSmtp"].Value = emailParamsVM.EmailParams.HostSmtp;
            configFile.AppSettings.Settings["EnableSsl"].Value = emailParamsVM.EmailParams.EnableSsl.ToString();
            configFile.AppSettings.Settings["Port"].Value = emailParamsVM.EmailParams.Port.ToString();
            configFile.AppSettings.Settings["SenderEmail"].Value = emailParamsVM.EmailParams.SenderEmail;
            configFile.AppSettings.Settings["SenderEmailPassword"].Value = emailParamsVM.EmailParams.SenderEmailPassword;
            configFile.AppSettings.Settings["SenderName"].Value = emailParamsVM.EmailParams.SenderName;

            configFile.Save();
        }
    }
}