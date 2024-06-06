using Repository.Interfaces;
using Repository.Models;
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

        var residence = new Residence
        {
            City = residenceCreateDto.City,
            HouseNumber = residenceCreateDto.HouseNumber,
            Street = residenceCreateDto.Street,
            ApartmentNumber = residenceCreateDto.ApartmentNumber,
            Person = person,
            PersonId = person.Id
        };

        await residenceRepository.AddResidenceAsync(residence);
    }

    public async Task<ResidenceDto?> GetResidenceByIdAsync(Guid userId, Role userRole, Guid residenceId)
    {
        var residence = await residenceRepository.GetResidenceByIdAsync(residenceId) ??
                        throw new ResidenceNotFoundException("Residence not found");

        if (residence.Person.User.Id != userId)
            throw new UnauthorizedAccessException("You are not authorized to get this residence");

        return new ResidenceDto
        {
            Id = residence.Id,
            PersonId = residence.PersonId,
            City = residence.City,
            HouseNumber = residence.HouseNumber,
            Street = residence.Street,
            ApartmentNumber = residence.ApartmentNumber
        };
    }

    public async Task<IEnumerable<ResidenceDto>> GetResidenceByPersonIdAsync(Guid userId, Role userRole, Guid personId)
    {
        var person = await personRepository.GetPersonByIdAsync(personId) ??
                     throw new PersonNotFoundException("Person not found");

        if (person.User.Id != userId)
            throw new UnauthorizedAccessException("You are not authorized to get residences of this person");

        var residences = await residenceRepository.GetResidenceByPersonIdAsync(personId);

        return residences.Select(residence => new ResidenceDto
        {
            Id = residence.Id,
            PersonId = residence.PersonId,
            City = residence.City,
            HouseNumber = residence.HouseNumber,
            Street = residence.Street,
            ApartmentNumber = residence.ApartmentNumber
        });
    }

    public async Task UpdateResidenceAsync(Guid userId, Role userRole, ResidenceUpdateDto residence)
    {
        var residenceToUpdate = await residenceRepository.GetResidenceByIdAsync(residence.Id) ??
                                throw new ResidenceNotFoundException("Residence not found");

        if (residenceToUpdate.Person.User.Id != userId)
            throw new UnauthorizedAccessException("You are not authorized to update this residence");

        if (residence.City != null)
            residenceToUpdate.City = residence.City;

        if (residence.HouseNumber != null)
            residenceToUpdate.HouseNumber = residence.HouseNumber;

        if (residence.Street != null)
            residenceToUpdate.Street = residence.Street;

        if (residence.ApartmentNumber != null)
            residenceToUpdate.ApartmentNumber = residence.ApartmentNumber;

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