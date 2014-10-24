using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.IO;

namespace FreeMailGun
{

 
    public class MailGunClient : IDisposable
    {
        private readonly string apiKey;
        private HttpClient httpClient;

        private const long maxFileSizeBytes = 5243000;      // about 5 MB

        public MailGunClient(string apiKey, string baseUrl)
        {
            if (String.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
            if (String.IsNullOrEmpty(baseUrl))
                throw new ArgumentException("baseUrl");

            this.apiKey = apiKey;
            InitializeHttpClient(baseUrl);
        }

        private void InitializeHttpClient(string baseUrl)
        {
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl),
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(apiKey)));
        }

        public MailGunResponse SendMessage(MailGunMessage msg)
        {
            if (msg == null)
                throw new ArgumentNullException("msg");
            if (!msg.To.Any())
                throw new ArgumentException("Cannot send MailGunMessage without a recipient");

            var requestContent = new MultipartFormDataContent();
           
            int attachmentCount = 0;
            foreach (var attachmentPath in msg.Attachments)
            {
                attachmentCount++;
                if (File.Exists(attachmentPath))
                {
                    string fileName = Path.GetFileName(attachmentPath);
                    var fileLength = new FileInfo(attachmentPath).Length;
                    if (attachmentCount > 5)
                    {
                        msg.Text = (msg.Text ?? string.Empty) + " {Attachment " + fileName + " NOT SENT. Maximum number of attachments reached.}";
                        if (!string.IsNullOrEmpty(msg.Html))
                        {
                            msg.Html += " {Attachment " + fileName + " NOT SENT. Maximum number of attachments reached.}";
                        }
                    }
                    else if (fileLength < maxFileSizeBytes)     // Actaully send the attachment.
                    {
                        var fileBytes = File.ReadAllBytes(attachmentPath);
                        var attachmentContent = new ByteArrayContent(fileBytes);
                        attachmentContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        
                        requestContent.Add(attachmentContent, "attachment", fileName);
                    }
                    else
                    {
                        msg.Text = (msg.Text ?? string.Empty) + " {Attachment " + fileName + " NOT SENT. File size too large.}";
                        if (!string.IsNullOrEmpty(msg.Html))
                        {
                            msg.Html += " {Attachment " + fileName + " dropped. File size too large.}";
                        }
                    }
                }
            }

            // This puts the form data of the email at the *end* of the HTTP Request. I'm not really a fan of this, but we need to fiddle w/ the message content while adding in the attachments.
            foreach (var msgItem in msg.ToKvpList())
                requestContent.Add(new StringContent(msgItem.Value), msgItem.Key);

            var responseTask = httpClient.PostAsync(MailGunMethods.MESSAGES, requestContent);
            
            MailGunResponse mailgunResponse = new MailGunResponse();

            try
            {
                using (HttpResponseMessage responseMessage = responseTask.Result)
                {
                    mailgunResponse.ParseResponseCode((int)responseMessage.StatusCode);
                    mailgunResponse.Message = responseMessage.Content.ReadAsStringAsync().Result;
                }
            }
            catch (HttpRequestException requestException)
            {
                mailgunResponse.Message = "Exception during MailGun SendMessage: " + requestException.Message;
            }
            return mailgunResponse;
        }

        public void Dispose()
        {
            if (httpClient != null)
                httpClient.Dispose();
        }
    }
}

