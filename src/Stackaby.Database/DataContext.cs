using Microsoft.EntityFrameworkCore;
using Stackaby.Models.Database;

namespace Stackaby.Database;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
}