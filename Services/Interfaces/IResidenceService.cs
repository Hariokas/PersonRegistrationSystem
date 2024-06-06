using Shared.DTOs.Residence;
using Shared.Enums;

namespace Services.Interfaces;

public interface IResidenceService
{
    Task AddResidenceAsync(Guid userId, ResidenceCreateDto residenceCreateDto);
    Task<ResidenceDto?> GetResidenceByIdAsync(Guid userId, Role userRole, Guid residenceId);
    Task<IEnumerable<ResidenceDto>> GetResidenceByPersonIdAsync(Guid userId, Role userRole, Guid personId);
    Task UpdateResidenceAsync(Guid userId, Role userRole, ResidenceUpdateDto residenceUpdateDto);
    Task DeleteResidenceAsync(Guid userId, Role userRole, Guid residenceId);
}