using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KoleosDemo.Entidades;

namespace KoleosDemo.Infrastructure
{
 public static class IdentityExtensions
 {
 public static IServiceCollection AddKoleosIdentity(this IServiceCollection services)
 {
 services.AddIdentity<ApplicationUser, IdentityRole>(options =>
 {
 options.Password.RequiredLength =6;
 options.Password.RequireLowercase = true;
 options.Password.RequireDigit = true;
 options.Password.RequireNonAlphanumeric = false;
 })
 .AddEntityFrameworkStores<ApplicationDbContext>()
 .AddDefaultTokenProviders();

 services.ConfigureApplicationCookie(options =>
 {
 options.LoginPath = "/login";
 options.ExpireTimeSpan = TimeSpan.FromHours(8);
 });
 return services;
 }
 }
}
