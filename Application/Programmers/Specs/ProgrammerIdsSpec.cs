using Domain.Commons;
using Domain.Programmers;
using System.Linq.Expressions;

namespace Application.Programmers.Specs
{
    public class ProgrammerIdsSpec : Specification<Programmer>
    {
        private readonly List<Guid> _ids;
        public ProgrammerIdsSpec(List<Guid> ids)
        {
            _ids = ids;
        }
        public override Expression<Func<Programmer, bool>> ToExpressAll()
        {
            if (_ids.Any() && !_ids.Exists(id => id != Guid.Empty)) return programmer => _ids.Contains(programmer.Id);
            return programmer => true;
        }
    }
}
