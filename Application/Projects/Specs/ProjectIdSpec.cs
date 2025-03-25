using Domain.Commons;
using Domain.Projects;
using System.Linq.Expressions;

namespace Application.Projects.Specs
{
    public class ProjectIdSpec : Specification<Project>
    {
        private readonly Guid _id;
        public ProjectIdSpec(Guid id)
        {
            _id = id;
        }

        public override Expression<Func<Project, bool>> ToExpressAll()
        {
            if (_id != Guid.Empty) return p => p.Id == _id;
            return p => true;
        }
    }
}
