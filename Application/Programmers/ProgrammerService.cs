using Application.Commons;
using Application.Programmers.DTOs;
using Application.Programmers.Specs;
using Application.ProjectManagers;
using Application.ProjectManagers.Specs;
using Application.Projects;
using Domain.Commons;
using Domain.Programmers;
using Domain.ProjectManagers;
using Infrastructure.Exceptions;
using Mapster;

namespace Application.Programmers
{
    public class ProgrammerService
    {
        private readonly IProgrammerProjectRepository _programmerProjectRepo;
        private readonly IProjectManagerRepository _projectManagerRepo;
        private readonly IProgrammerRepository _programmerRepo;
        private readonly IUnitOfWork _uow;

        public ProgrammerService(
            IProgrammerProjectRepository programmerProjectRepo,
            IProjectManagerRepository projectManagerRepo,
            IProgrammerRepository programmerRepo,
            IUnitOfWork uow
            )
        {
            _programmerProjectRepo = programmerProjectRepo;
            _projectManagerRepo = projectManagerRepo;
            _programmerRepo = programmerRepo;
            _uow = uow;
        }

        public async Task<List<ProgrammerListDTO>> ListProgrammersAsync(bool isAvaiable)
        {
            var programmers = await _programmerRepo.ListProgrammersAsync(new ProgrammerIsAvailableSpec(isAvaiable));
            return programmers.Adapt<List<ProgrammerListDTO>>();
        }

        public async Task<ProgrammerGetDTO> GetProgrammerAsync(Guid id)
        {
            var programmer = await _programmerRepo.GetProgrammerAsync(new ProgrammerIdSpec(id).And(new ProgrammerIsAvailableSpec(true)));
            if (programmer is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROGRAMMER);

            return programmer.Adapt<ProgrammerGetDTO>();
        }

        public async Task CreateProgrammerAsync(ProgrammerCreateUpdateDTO dto)
        {
            var existingProgrammer = await _programmerRepo.GetProgrammerAsync(new ProgrammerEmailSpec(dto.email).And(new ProgrammerIsAvailableSpec(true)));
            if (existingProgrammer is not null)
                throw new BadRequestException(ErrorMessages.TAKEN_PROGRAMMER_EMAIL);

            var programmerAddress = Address.Create(
                dto.address.country, 
                dto.address.zipCode, 
                dto.address.county, 
                dto.address.settlement, 
                dto.address.street, 
                dto.address.houseNumber, 
                dto.address.door
            );

            ProjectManager? programmerProjectManager = null;
            if (dto.projectManagerId is not null && dto.projectManagerId.HasValue && dto.projectManagerId.Value != Guid.Empty)
            {
                programmerProjectManager = await _projectManagerRepo.GetProjectManagerAsync(
                    new ProjectManagerIdSpec(dto.projectManagerId.Value).And(new ProjectManagerIsAvailableSpec(true)));
                if (programmerProjectManager is null)
                    throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);
            }

            var programmer = Programmer.Create(dto.name, dto.email, dto.phone, programmerAddress, dto.dateOfBirth, dto.role, dto.isIntern, programmerProjectManager);

            if (programmerProjectManager is not null) 
                programmerProjectManager.Employees.Add(programmer);

            await _programmerRepo.CreateProgrammerAsync(programmer);
            await _uow.CommitAsync();
        }

        public async Task UpdateProgrammerAsync(Guid id, ProgrammerCreateUpdateDTO dto)
        {
            var programmer = await _programmerRepo.GetProgrammerAsync(new ProgrammerIdSpec(id).And(new ProgrammerIsAvailableSpec(true)));
            if (programmer is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROGRAMMER);

            if (dto.email != programmer.Email)
            {
                var ProgrammerWithExistingEmail = await _programmerRepo.GetProgrammerAsync(new ProgrammerEmailSpec(dto.email).And(new ProgrammerIsAvailableSpec(true)));
                if (ProgrammerWithExistingEmail is not null)
                    throw new BadRequestException(ErrorMessages.TAKEN_PROGRAMMER_EMAIL);
            }

            programmer.Address.Update(
                dto.address.country,
                dto.address.zipCode,
                dto.address.county,
                dto.address.settlement,
                dto.address.street,
                dto.address.houseNumber,
                dto.address.door
            );

            // check if the added pm id is valid
            ProjectManager? newProgrammerProjectManager = null;
            if (dto.projectManagerId is not null && dto.projectManagerId.HasValue && dto.projectManagerId.Value != Guid.Empty)
            {
                newProgrammerProjectManager = await _projectManagerRepo.GetProjectManagerAsync(
                    new ProjectManagerIdSpec(dto.projectManagerId.Value).And(new ProjectManagerIsAvailableSpec(true)));
                if (newProgrammerProjectManager is null)
                    throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);
            }

            // if the new is not the same as the previous pm, remove the programmer from the previous pm employee list
            if (newProgrammerProjectManager is not null && programmer.ProjectManagerId != newProgrammerProjectManager.Id)
            {
                if (programmer.ProjectManager is not null)
                    programmer.ProjectManager.Employees.Remove(programmer);

                newProgrammerProjectManager.Employees.Add(programmer);
            }

            programmer.Update(
                dto.name, 
                dto.email, 
                dto.phone, 
                dto.dateOfBirth, 
                dto.role, 
                dto.isIntern, 
                newProgrammerProjectManager);

            await _uow.CommitAsync();
        }

        public async Task DeleteProgrammerAsync(Guid id)
        {
            var programmer = await _programmerRepo.GetProgrammerAsync(new ProgrammerIdSpec(id).And(new ProgrammerIsAvailableSpec(true)));
            if (programmer is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROGRAMMER);

            if (programmer.ProjectManager is not null) 
                programmer.ProjectManager.Employees.Remove(programmer);

            if (programmer.ProgrammerProjects.Any())
            {
                foreach (var programmerProject in programmer.ProgrammerProjects.ToList())
                {

                    programmerProject.Project.ProgrammerProjects.Remove(programmerProject);
                    programmer.ProgrammerProjects.Remove(programmerProject);

                    _programmerProjectRepo.DeleteProgrammerProject(programmerProject);
                }
            }

            programmer.Delete();

            await _uow.CommitAsync();
        }
    }
}
