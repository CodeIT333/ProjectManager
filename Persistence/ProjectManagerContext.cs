using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence
{
    public class ProjectManagerContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ProjectManagerContext(DbContextOptions<ProjectManagerContext> options, IConfiguration configuration): base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
    }
}
