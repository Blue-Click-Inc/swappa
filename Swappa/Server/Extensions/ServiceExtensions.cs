using Mailjet.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swappa.Data.Configurations;
using Swappa.Data.Contracts;
using Swappa.Data.Implementations;
using Swappa.Data.Services;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Models;
using Swappa.Shared.DTOs;
using System.Text;

namespace Swappa.Server.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureMailJet(this IServiceCollection services, IConfiguration configuration) =>
            services.AddHttpClient<IMailjetClient, MailjetClient>(client =>
            {
                var settings = configuration.GetSection("MailJet");
                var apiKey = settings["Key"];
                var apiSecret = settings["Secret"];
                client.UseBasicAuthentication(apiKey, apiSecret);
            });

        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.WithOrigins("https://localhost:7027")
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

        public static void ConfigureMongoIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseName = configuration.GetValue<string>("MongoDbSettings:DatabaseName");
            var connectionStr = configuration.GetValue<string>("MongoDbSettings:ConnectionString");
            services.AddIdentity<AppUser, AppRole>()
                .AddMongoDbStores<AppUser, AppRole, Guid>(connectionStr, databaseName)
                .AddDefaultTokenProviders();
        }

        public static void ConfigureCloudinary(this IServiceCollection services, IConfiguration configuration) =>
            services.Configure<CloudinarySettings>(configuration.GetSection(nameof(CloudinarySettings)));

        public static void ConfigureController(this IServiceCollection services) =>
            services
                .AddControllers(config =>
                {
                    config.RespectBrowserAcceptHeader = true;
                    config.ReturnHttpNotAcceptable = true;
                })
                .AddNewtonsoftJson(x =>
                    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(x =>
                    x.SerializerSettings.ContractResolver = new DefaultContractResolver());

        public static void ConfigureSwagger(this IServiceCollection services) =>
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Swappa Api",
                    Version = "v1.0",
                    Description = "Swappa API by Ojo Toba Rufus",
                    Contact = new OpenApiContact
                    {
                        Name = "Toba R. Ojo",
                        Email = "ojotobar@gmail.com",
                        Url = new Uri("https://ojotobar.netlify.app")
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Swappa Api"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<ApiResponseDto>();
            services.AddScoped<INotify, Notify>();
            services.AddScoped<IMedia, Media>();
        }

        public static void ConfigureMediatR(this IServiceCollection services) =>
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(Program).Assembly));

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Key"];
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
                };
            });
        }
    }
}
