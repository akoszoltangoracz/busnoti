using BusNoti.Components;
using BusNoti.Handlers;
using BusNoti.State;
using Microsoft.EntityFrameworkCore;
using NATS.Client.Core;
using NATS.Client.Hosting;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// dotnet watch --profile http

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddNats(1, opts => {
    opts = new NatsOpts{
        Url = builder.Configuration.GetValue<string>("natsUrl")!,
    };

    return opts;
});

builder.Services.AddDbContextFactory<BusNoti.Contexts.ChannelContext>(
    opt => opt.UseNpgsql(builder.Configuration.GetValue<string>("pgConnString"))
);

builder.Services.AddHostedService<BusHandler>();
builder.Services.AddRadzenComponents();

builder.Services.AddScoped<AppState>();

builder.Services.AddSingleton<MessageState>();
builder.Services.AddSingleton<BlacklistHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
