using FreeMailGun;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeIdentity
{

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var mailer = new MailGunClient(ConfigurationManager.AppSettings["MailGunApiKey"], ConfigurationManager.AppSettings["MailGunURL"]);
            var msg = new MailGunMessage();

            msg.AddTo(message.Destination);

            var ccemail = ConfigurationManager.AppSettings["MailGunCCEmail"];
            if (!string.IsNullOrWhiteSpace(ccemail))
            {
                msg.AddCc(ConfigurationManager.AppSettings["MailGunCCEmail"]);
            }

            msg.From = ConfigurationManager.AppSettings["MailGunFromEmailPasswordRecovery"];
            msg.Subject = message.Subject;
            msg.Text = message.Body;

            mailer.SendMessage(msg);
            return Task.FromResult(0);
        }
    }
}
