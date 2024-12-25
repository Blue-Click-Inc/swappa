using BlazorBootstrap;
using CloudinaryDotNet.Actions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Pipelines.Sockets.Unofficial.Buffers;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Server.Commands.Faq;
using Swappa.Server.Commands.Role;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;
using static System.Collections.Specialized.BitVector32;

namespace Swappa.Server.Extensions
{
    public static class HandlerExtensions
    {
        private const string PASS = "P@55w0rd";
        public static PagedList<Vehicle> MapLocations(this PagedList<Vehicle> vehicles, Dictionary<Guid, EntityLocation> locations)
        {
            vehicles.ForEach(v =>
            {
                v.Location = locations.GetValueOrDefault(v.Id);
            });

            return vehicles;
        }

        public static PagedList<Vehicle> MapImages(this PagedList<Vehicle> vehicles, Dictionary<Guid, List<Image>> images)
        {
            vehicles.ForEach(v =>
            {
                v.Images = images.GetValueOrDefault(v.Id) ?? new List<Image>();
            });

            return vehicles;
        }

        internal static async Task SeedSystemData(this WebApplication app, ILogger<Program> logger)
        {
            using var scope = app.Services.CreateScope();
            await scope.DoSeedSystemRoles(logger);
            await scope.SeedUsers(logger);
            await scope.DoSeedFaqs(logger);
        }

        private static async Task DoSeedFaqs(this IServiceScope scope, ILogger<Program> logger)
        {
            var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
            if (mediatr.IsNotNull())
            {
                var faqs = GetFaqsToCreate();
                if (faqs.IsNotNullOrEmpty())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();
                    if(await repo.Faq.Exists(f => !f.IsDeprecated))
                    {
                        logger.LogInformation("Skipping FAQs seeding...");
                        return;
                    }
                    logger.LogInformation($"Starting FAQs seeding...");
                    logger.LogInformation($"Adding {faqs.Count} FAQs to the database");
                    var res = await mediatr.Send(new CreateFaqsCommand
                    {
                        Requests = faqs
                    });

                    if (res.IsSuccessful)
                    {
                        logger.LogInformation(res.Data);
                    }
                    else
                    {
                        logger.LogError(res.Message);
                    }
                }
                else
                {
                    logger.LogInformation("Seeding skipped.... FAQs already exist in the database.");
                }
            }
        }

        private static List<FaqToCreateDto> GetFaqsToCreate()
        {
            return new List<FaqToCreateDto>
            {
                new() {
                    Title = "How do I list my car for sale?",
                    Details = "Log in to your account, click \"Sell a Car,\" and follow the step-by-step instructions."
                },
                new() {
                    Title = "What warranties are offered?",
                    Details = "We provide manufacturer warranties for new cars and extended warranties for certified pre-owned vehicles."
                },
                new() {
                    Title = " How do I reset my password?",
                    Details = "Go to the login page and click \"Forgot Password.\" Follow the instructions to reset it."
                }
            };
        }

        private static async Task DoSeedSystemRoles(this IServiceScope scope, ILogger<Program> logger)
        {
            var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
            if (mediatr.IsNotNull())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                if (roleManager.IsNotNull())
                {
                    if (roleManager.Roles.IsNullOrEmpty())
                    {
                        var roles = EnumExtension.AllRolesString();
                        logger.LogInformation($"Starting roles seeding: {string.Join(",", roles)}");
                        foreach (var role in roles)
                        {
                            logger.LogInformation($"Adding role: {role} to the database");
                            var res = await mediatr.Send(new AddRoleCommand
                            {
                                RoleName = role
                            });

                            if (res.IsSuccessful)
                            {
                                logger.LogInformation($"Successfully added role: {role} to the database");
                            }
                            else
                            {
                                logger.LogError($"Exception occurred while adding: {role} to the database");
                            }
                        }
                        return;
                    }

                    logger.LogInformation("Seeding skipped.... Roles already exist in the database.");
                }
            }
        }

        private static async Task SeedUsers(this IServiceScope scope, ILogger<Program> logger)
        {
            var users = new List<RegisterDto>
            {
                new() 
                {
                    Name = "Super Admin",
                    Email = "super.admin@example.com",
                    Gender = Entities.Enums.Gender.Male,
                    Role = Entities.Enums.SystemRole.SuperAdmin,
                    Password = PASS
                },
                new() 
                {
                    Name = "Admin",
                    Email = "admin@example.com",
                    Gender = Entities.Enums.Gender.Male,
                    Role = Entities.Enums.SystemRole.Admin,
                    Password = PASS
                },
                new() 
                {
                    Name = "Common User",
                    Email = "user001@example.com",
                    Gender = Entities.Enums.Gender.Female,
                    Role = Entities.Enums.SystemRole.User,
                    Password = PASS
                },
                new() 
                {
                    Name = "Merchant",
                    Email = "merchant@example.com",
                    Gender = Entities.Enums.Gender.Male,
                    Role = Entities.Enums.SystemRole.Merchant,
                    Password = PASS
                }
            };
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            if (userManager.IsNotNull())
            {
                if (userManager.Users.Any())
                {
                    logger.LogInformation("Seeding skipped.... Users already exist in the database.");
                }
                else
                {
                    foreach (var user in users)
                    {
                        await userManager.DoSeedUser(user, logger);
                    }
                }
            }
        }

        private static async Task DoSeedUser(this UserManager<AppUser> userManager, 
            RegisterDto dto, ILogger<Program> logger)
        {
            if (userManager.IsNotNull())
            {
                var user = await userManager.FindByEmailAsync(dto.Email);
                if (user.IsNull())
                {
                    user = new AppUser
                    {
                        Name = dto.Name,
                        Email = dto.Email,
                        Gender = dto.Gender,
                        EmailConfirmed = true,
                        UserName = dto.Email,
                        Status = Entities.Enums.Status.Active,
                    };

                    logger.LogInformation($"Adding User: {user.Name} to the database");
                    var result = await userManager.CreateAsync(user, dto.Password);
                    if (result.Succeeded)
                    {
                        logger.LogInformation($"Successfully added User: {user.Name} to the database");
                        var roleResult = await userManager.AddToRoleAsync(user, dto.Role.ToString());
                        if (!roleResult.Succeeded)
                        {
                            logger.LogError($"Adding user: {user.Name} to role failed. Deleting user record...");
                            await userManager.DeleteAsync(user);
                        }
                        else
                        {
                            logger.LogInformation($"Added user: {user.Name} to the {dto.Role.GetDescription()} role");
                        }
                    }
                }
            }
        }
    }
}
