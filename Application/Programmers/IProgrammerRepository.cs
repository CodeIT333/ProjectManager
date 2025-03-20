using Domain.Commons;
using Domain.Programmers;

namespace Application.Programmers
{
    public interface IProgrammerRepository
    {
        Task<List<Programmer>> ListProgrammersAsync(Specification<Programmer> spec = null);
        Task<Programmer?> GetProgrammerAsync(Specification<Programmer> spec);
        Task CreateProgrammerAsync(Programmer programmer);
    }
}
