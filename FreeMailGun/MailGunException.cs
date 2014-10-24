using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeMailGun
{
    public class MailGunException : Exception
    {
        private readonly MailGunResponse mailgunResponse;
        
        public MailGunException(MailGunResponse mailgunResponse)
        {
            this.mailgunResponse = mailgunResponse;
        }

        public override string Message
        {
            get { return "HTTP Status: " + mailgunResponse.ResponseCode + " " + mailgunResponse.Message; }
        }
    }
}
