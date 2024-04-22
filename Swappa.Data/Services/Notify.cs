using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Swappa.Data.Services.Interfaces;

namespace Swappa.Data.Services
{
    public class Notify : INotify
    {
        private readonly IMailjetClient mailClient;
        private readonly string emailAddress;

        public Notify(IMailjetClient mailClient, IConfiguration configuration)
        {
            this.mailClient = mailClient;
            var settings = configuration.GetSection("MailJet");
            this.emailAddress = settings["Email"]!;
        }

        public async Task<bool?> SendAsync(string to, string message, string subject)
        {
            MailjetResponse response = null!;
            var emails = new List<string>() { to };

            try
            {
                foreach (var mail in emails)
                {
                    string email = mail;
                    MailjetRequest request = new MailjetRequest { Resource = SendV31.Resource }
                     .Property(Send.Messages, new JArray {
                        new JObject
                        {
                            {
                                "From",new JObject
                                {
                                    {"Email",emailAddress},
                                    {"Name", "Swappa Inc."}
                                }
                            },
                            {
                                "To", new JArray
                                {
                                    new JObject
                                    {
                                        {"Email", mail },
                                    }
                                }
                            },
                            { "Subject", subject },
                            { "TextPart", message },
                            { "HtmlPart", message },
                            { "CustomId", "SwappaApp" }
                        }
                    });
                    response = await mailClient.PostAsync(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response?.IsSuccessStatusCode;
        }

        public async Task<bool?> SendAsync(string to, string message, string subject, IFormFile file)
        {
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            var fileBytes = stream.ToArray();
            var base64String = Convert.ToBase64String(fileBytes);

            MailjetResponse response = null!;
            var emails = new List<string>() { to };

            try
            {
                foreach (var mail in emails)
                {
                    string email = mail;
                    MailjetRequest request = new MailjetRequest { Resource = SendV31.Resource }
                     .Property(Send.Messages, new JArray {
                        new JObject
                        {
                            {
                                "From",new JObject
                                {
                                    {"Email", emailAddress},
                                    {"Name", "Swappa Inc."}
                                }
                            },
                            {
                                "To", new JArray
                                {
                                    new JObject
                                    {
                                        {"Email", mail },
                                    }
                                }
                            },
                            { "Subject", subject },
                            { "TextPart", message },
                            { "HtmlPart", message },
                            { "Attachments", new JArray
                                {
                                    new JObject
                                    {
                                        {"ContentType", "text/plain" },
                                        {"Filename", file.FileName },
                                        {"Base64Content", base64String }
                                    }
                                 }
                            },
                            { "CustomId", "SwappaApp" }
                        }
                    });
                    response = await mailClient.PostAsync(request);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return response?.IsSuccessStatusCode;
        }
    }
}
