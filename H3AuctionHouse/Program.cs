using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using AuctionHouseBackend.Database;
using AuctionHouseBackend.Managers;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using AuctionHouseBackend.Interfaces;
using AuctionHouseBackend.Models;

namespace H3AuctionHouse
{
    public class Program
    {
        /*patrick*///static string constring = "Server=PJJ-P15S-2022\\SQLEXPRESS;Database=AuctionHouse;Trusted_Connection=True;";
        /*phillip*/ static string constring = "Server=DESKTOP-51IFUJ0\\SQLEXPRESS;Database=AuctionHouse;Trusted_Connection=True;";
        //public static AccountManager _loginManager = new AccountManager(new DatabaseLogin(constring));
        //public static AuctionProductManager _auctionproductmanager = new AuctionProductManager(new DatabaseAuctionProduct(constring));
        //public static IEmailManager emailManager = new SMTPEmailManager();
        public static Manager manager = new Manager(constring);
        
        public static void StartStatusChangedEvent()
        {
            for (int i = 0; i < manager.Get<AuctionProductManager>().Products.Count; i++)
            {
                manager.Get<AuctionProductManager>().Products[i].Product.OnStatusChanged += Product_OnStatusChanged;
            }
           
        }

        public static void StartOnProductCreatedEvent()
        {
            manager.Get<AuctionProductManager>().OnProductCreated += _auctionproductmanager_OnProductCreated;
        }

        private static void _auctionproductmanager_OnProductCreated(object? sender, object e)
        {
            for (int i = 0; i < manager.Get<AuctionProductManager>().Products.Count; i++)
            {
                manager.Get<AuctionProductManager>().Products[i].Product.OnStatusChanged -= Product_OnStatusChanged;
                manager.Get<AuctionProductManager>().Products[i].Product.OnStatusChanged += Product_OnStatusChanged;
            }
        }

        private static void Product_OnStatusChanged(object? sender, object e)
        {
            manager.Get<SMTPEmailManager>().SendMail(((ProductModel<AuctionProductModel>)e).Owner, "subject", "body");
        }

        public static void Main(string[] args)
        {
            StartStatusChangedEvent();
            StartOnProductCreatedEvent();
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddScoped<IInputSanitizer, InputSanitizer>();
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
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