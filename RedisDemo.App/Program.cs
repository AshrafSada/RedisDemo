using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RedisDemo.App.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        _ = builder.Services.AddRazorPages();
        _ = builder.Services.AddServerSideBlazor();
        _ = builder.Services.AddSingleton<WeatherForecastService>();
        _ = builder.Services.AddStackExchangeRedisCache(options =>
        {
            // to use the same database cache for more than one App
            options.InstanceName = "RedisDemo_";
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            _ = app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            _ = app.UseHsts();
        }

        _ = app.UseHttpsRedirection();

        _ = app.UseStaticFiles();

        _ = app.UseRouting();

        _ = app.MapBlazorHub();
        _ = app.MapFallbackToPage("/_Host");

        app.Run();
    }
}