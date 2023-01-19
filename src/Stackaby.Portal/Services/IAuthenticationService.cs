using Stackaby.Models.Services;

namespace Stackaby.Portal.Services;

public interface IAuthenticationService
{
    bool IsAuthenticated { get; }
    AuthenticatedUser? GetAuthenticatedUser();
    Task SignIn(string email, string password, bool isPersistent = false);
    Task SignOut();
}