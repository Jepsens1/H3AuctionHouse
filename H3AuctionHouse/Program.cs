using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Identity;
using AuctionHouseBackend.Database;
using AuctionHouseBackend.Managers;
using System.Security.Cryptography.X509Certificates;

namespace H3AuctionHouse
{
    public class Program
    {
        public static LoginManager _manager = new LoginManager(new DatabaseLogin("Server=DESKTOP-51IFUJ0\\SQLEXPRESS;Database=AuctionHouse;Trusted_Connection=True;"));
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = "AuctionSession";
            });
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });

            builder.Services.AddAuthorization();

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
            app.UseSession();
            app.Use(async (context, next) =>
            {
                var token = context.Request.Cookies["token"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}