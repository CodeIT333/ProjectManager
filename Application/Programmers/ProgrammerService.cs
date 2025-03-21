using Application.Commons;
using Application.Programmers.DTOs;
using Application.Programmers.Specs;
using Application.ProjectManagers;
using Application.ProjectManagers.Specs;
using Domain.Commons;
using Domain.Programmers;
using Domain.ProjectManagers;
using Infrastructure.Exceptions;
using Mapster;

namespace Application.Programmers
{
    public class ProgrammerService
    {
        private readonly IProjectManagerRepository _projectManagerRepo;
        private readonly IProgrammerRepository _programmerRepo;
        private readonly IUnitOfWork _uow;

        public ProgrammerService(
            IProjectManagerRepository projectManagerRepo,
            IProgrammerRepository programmerRepo,
            IUnitOfWork uow
            )
        {
            _projectManagerRepo = projectManagerRepo;
            _programmerRepo = programmerRepo;
            _uow = uow;
        }

        public async Task<List<ProgrammerListDTO>> ListProgrammersAsync()
        {
            var programmers = await _programmerRepo.ListProgrammersAsync();
            return programmers.Adapt<List<ProgrammerListDTO>>();
        }

        public async Task<ProgrammerGetDTO> GetProgrammerAsync(Guid id)
        {
            var programmer = await _programmerRepo.GetProgrammerAsync(new ProgrammerIdSpec(id));
            if (programmer is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROGRAMMER);

            return programmer.Adapt<ProgrammerGetDTO>();
        }

        public async Task CreateProgrammerAsync(ProgrammerCreateUpdateDTO dto)
        {
            var existingProgrammer = await _programmerRepo.GetProgrammerAsync(new ProgrammerEmailSpec(dto.email));
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

            ProjectManager programmerProjectManager = null;
            if (dto.projectManagerId is not null && dto.projectManagerId.HasValue && dto.projectManagerId.Value != Guid.Empty)
            {
                programmerProjectManager = await _projectManagerRepo.GetProjectManagerAsync(new ProjectManagerIdSpec(dto.projectManagerId.Value));
                if (programmerProjectManager is null)
                    throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);
            }

            var programmer = Programmer.Create(dto.name, dto.email, dto.phone, programmerAddress, dto.dateOfBirth, dto.role, dto.isIntern, programmerProjectManager);

            if (programmerProjectManager is not null) programmerProjectManager.Employees.Add(programmer);

            await _programmerRepo.CreateProgrammerAsync(programmer);
            await _uow.CommitAsync();
        }

        public async Task UpdateProgrammerAsync(Guid id, ProgrammerCreateUpdateDTO dto)
        {
            var programmer = await _programmerRepo.GetProgrammerAsync(new ProgrammerIdSpec(id));
            if (programmer is null)
                throw new NotFoundException(ErrorMessages.NOT_FOUND_PROGRAMMER);

            if (dto.email != programmer.Email)
            {
                var ProgrammerWithExistingEmail = await _programmerRepo.GetProgrammerAsync(new ProgrammerEmailSpec(dto.email));
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

            ProjectManager programmerProjectManager = null;
            if (dto.projectManagerId is not null && dto.projectManagerId.HasValue && dto.projectManagerId.Value != Guid.Empty)
            {
                programmerProjectManager = await _projectManagerRepo.GetProjectManagerAsync(new ProjectManagerIdSpec(dto.projectManagerId.Value));
                if (programmerProjectManager is null)
                    throw new NotFoundException(ErrorMessages.NOT_FOUND_PROJECT_MANAGER);
            }

            programmer.Update(
                dto.name, 
                dto.email, 
                dto.phone, 
                dto.dateOfBirth, 
                dto.role, 
                dto.isIntern, 
                programmerProjectManager);

            if (programmerProjectManager is not null)
                programmerProjectManager.Employees.Add(programmer);

            await _uow.CommitAsync();
        }
    }
}
