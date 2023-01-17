using Microsoft.EntityFrameworkCore;
using Stackaby.Database;
using Stackaby.Helper;
using Stackaby.Interfaces;
using Stackaby.Models.Database;
using Stackaby.Models.Services;

namespace Stackaby.Services;

public class UserService : IUserService
{
    private readonly DataContext _dataContext;
    
    public UserService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task Register(string firstName, string lastName, string email, string password)
    {
        var hashedPassword = PasswordHelper.HashPassword(password);

        var user = new User()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = hashedPassword,
            Created = DateTimeOffset.Now
        };

        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<AuthenticatedUser?> Authenticate(string email, string password)
    {
        var user = await _dataContext.Users
            .Where(x => x.Email == email)
            .Select(x => new
            {
                x.Id,
                x.FirstName,
                x.LastName,
                x.Email,
                x.Password
            })
            .SingleOrDefaultAsync();

        if (user == null || PasswordHelper.VerifyPassword(password, user.Password))
        {
            return null;
        }

        return new AuthenticatedUser()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }
}