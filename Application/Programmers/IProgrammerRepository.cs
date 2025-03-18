using Domain.Programmers;

namespace Application.Programmers
{
    public interface IProgrammerRepository
    {
        Task<List<Programmer>> ListProgrammersAsync();
        Task<Programmer> GetProgrammerAsync(Guid id);
    }
}
