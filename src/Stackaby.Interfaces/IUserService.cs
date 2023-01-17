using Stackaby.Models.Services;

namespace Stackaby.Interfaces;

public interface IUserService
{
    Task Register(string firstName, string lastName, string email, string password);
    Task<AuthenticatedUser?> Authenticate(string email, string password);
}