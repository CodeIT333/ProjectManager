using Domain.Commons;
using Domain.Programmers;
using System.Linq.Expressions;

namespace Application.Programmers.Specs
{
    public class ProgrammerAvailableSpec : Specification<Programmer>
    {
        private readonly bool _isAvailable;
        public ProgrammerAvailableSpec(bool isAvailable)
        {
            _isAvailable = isAvailable;
        }

        public override Expression<Func<Programmer, bool>> ToExpressAll()
        {
            return programmer => programmer.IsArchived != _isAvailable;
        }
    }
}
