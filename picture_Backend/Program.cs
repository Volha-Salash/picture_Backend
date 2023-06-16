using picture_Backend;
using picture_Backend.Data.Context;
using picture_Backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ImageContext>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection(ConnectionStringOptions.Position));
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
   app.UseStaticFiles();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
