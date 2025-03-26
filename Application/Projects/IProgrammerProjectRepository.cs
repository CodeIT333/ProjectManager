using Domain.Projects;

namespace Application.Projects
{
    public interface IProgrammerProjectRepository
    {
        Task CreateProgrammerProjectAsync(ProgrammerProject programmerProject);
        void DeleteProgrammerProject(ProgrammerProject programmerProject);
    }
}
