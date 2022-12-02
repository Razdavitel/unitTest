global using Microsoft.AspNetCore.Components.Authorization;
global using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using Squads.Client.Extensions;
using Squads.Client;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddSquadHttpClient(builder.HostEnvironment);
builder.Services.AddApiServices();
builder.Services.AddMudServices();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<MudBlazor.DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

await builder.Build().RunAsync();
