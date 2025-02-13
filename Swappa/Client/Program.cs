using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;
using Swappa.Client;
using Swappa.Client.Extensions;
using Swappa.Client.State;
using Swappa.Shared.Interface;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/v1/") });
builder.Services.AddBlazorBootstrap();
builder.Services.ConfigureInterceptor();
builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredLocalStorage();
builder.Services.ConfigureServices();
builder.Services
    .AddRefitClient<IExternalLocation>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://locations-marker.onrender.com/api/v1/location"));
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();