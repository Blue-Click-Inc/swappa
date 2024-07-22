using DinkToPdf;
using DinkToPdf.Contracts;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Mongo.Common.MongoDB;
using RedisCache.Common.Repository.Extensions;
using Serilog;
using Swappa.Server.Configurations;
using Swappa.Server.Extensions;
using Swappa.Server.Filters;

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
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

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
builder.Services.ConfigureRedis(builder.Configuration.GetSection("Redis").GetValue<string>("Connection") ?? string.Empty)
    .ConfigureCacheRepository();

builder.Services.ConfigureHangfireClient(builder.Configuration);
builder.Services.ConfigureHangfireServer();

builder.Services.AddHttpContextAccessor();

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
app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter(builder.Configuration) },
});
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
