using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Mongo.Common.MongoDB;
using Serilog;
using Swappa.Server.Configurations;
using Swappa.Server.Extensions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
Configurations.ConfigureLogging();

builder.Services.ConfigureCors();
builder.Services.ConfigureMongoIdentity(builder.Configuration);
builder.Host.UseSerilog();
builder.Services.ConfigureMongoConnection(builder.Configuration);
builder.Services.ConfigureMediatR();
builder.Services.ConfigureServices();
builder.Services.ConfigureMailJet(builder.Configuration);
builder.Services.ConfigureCloudinary(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddApiVersioning(opt =>
{
    opt.ReportApiVersions = true;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
//builder.Services.ConfigureController();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddHttpContextAccessor();

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = false;
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c919b5dc3e1e5b3978d096512479c2d40e795cacbe5052a341e50d67bdbbf40664b36ee8d3a73919d02b23d2fa382fa7e4edc31ef60da269c2b1f77f88912178")),
//        ValidateIssuer = false,
//        ValidateAudience = false,
//    };
//});

var app = builder.Build();
// Configure the HTTP request pipeline.

var logger = app.Services.GetRequiredService<ILogger<Program>>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
