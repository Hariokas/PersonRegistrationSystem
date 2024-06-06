using System.ComponentModel.DataAnnotations;
using Shared.DTOs.Residence;

namespace Shared.Validations;

public static class ResidenceValidationHelper
{
    public static void ValidateResidenceCreateDto(ResidenceCreateDto residenceCreateDto)
    {
        if (string.IsNullOrWhiteSpace(residenceCreateDto.City))
            throw new ValidationException("City is required.");

        if (string.IsNullOrWhiteSpace(residenceCreateDto.Street))
            throw new ValidationException("Street is required.");

        if (string.IsNullOrWhiteSpace(residenceCreateDto.HouseNumber))
            throw new ValidationException("House number is required.");
    }
}