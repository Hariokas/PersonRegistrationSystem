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

        modelBuilder.Entity<User>()
            .HasMany(u => u.People)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Person>()
            .HasOne(p => p.Residence)
            .WithOne(r => r.Person)
            .HasForeignKey<Residence>(r => r.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}