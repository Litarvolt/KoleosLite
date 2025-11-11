using KoleosDemo;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components; // for NavigationManager
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.RegularExpressions;
using KoleosDemo.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services via modular extension
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddKoleosServices(builder.Configuration);

// Note: RevalidatingIdentityAuthenticationStateProvider registration removed to avoid dependency issues.

var app = builder.Build();

// Normalize incoming paths: remove duplicate slashes to avoid '//' issues
app.Use(async (context, next) =>
{
 var path = context.Request.Path.Value ?? string.Empty;
 if (path.Contains("//"))
 {
 var normalizedPath = Regex.Replace(path, "/{2,}", "/");
 var newUrl = normalizedPath + context.Request.QueryString;
 context.Response.Redirect(newUrl, permanent: true);
 return;
 }
 await next();
});

if (app.Environment.IsDevelopment())
{
 app.UseDeveloperExceptionPage();
 app.UseSwagger();
 app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Protect API endpoints by default except user registration/auth
app.MapControllers().RequireAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
