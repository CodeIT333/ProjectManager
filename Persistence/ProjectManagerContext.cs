using Domain.Commons;
using Domain.Customers;
using Domain.Programmers;
using Domain.ProjectManagers;
using Domain.Projects;
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

        public DbSet<Programmer> Programmers { get; set; }
        public DbSet<ProjectManager> ProjectManagers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO set up address entity correctly
            modelBuilder.Entity<Address>().HasNoKey();
        }
    }
}
