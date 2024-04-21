using Mongo.Common.MongoDB;
using Serilog;
using Swappa.Server.Configurations;
using Swappa.Server.Extensions;
using Swappa.Server.Filters;

var builder = WebApplication.CreateBuilder(args);
Configurations.ConfigureLogging();
//builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.ConfigureCors();
builder.Services.ConfigureMongoIdentity(builder.Configuration);
builder.Host.UseSerilog();
builder.Services.ConfigureMongoConnection(builder.Configuration);
builder.Services.ConfigureMediatR();
builder.Services.ConfigureController();
builder.Services.ConfigureServices();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureCloudinary(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.AddAutoMapper(typeof(Program));

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
app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
