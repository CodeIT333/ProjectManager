using Domain.Commons;
using Domain.Projects;
using System.Linq.Expressions;

namespace Application.Projects.Specs
{
    public class ProjectIsAvailableSpec : Specification<Project>
    {
        private readonly bool _isAvailable;
        public ProjectIsAvailableSpec(bool isAvailable)
        {
            _isAvailable = isAvailable;
        }

        public override Expression<Func<Project, bool>> ToExpressAll()
        {
            return p => p.IsArchived != _isAvailable;
        }
    }
}
