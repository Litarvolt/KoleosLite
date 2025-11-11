using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace KoleosDemo.Services;

[System.Obsolete("Use ASP.NET Core Identity authentication provider instead.")]
public class SimpleAuthStateProvider : AuthenticationStateProvider
{
 private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
 private ClaimsPrincipal? _user;

 public override Task<AuthenticationState> GetAuthenticationStateAsync()
 {
 if (_user == null)
 {
 return Task.FromResult(new AuthenticationState(_anonymous));
 }
 return Task.FromResult(new AuthenticationState(_user));
 }

 public void MarkUserAsAuthenticated(string username, string role = "User")
 {
 var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Role, role) }, "apiauth");
 _user = new ClaimsPrincipal(identity);
 NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
 }

 public void MarkUserAsLoggedOut()
 {
 _user = null;
 NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
 }

 public bool IsAuthenticated => _user != null;
}