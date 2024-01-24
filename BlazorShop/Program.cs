using System;
using System.Text;
using BlazorShop.Models;
using BlazorShop.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

Console.OutputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddSingleton<ICatalog, InMemoryCatalog>();
builder.Services.AddSingleton<IClock, RealClock>();
builder.Services.AddScoped<IEmailSender, MailKitSmtpEmailSender>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("AppDb"));
});
builder.Services.AddScoped<ICatalog, EfCatalog>();

builder.Configuration.AddJsonFile("params.json"); 
// устанавливаем объект EmailMessageAutorizationModel по настройкам из конфигурации
builder.Services.Configure<EmailMessageAutorizationModel>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();

app.Map("/email_sender_api/{receiver_email}/{subject}/{htmlBody}/{senderName}",
    (string receiver_email,
    string subject,
    string htmlBody,
    string senderName,
    IEmailSender sender) => 
{

    // return sender.SendEmailApi(receiver_email, subject, htmlBody, senderName);
    return Task.FromResult( "i'm here");
});
app.MapFallbackToPage("/_Host");

app.Run();

