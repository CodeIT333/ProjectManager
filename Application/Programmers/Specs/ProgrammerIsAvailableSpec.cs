using Domain.Commons;
using Domain.Programmers;
using System.Linq.Expressions;

namespace Application.Programmers.Specs
{
    public class ProgrammerIsAvailableSpec : Specification<Programmer>
    {
        private readonly bool _isAvailable;
        public ProgrammerIsAvailableSpec(bool isAvailable)
        {
            _isAvailable = isAvailable;
        }

        public override Expression<Func<Programmer, bool>> ToExpressAll()
        {
            return programmer => programmer.IsArchived != _isAvailable;
        }
    }
}
