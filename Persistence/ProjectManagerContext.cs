using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ProjectManagerContext : DbContext
    {
        public ProjectManagerContext(DbContextOptions<ProjectManagerContext> options): base(options)
        {
        }

    }
}
