using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using picture_Backend;
using picture_Backend.Data.Context;
using picture_Backend.Models;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
   .AddJsonFile("appsettings.json")
   .Build();

// Add services to the container.
builder.Services.AddSingleton<ImageContext>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
      options.TokenValidationParameters = new TokenValidationParameters
      {
         ValidateIssuer = false,
         ValidateAudience = false,
         //ValidAudience = configuration["JWT:ValidAudience"],
         //ValidIssuer = configuration["JWT:ValidIssuer"],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr"))
      });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection(ConnectionStringOptions.Position));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();   
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
   FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
   RequestPath = "/images"
});

app.UseRouting();
//app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
   endpoints.MapControllers();
});

app.Run();

