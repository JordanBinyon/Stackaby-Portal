using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using Stackaby.Interfaces;
using Stackaby.Models.Portal.User;
using Stackaby.Models.Services;

namespace Stackaby.Portal.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpContext? _httpContext;
    private readonly IUserService _userService;

    public AuthenticationService(IHttpContextAccessor httpContextAccessor, IUserService userService)
    {
        _userService = userService;
        _httpContext= httpContextAccessor.HttpContext;
    }

    public bool IsAuthenticated => _httpContext?.User.Identity?.IsAuthenticated == true;
    
    public AuthenticatedUser? GetAuthenticatedUser()
    {
        if (_httpContext == null)
        {
            throw new Exception("HttpContext is null");
        }

        if (!IsAuthenticated)
        {
            throw new Exception("User is not authenticated");
        }

        var serialisedUserDetails = _httpContext.User.Claims
            .FirstOrDefault(x => x.Type == AuthenticatedUserClaimTypes.SerialisedUserClaim);

        if (string.IsNullOrWhiteSpace(serialisedUserDetails?.Value))
        {
            throw new Exception("User claim containing details is empty");
        }

        return JsonConvert.DeserializeObject<AuthenticatedUser>(serialisedUserDetails.Value);
    }

    public async Task SignIn(string email, string password, bool isPersistent = false)
    {
        if (_httpContext == null)
        {
            throw new Exception("HttpContext is null");
        }

        var authenticateUserDetails = await _userService.Authenticate(email, password);

        if (authenticateUserDetails == null)
        {
            // TODO Make this more user friendly
            throw new Exception("Invalid user details");
        }

        var identity = new ClaimsIdentity(GetUserClaims(authenticateUserDetails), CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }

    public async Task SignOut()
    {
        if (_httpContext == null)
        {
            throw new Exception("HttpContext is null");
        }
        
        await _httpContext.SignOutAsync();
    }

    private static IEnumerable<Claim> GetUserClaims(AuthenticatedUser authenticatedUser)
    {
        var claims = new List<Claim> { new(AuthenticatedUserClaimTypes.SerialisedUserClaim, JsonConvert.SerializeObject(authenticatedUser)) };

        return claims;
    }
}