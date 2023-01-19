namespace Stackaby.Interfaces;

public interface IProjectService
{
    Task Create(string name, string description);
}