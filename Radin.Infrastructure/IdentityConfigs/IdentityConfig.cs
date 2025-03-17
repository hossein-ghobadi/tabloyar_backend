﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radin.Domain.Entities.Users;
using Radin.Persistence.Contexts;
using System;
using System.Threading.Tasks;

namespace Radin.Infrastructure.IdentityConfigs
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            var connection1 = Environment.GetEnvironmentVariable("CONNECTION_RADINGH");

            services.AddDbContext<IdentityDataBaseContext>(options => options.UseSqlServer(connection1));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDataBaseContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<PersianIdentityError>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 1;

                // User settings
                options.User.RequireUniqueEmail = true;

                // Lockout settings
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            });

            // Configure application cookie settings with dynamic Domain
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".AspNetCore.Cookies";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None; // Required for cross-origin
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Secure=true
                options.ExpireTimeSpan = TimeSpan.FromMinutes(1130);
                options.SlidingExpiration = true;

                options.Events = new CookieAuthenticationEvents
                {
                    OnSigningIn = context =>
                    {
                        var origin = context.HttpContext.Request.Headers["Origin"].ToString();

                        if (origin.StartsWith("https://radintablo.com", StringComparison.OrdinalIgnoreCase))
                        {
                            // Set Domain for production frontend
                            context.CookieOptions.Domain = Environment.GetEnvironmentVariable("COOKIE_DOMAIN");
                        }
                        else if (origin.StartsWith("http://localhost:3000", StringComparison.OrdinalIgnoreCase) ||
                                 origin.StartsWith("https://localhost:3000", StringComparison.OrdinalIgnoreCase))
                        {
                            // Do not set Domain for development frontend
                            context.CookieOptions.Domain = null;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            // Configure external login cookie settings with dynamic Domain
            services.ConfigureExternalCookie(options =>
            {
                options.Cookie.Name = ".AspNetCore.ExternalCookies";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None; // Required for cross-origin
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Secure=true

                options.Events = new CookieAuthenticationEvents
                {
                    OnSigningIn = context =>
                    {
                        var origin = context.HttpContext.Request.Headers["Origin"].ToString();

                        if (origin.StartsWith("https://radintablo.com", StringComparison.OrdinalIgnoreCase))
                        {
                            // Set Domain for production frontend
                            context.CookieOptions.Domain = Environment.GetEnvironmentVariable("COOKIE_DOMAIN");
                        }
                        else if (origin.StartsWith("http://localhost:3000", StringComparison.OrdinalIgnoreCase) ||
                                 origin.StartsWith("https://localhost:3000", StringComparison.OrdinalIgnoreCase))
                        {
                            // Do not set Domain for development frontend
                            context.CookieOptions.Domain = null;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
