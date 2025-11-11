using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using KoleosDemo.Repositories;
using KoleosDemo.Entidades;

namespace KoleosDemo.Infrastructure
{
 public static class ServiceExtensions
 {
 public static IServiceCollection AddKoleosServices(this IServiceCollection services, IConfiguration configuration)
 {
 // DbContext
 services.AddDbContext<ApplicationDbContext>(options =>
 options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

 // Identity (requires Identity packages)
 services.AddKoleosIdentity();

 // Blazor & Http
 services.AddRazorPages();
 services.AddServerSideBlazor();
 services.AddScoped(sp => {
 var nav = sp.GetRequiredService<NavigationManager>();
 return new HttpClient { BaseAddress = new Uri(nav.BaseUri) };
 });

 // Authorization
 services.AddAuthorizationCore();

 // Repositories and other infrastructure can be registered here
 services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

 return services;
 }
 }
}
