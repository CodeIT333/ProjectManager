using Domain.Commons;
using Domain.Programmers;
using System.Linq.Expressions;

namespace Application.Programmers.Specs
{
    public class ProgrammerIdSpec : Specification<Programmer>
    {
        private readonly Guid _id;
        public ProgrammerIdSpec(Guid id)
        {
            _id = id;
        }
        public override Expression<Func<Programmer, bool>> ToExpressAll()
        {
            if (_id != Guid.Empty) return programmer => programmer.Id == _id;
            return programmer => true;
        }
    }
}
