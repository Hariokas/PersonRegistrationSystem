using Repository.Models;
using Shared.DTOs.Residence;

namespace Repository.Extensions;

public static class ResidenceExtensions
{
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