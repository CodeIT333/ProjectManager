﻿using Application.Commons;
using Application.Programmers;
using Application.Programmers.Specs;
using Application.ProjectManagers.DTOs;
using Application.ProjectManagers.Specs;
using Application.Projects;
using Application.Projects.Specs;
using Domain.Commons;
using Domain.Programmers;
using Domain.ProjectManagers;
using Domain.Projects;
using Infrastructure.Exceptions;
using Mapster;

namespace Application.ProjectManagers
{
    public class ProjectManagerService
    {
        private readonly IProjectManagerRepository _projectManagerRepo;
        private readonly IProgrammerRepository _programmerRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly IUnitOfWork _uow;

        public ProjectManagerService(
            IProjectManagerRepository projectManagerRepo,
            IProgrammerRepository programmerRepo,
            IProjectRepository projectRepo,
            IUnitOfWork uow
            )
        {
            _projectManagerRepo = projectManagerRepo;
            _programmerRepo = programmerRepo;
            _projectRepo = projectRepo;
            _uow = uow;
        }

        public async Task<List<ProjectManagerListDTO>> ListProjectManagersAsync(bool isAvaiable)
        {
            var projectManagers = await _projectManagerRepo.ListProjectManagersAsync(new ProjectManagerIsAvailableSpec(isAvaiable));
            return projectManagers.Adapt<List<ProjectManagerListDTO>>();
        }

        public async Task<ProjectManagerGetDTO> GetProjectManagerAsync(Guid id)
        {
            var projectManager = await _projectManagerRepo.GetProjectManagerAsync(new ProjectManagerIdSpec(id).And(new ProjectManagerIsAvailableSpec(true)));
            if (projectManager is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);
            
            return projectManager.Adapt<ProjectManagerGetDTO>();
        }

        public async Task CreateProjectManagerAsync(ProjectManagerCreateDTO dto)
        {
            var existingProjectManager = await _projectManagerRepo.GetProjectManagerAsync(new ProjectManagerEmailSpec(dto.email).And(new ProjectManagerIsAvailableSpec(true)));
            if (existingProjectManager is not null)
                throw new BadRequestException(ErrorMessages.TAKEN_PROJECT_MANAGER_EMAIL);

            var address = Address.Create(
                dto.address.country,
                dto.address.zipCode,
                dto.address.county,
                dto.address.settlement,
                dto.address.street,
                dto.address.houseNumber,
                dto.address.door
            );

            List<Programmer> programmers = new();
            if (dto.employeeIds is not null && dto.employeeIds.Any())
            {
                foreach (var employeeId in dto.employeeIds)
                {
                    var programmer = await _programmerRepo.GetProgrammerAsync(new ProgrammerIdSpec(employeeId).And(new ProgrammerIsAvailableSpec(true)));
                    if (programmer is null)
                        throw new NotFoundException(ErrorMessages.NOT_FOUND_PROGRAMMER);

                    programmers.Add(programmer);
                }
            }

            var projectManager = ProjectManager.Create(
                dto.name,
                dto.email,
                dto.phone,
                address,
                dto.dateOfBirth,
                programmers
            );

            if (programmers.Any())
                programmers.ForEach(p => p.SetProjectManager(projectManager));

            await _projectManagerRepo.CreateProjectManagerAsync(projectManager);
            await _uow.CommitAsync();
        }

        public async Task UpdateProjectManagerAsync(Guid id, ProjectManagerUpdateDTO dto)
        {
            var projectManager = await _projectManagerRepo.GetProjectManagerAsync(new ProjectManagerIdSpec(id).And(new ProjectManagerIsAvailableSpec(true)));
            if (projectManager is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);

            projectManager.Address.Update(
                dto.address.country,
                dto.address.zipCode,
                dto.address.county,
                dto.address.settlement,
                dto.address.street,
                dto.address.houseNumber,
                dto.address.door
            );

            List<Project> projects = new();
            if (dto.projectIds is not null && dto.projectIds.Any())
            {
                foreach (var projectId in dto.projectIds)
                {
                    var project = await _projectRepo.GetProjectAsync(new ProjectIdSpec(projectId).And(new ProjectIsAvailableSpec(true)));
                    if (project is null)
                        throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT);

                    projects.Add(project);
                }
            }
            if (projects.Any())
            {
                var projectsToAdd = projects.Except(projectManager.Projects).ToList();
                projectsToAdd.ForEach(p => p.SetProjectManager(projectManager));
                projectManager.Projects.AddRange(projectsToAdd);

                var projectsToRemove = projectManager.Projects.Except(projects).ToList();
                projectsToRemove.ForEach(p => p.SetProjectManager(null));
                projectManager.Projects.RemoveAll(p => projectsToRemove.Contains(p));
            }
            else
            {
                projectManager.Projects.ForEach(p => p.SetProjectManager(null));
                projectManager.Projects.Clear();
            }

            List<Programmer> programmers = new();
            if (dto.employeeIds is not null && dto.employeeIds.Any())
            {
                foreach (var employeeId in dto.employeeIds)
                {
                    var programmer = await _programmerRepo.GetProgrammerAsync(new ProgrammerIdSpec(employeeId).And(new ProgrammerIsAvailableSpec(true)));
                    if (programmer is null)
                        throw new NotFoundException(ErrorMessages.NOT_FOUND_PROGRAMMER);

                    programmers.Add(programmer);
                }
            }
            if (programmers.Any())
            {
                var programmersToAdd = programmers.Except(projectManager.Employees).ToList();
                programmersToAdd.ForEach(p => p.SetProjectManager(projectManager));
                projectManager.Employees.AddRange(programmersToAdd);

                var programmersToRemove = projectManager.Employees.Except(programmers).ToList();
                projectManager.Employees.RemoveAll(p => programmersToRemove.Contains(p));   
                programmersToRemove.ForEach(p => p.SetProjectManager(null));
            }
            else
            {
                projectManager.Employees.ForEach(p => p.SetProjectManager(null));
                projectManager.Employees.Clear();
            }

            projectManager.Update(
                dto.name,
                dto.email,
                dto.phone,
                dto.dateOfBirth,
                programmers,
                projects
            );

            await _uow.CommitAsync();
        }

        public async Task DeleteProjectManagerAsync(Guid id)
        {
            var projectManager = await _projectManagerRepo.GetProjectManagerAsync(new ProjectManagerIdSpec(id).And(new ProjectManagerIsAvailableSpec(true)));
            if (projectManager is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);

            if (projectManager.Projects.Any())
                throw new BadRequestException(ErrorMessages.EXISTING_PROJECT_FOR_PROJECT_MANAGER);

            if (projectManager.Employees.Any())
                projectManager.Employees.ForEach(p => p.RemoveProjectManager());

            projectManager.Delete();

            await _uow.CommitAsync();
        }
    }
}
