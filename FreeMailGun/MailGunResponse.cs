using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeMailGun
{
    public class MailGunResponse
    {
        public MailGunResponse()
        {
            ResponseCode = MailGunResponseCode.Unknown;
            Message = string.Empty;
        }

        public MailGunResponseCode ResponseCode { get; set; }
        public string Message { get; set; }

        public void ParseResponseCode(int value)
        {
            MailGunResponseCode code;
            if (Enum.TryParse(value.ToString(), out code))
                ResponseCode = code;
        }

        public override string ToString()
        {
            return "HTTP Status: " + ResponseCode + " " + Message;
        }
    }

}
