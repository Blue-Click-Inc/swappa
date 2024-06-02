using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Data.Services
{
    public class Notify : INotify
    {
        private readonly IMailjetClient mailClient;
        private readonly UserManager<AppUser> userManager;
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;
        private readonly string emailAddress;

        public Notify(IMailjetClient mailClient, IConfiguration configuration, 
            UserManager<AppUser> userManager, IRepositoryManager repository,
            ApiResponseDto response)
        {
            this.mailClient = mailClient;
            this.userManager = userManager;
            this.repository = repository;
            this.response = response;
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

        public async Task<ResponseModel<string>> SendAccountEmailAsync(AppUser user, StringValues origin, TokenType tokenType)
        {
            var token = string.Empty;
            switch (tokenType)
            {
                case TokenType.AccountConfirmation:
                    token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    break;
                case TokenType.PasswordReset:
                    token = await userManager.GeneratePasswordResetTokenAsync(user);
                    break;
                default:
                    break;
            }      
            
            var url = origin.BuildAccountUrl(token, tokenType);
            var message = string.Empty;
            switch (tokenType)
            {
                case TokenType.AccountConfirmation:
                    message = Statics.GetAccountConfirmationTemplate(url, user.Name);
                    break;
                case TokenType.PasswordReset:
                    message = Statics.GetPasswordResetTemplate(url);
                    break;
                default:
                    break;
            }
            
            var success = await SendAsync(user.Email, message, tokenType.GetDescription());
            return await ProcessResponse(success, user, token, tokenType);    
        }

        private async Task<ResponseModel<string>> ProcessResponse(bool? success, AppUser user, string token, TokenType tokenType)
        {
            if (success.GetValueOrDefault())
            {
                var tokenToAdd = new Token
                {
                    UserId = user.Id,
                    Type = tokenType,
                    Value = token
                };
                await repository.Token.AddAsync(tokenToAdd);
                if(tokenType == TokenType.AccountConfirmation)
                {
                    return response
                    .Process<string>(new ApiOkResponse<string>("Registration successful. Please check your email to confirm your account"));
                }
                else
                {
                    return response
                    .Process<string>(new ApiOkResponse<string>("Password reset successful. Please check your email to change mail to set a new password password."));
                }

            }
            else
            {
                if(tokenType == TokenType.AccountConfirmation)
                {
                    var userToDelete = await userManager.FindByIdAsync(user.Id.ToString());
                    if (userToDelete != null)
                    {
                        await userManager.DeleteAsync(userToDelete);
                    }
                    return response
                        .Process<string>(new BadRequestResponse("Registration failed! Please try again later."));
                }
                else
                {
                    return response
                        .Process<string>(new ApiOkResponse<string>("Password reset failed. Could not send reset toekn. Please try again later."));
                }
            }
        }
    }
}
