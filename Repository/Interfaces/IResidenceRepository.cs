using Repository.Models;

namespace Repository.Interfaces;

public interface IResidenceRepository
{
    Task AddResidenceAsync(Residence residence);
    Task<Residence?> GetResidenceByIdAsync(Guid residenceId);
    Task<Residence?> GetResidenceByPersonIdAsync(Guid personId);
    Task UpdateResidenceAsync(Residence residence);
    Task DeleteResidenceAsync(Residence residence);
}