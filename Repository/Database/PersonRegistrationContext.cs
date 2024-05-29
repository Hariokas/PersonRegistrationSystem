using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Repository.Database;

public class PersonRegistrationContext : DbContext
{
    public PersonRegistrationContext(DbContextOptions<PersonRegistrationContext> options) : base(options)
    {
    }

    public DbSet<Person> People { get; set; }
    public DbSet<Residence> Residences { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}