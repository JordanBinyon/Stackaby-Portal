using Newtonsoft.Json;

namespace Stackaby.Models.Services;

public class AuthenticatedUser
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    [JsonIgnore]
    public string FullName => $"{FirstName} {LastName}";
}