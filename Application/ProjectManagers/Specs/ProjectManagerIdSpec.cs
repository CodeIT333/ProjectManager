using Domain.Commons;
using Domain.ProjectManagers;
using System.Linq.Expressions;

namespace Application.ProjectManagers.Specs
{
    public class ProjectManagerIdSpec : Specification<ProjectManager>
    {
        private readonly Guid _id;
        public ProjectManagerIdSpec(Guid id)
        {
            _id = id;
        }

        public override Expression<Func<ProjectManager, bool>> ToExpressAll()
        {
            if (_id != Guid.Empty) return pm => pm.Id == _id;
            return pm => true;
        }
    }
}
