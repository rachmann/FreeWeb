using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace FreeMailGun
{
    public class MailGunMessage 
    {
        public string From { get; set; }
        public ICollection<string> To { get; private set; }
        public ICollection<string> Cc { get; private set; }
        public ICollection<string> Bcc { get; private set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public string Html { get; set; }
        public string ReplyTo { get; set; }

        public ICollection<string> Tags { get; private set; }

        public ICollection<string> Attachments { get; private set; }

        public MailGunMessage()
        {
            To = new List<string>();
            Cc = new List<string>();
            Bcc = new List<string>();
            Tags = new List<string>();
            Attachments = new List<string>();
        }

        public void AddTo(string toAddress)
        {
            if (string.IsNullOrEmpty(toAddress))
                throw new ArgumentException("MailGunMessage toAddress cannot be null or empty", "toAddress");
            To.Add(toAddress);
        }

        public void AddCc(string ccAddress)
        {
            if (string.IsNullOrEmpty(ccAddress))
                throw new ArgumentException("MailGunMessage ccAddress cannot be null or empty", "ccAddress");
            Cc.Add(ccAddress);
        }

        public void AddBcc(string bccAddress)
        {
            if (string.IsNullOrEmpty(bccAddress))
                throw new ArgumentException("MailGunMessage bccAddress cannot be null or empty", "bccAddress");

            Bcc.Add(bccAddress);
        }

        public void AddTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                throw new ArgumentException("MailGunMessage tag cannot be null or empty", "tag");
            Tags.Add(tag);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown is required fields are not populated.</exception>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, string>> ToKvpList()
        {
            if (string.IsNullOrEmpty(From))
                throw new InvalidOperationException("From field is required to prepare a MailGunMessage");
            if (!To.Any())
                throw new InvalidOperationException("One recipient at minimum in the To field is required to prepare a MailGunMessage");
            
            IList<KeyValuePair<string, string>> kvps = new List<KeyValuePair<string, string>>();

            kvps.Add(new KeyValuePair<string, string>("from", From));
            kvps.Add(new KeyValuePair<string, string>("to", string.Join(",", To)));
            
            if (Cc.Any())
                kvps.Add(new KeyValuePair<string, string>("cc", string.Join(",", Cc)));
            if (Bcc.Any())
                kvps.Add(new KeyValuePair<string, string>("bcc", string.Join(",", Bcc)));
            if (Subject != null)
                kvps.Add(new KeyValuePair<string, string>("subject", Subject));

            // Ensure that we have body content to send. MailGun will reject any messages without [Text] or [Html]
            if (string.IsNullOrEmpty(Text) && string.IsNullOrEmpty(Html))
                Text = " ";

            if (Text != null)
                kvps.Add(new KeyValuePair<string, string>("text", Text));
            if (Html != null)
                kvps.Add(new KeyValuePair<string, string>("html", Html));
            if (!string.IsNullOrEmpty(ReplyTo))
                kvps.Add(new KeyValuePair<string, string>("h:Reply-To", ReplyTo));
            if (Tags.Any())
            {
                foreach (var tag in Tags.Where(x => !string.IsNullOrEmpty(x)))
                    kvps.Add(new KeyValuePair<string, string>("o:tag", tag));
            }
            return kvps;
        }
    }
}
