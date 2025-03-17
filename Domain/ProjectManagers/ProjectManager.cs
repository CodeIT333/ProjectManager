using Domain.Commons;
using Domain.Commons.Models;
using Domain.Programmers;
using Domain.Projects;
using System.ComponentModel.DataAnnotations;

namespace Domain.ProjectManagers
{
    public class ProjectManager : Entity<Guid>
    {
        [Required, MaxLength(200)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required, MaxLength(20)]
        public string Phone { get; set; }
        public Address Address { get; protected set; }
        public DateTime DateOfBirth { get; protected set; }
        public List<Project> Projects { get; protected set; } = [];
        public List<Programmer> Employees { get; protected set; } = [];
    }
}
