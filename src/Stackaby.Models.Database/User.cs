using System.ComponentModel.DataAnnotations;

namespace Stackaby.Models.Database;

public class User
{
    [Key]
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
}