using Domain.Commons;
using Domain.ProjectManagers;
using System.Linq.Expressions;

namespace Application.ProjectManagers.Specs
{
    public class ProjectManagerEmployeeIdSpec : Specification<ProjectManager>
    {
        private readonly Guid _employeeId;
        public ProjectManagerEmployeeIdSpec(Guid employeeId)
        {
            _employeeId = employeeId;
        }

        public override Expression<Func<ProjectManager, bool>> ToExpressAll()
        {
            if (_employeeId != Guid.Empty) return pm => pm.Employees.Any(e => e.Id == _employeeId);
            return pm => true;
        }
    }
}
