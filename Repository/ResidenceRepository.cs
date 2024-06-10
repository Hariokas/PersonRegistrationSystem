using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Interfaces;
using Repository.Models;

namespace Repository;

public class ResidenceRepository(PersonRegistrationContext dbContext) : IResidenceRepository
{
    public async Task AddResidenceAsync(Residence residence)
    {
        await dbContext.Residences.AddAsync(residence);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Residence?> GetResidenceByIdAsync(Guid residenceId)
    {
        return await dbContext.Residences.Include(r => r.Person)
            .FirstOrDefaultAsync(r => r.Id == residenceId);
    }

    public async Task<Residence?> GetResidenceByPersonIdAsync(Guid personId)
    {
        return await dbContext.Residences.Include(r => r.Person)
            .FirstOrDefaultAsync(r => r.PersonId == personId);
    }

    public async Task UpdateResidenceAsync(Residence residence)
    {
        dbContext.Residences.Update(residence);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteResidenceAsync(Residence residence)
    {
        dbContext.Residences.Remove(residence);
        await dbContext.SaveChangesAsync();
    }
}