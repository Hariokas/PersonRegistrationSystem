using Repository.Extensions;
using Repository.Interfaces;
using Services.Interfaces;
using Shared;
using Shared.DTOs.Residence;
using Shared.Enums;

namespace Services;

public class ResidenceService(IPersonRepository personRepository, IResidenceRepository residenceRepository) : IResidenceService
{
    public async Task AddResidenceAsync(Guid userId, ResidenceCreateDto residenceCreateDto)
    {
        var person = await personRepository.GetPersonByIdAsync(residenceCreateDto.PersonId) ??
                     throw new PersonNotFoundException("Person not found");

        if (person.User.Id != userId)
            throw new UnauthorizedAccessException("You are not authorized to add residenceCreateDto to this person");

        var residence = residenceCreateDto.ToResidence(person);

        await residenceRepository.AddResidenceAsync(residence);
    }

    public async Task<ResidenceDto?> GetResidenceByIdAsync(Guid userId, Role userRole, Guid residenceId)
    {
        var residence = await residenceRepository.GetResidenceByIdAsync(residenceId) ??
                        throw new ResidenceNotFoundException("Residence not found");

        if (residence.Person.User.Id != userId)
            throw new UnauthorizedAccessException("You are not authorized to get this residence");

        var residenceDto = residence.ToResidenceDto();

        return residenceDto;
    }

    public async Task<IEnumerable<ResidenceDto>> GetResidenceByPersonIdAsync(Guid userId, Role userRole, Guid personId)
    {
        var person = await personRepository.GetPersonByIdAsync(personId) ??
                     throw new PersonNotFoundException("Person not found");

        if (person.User.Id != userId)
            throw new UnauthorizedAccessException("You are not authorized to get residences of this person");

        var residences = await residenceRepository.GetResidenceByPersonIdAsync(personId);

        var residenceDtos = residences.ToList().Select(residence => residence.ToResidenceDto());

        return residenceDtos;
    }

    public async Task UpdateResidenceAsync(Guid userId, Role userRole, ResidenceUpdateDto residenceUpdateDto)
    {
        var residenceToUpdate = await residenceRepository.GetResidenceByIdAsync(residenceUpdateDto.Id) ??
                                throw new ResidenceNotFoundException("Residence not found");

        if (residenceToUpdate.Person.User.Id != userId)
            throw new UnauthorizedAccessException("You are not authorized to update this residence");

        residenceToUpdate.UpdateWithDto(residenceUpdateDto);

        await residenceRepository.UpdateResidenceAsync(residenceToUpdate);
    }

    public async Task DeleteResidenceAsync(Guid userId, Role userRole, Guid residenceId)
    {
        var residence = await residenceRepository.GetResidenceByIdAsync(residenceId) ??
                        throw new ResidenceNotFoundException("Residence not found");

        if (residence.Person.User.Id != userId && userRole != Role.Admin)
            throw new UnauthorizedAccessException("You are not authorized to delete this residence");

        await residenceRepository.DeleteResidenceAsync(residence);
    }
}