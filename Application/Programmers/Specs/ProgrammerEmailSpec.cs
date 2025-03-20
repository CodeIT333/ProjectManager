using Domain.Commons;
using Domain.Programmers;
using System.Linq.Expressions;

namespace Application.Programmers.Specs
{
    public class ProgrammerEmailSpec : Specification<Programmer>
    {
        private readonly string _email;
        public ProgrammerEmailSpec(string email)
        {
            _email = email;
        }
        public override Expression<Func<Programmer, bool>> ToExpressAll()
        {
            if (!string.IsNullOrWhiteSpace(_email)) return programmer => programmer.Email == _email;
            return programmer => true;
        }
    }
}
