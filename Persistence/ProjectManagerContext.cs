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

        // for runtime config
        public ProjectManagerContext(DbContextOptions<ProjectManagerContext> options, IConfiguration configuration): base(options)
        {
            _configuration = configuration;
        }

        // for migrations
        public ProjectManagerContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionStr = _configuration.GetConnectionString("DefaultConnection") ?? 
                    "Server=localhost;Database=ProjectManagerDB;Trusted_Connection=True;TrustServerCertificate=True;";
                optionsBuilder.UseSqlServer(connectionStr);
            }
        }

        public DbSet<Programmer> Programmers { get; set; }
        public DbSet<ProjectManager> ProjectManagers { get; set; }
        public DbSet<ProgrammerProject> ProgrammerProjects { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetEntityKeys(modelBuilder);
            ConfigureEntities(modelBuilder);

            
        }

        private void SetEntityKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Programmer>().HasKey(p => p.Id);
            modelBuilder.Entity<ProjectManager>().HasKey(pm => pm.Id);
            modelBuilder.Entity<ProgrammerProject>().HasKey(pp => new { pp.ProgrammerId, pp.ProjectId });
            modelBuilder.Entity<Project>().HasKey(p => p.Id);
            modelBuilder.Entity<Customer>().HasKey(c => c.Id);
        }

        private void ConfigureEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Programmer>().OwnsOne(p => p.Address);

            modelBuilder.Entity<ProjectManager>().OwnsOne(pm => pm.Address);

            // programmer [1] - [Many] project
            modelBuilder.Entity<ProgrammerProject>()
                .HasOne(pp => pp.Programmer)
                .WithMany(p => p.ProgrammerProjects)
                .HasForeignKey(pp => pp.ProgrammerId);

            modelBuilder.Entity<ProgrammerProject>()
                .HasOne(pp => pp.Project)
                .WithMany(p => p.ProgrammerProjects)
                .HasForeignKey(pp => pp.ProjectId);

            // customer [Many] - [1] project
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.Projects)
                .HasForeignKey(cp => cp.CustomerId);

            // pm [1] - [Many] project
            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectManager)
                .WithMany(pm => pm.Projects)
                .HasForeignKey(pm => pm.ProjectManagerId);

            // pm [0/1] - [Many] programmer
            modelBuilder.Entity<Programmer>()
                .HasOne<ProjectManager>()
                .WithMany(pm => pm.Employees)
                .HasForeignKey(p => p.ProjectManagerId)
                .IsRequired(false);
        }
    }
}
