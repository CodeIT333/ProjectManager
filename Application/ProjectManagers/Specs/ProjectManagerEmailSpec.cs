using Domain.Commons;
using Domain.ProjectManagers;

namespace Application.ProjectManagers.Specs
{
    public class ProjectManagerEmailSpec : Specification<ProjectManager>
    {
        private readonly string _email;
        public ProjectManagerEmailSpec(string email)
        {
            _email = email;
        }

        public override System.Linq.Expressions.Expression<Func<ProjectManager, bool>> ToExpressAll()
        {
            if (!string.IsNullOrWhiteSpace(_email)) return pm => pm.Email == _email;
            return pm => true;
        }
    }
}
