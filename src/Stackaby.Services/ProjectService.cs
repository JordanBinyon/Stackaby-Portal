using Stackaby.Database;
using Stackaby.Interfaces;
using Stackaby.Models.Database;

namespace Stackaby.Services;

public class ProjectService : IProjectService
{
    private readonly DataContext _dataContext;

    public ProjectService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task Create(string name, string description)
    {
        var project = new Project()
        {
            Name = name,
            Description = description
        };

        _dataContext.Projects.Add(project);

        await _dataContext.SaveChangesAsync();
    }
}