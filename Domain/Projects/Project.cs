﻿using Domain.Commons.Models;
using Domain.Customers;
using Domain.ProjectManagers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Projects
{
    public class Project : AggregateRoot<Guid>
    {
        public Guid? ProjectManagerId { get; protected set; }
        public ProjectManager? ProjectManager { get; protected set; }
        public List<ProgrammerProject> ProgrammerProjects { get; protected set; } = [];
        public Guid? CustomerId { get; protected set; }
        public Customer? Customer { get; protected set; }
        public DateOnly StartDate { get; protected set; }
        [MaxLength(10000)]
        public string Description { get; protected set; }
        [DefaultValue(false)]
        public bool IsArchived { get; protected set; } = false;

        public static Project Create(
            ProjectManager projectManager,
            Customer customer,
            string description
            )
        {
            return new Project
            {
                Id = Guid.NewGuid(),
                ProjectManager = projectManager,
                ProjectManagerId = projectManager.Id,
                Customer = customer,
                CustomerId = customer.Id,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Description = description
            };
        }

        public void Update(
            ProjectManager? projectManager,
            Customer? customer,
            string description
            )
        {
            SetProjectManager(projectManager);
            Customer = customer;
            CustomerId = customer?.Id;
            if (description != Description) Description = description;   
        }

        public void Delete()
        {
            ProjectManager = null;
            ProjectManagerId = null;
            Customer = null;
            CustomerId = null;
            IsArchived = true;
        }

        public void SetProjectManager(ProjectManager? projectManager)
        {
            ProjectManager = projectManager;
            ProjectManagerId = projectManager?.Id;
        }
    }
}
