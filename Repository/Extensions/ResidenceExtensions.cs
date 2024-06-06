using Repository.Models;
using Shared.DTOs.Residence;

namespace Repository.Extensions;

public static class ResidenceExtensions
{

    public static void UpdateWithDto(this Residence residence, ResidenceUpdateDto residenceUpdateDto)
    {
        residence.City = residenceUpdateDto.City ?? residence.City;
        residence.Street = residenceUpdateDto.Street ?? residence.Street;
        residence.HouseNumber = residenceUpdateDto.HouseNumber ?? residence.HouseNumber;
        residence.ApartmentNumber = residenceUpdateDto.ApartmentNumber ?? residence.ApartmentNumber;
    }

    public static Residence ToResidence(this ResidenceCreateDto residenceCreateDto, Person person)
    {
        return new Residence
        {
            City = residenceCreateDto.City,
            HouseNumber = residenceCreateDto.HouseNumber,
            Street = residenceCreateDto.Street,
            ApartmentNumber = residenceCreateDto.ApartmentNumber,
            Person = person,
            PersonId = person.Id
        };
    }

    public static Residence ToResidence(this ResidenceDto residenceDto)
    {
        return new Residence
        {
            City = residenceDto.City ?? "",
            Street = residenceDto.Street ?? "",
            HouseNumber = residenceDto.HouseNumber ?? "",
            ApartmentNumber = residenceDto.ApartmentNumber,
            PersonId = residenceDto.PersonId
        };
    }

    public static ResidenceDto ToResidenceDto(this Residence residence)
    {
        return new ResidenceDto
        {
            City = residence.City,
            Street = residence.Street,
            HouseNumber = residence.HouseNumber,
            ApartmentNumber = residence.ApartmentNumber,
            PersonId = residence.PersonId,
            Id = residence.Id
        };
    }

}