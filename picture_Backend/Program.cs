using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using picture_Backend;
using picture_Backend.Data.Context;
using picture_Backend.Models;
using picture_Backend.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
   .AddJsonFile("appsettings.json", true, true)
   .Build();

// Add services to the container.
builder.Services.AddSingleton<ImageContext>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
   .AddCookie(options =>
   {
      options.Cookie.Name = "YourCookieName"; // Название куки
      options.Cookie.HttpOnly = true; // Запретить доступ к кукам из JavaScript
      options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Время жизни куки
      options.LoginPath = "/user/login"; // Путь к странице входа
   });
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection(ConnectionStringOptions.Position));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();   
   app.UseCors(x => x
      .AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader());
   
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
   FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
   RequestPath = "/images"
});



   
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
   endpoints.MapControllers();
});

app.Run();