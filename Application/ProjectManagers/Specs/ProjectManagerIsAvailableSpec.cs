using Domain.Commons;
using Domain.ProjectManagers;
using System.Linq.Expressions;

namespace Application.ProjectManagers.Specs
{
    public class ProjectManagerIsAvailableSpec : Specification<ProjectManager>
    {
        private readonly bool _isAvailable;
        public ProjectManagerIsAvailableSpec(bool isAvailable)
        {
            _isAvailable = isAvailable;
        }

        public override Expression<Func<ProjectManager, bool>> ToExpressAll()
        {
            return pm => pm.IsArchived != _isAvailable;
        }
    }
}
