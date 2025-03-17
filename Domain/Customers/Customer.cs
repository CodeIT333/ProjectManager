using Domain.Commons.Models;
using Domain.Projects;
using System.ComponentModel.DataAnnotations;

namespace Domain.Customers
{
    public class Customer : Entity<Guid>
    {
        [Required, MaxLength(200)]
        public string Name { get; protected set; }
        [Required, MaxLength(100)]
        public string Email { get; protected set; }
        [Required, MaxLength(20)]
        public string Phone { get; protected set; }
        public List<Project> Projects { get; protected set; } = [];
    }
}
