using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Petz.Dal.Entities;

namespace Petz.Dal
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Family> Families { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ExpenseRecord> ExpenseRecords { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<PetTask> PetTasks { get; set; }
    }
}