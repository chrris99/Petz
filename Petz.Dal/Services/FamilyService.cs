using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Petz.Dal.Dtos;
using Petz.Dal.Entities;


namespace Petz.Dal.Services
{
    public class FamilyService
    {
        public ApplicationDbContext Context { get; }

        public FamilyService(ApplicationDbContext context)
        {
            Context = context;
        }

        public IEnumerable<FamilyDto> GetFamilies()
        {
            var families = Context.Families.Select(family => new FamilyDto
            {
                Id = family.Id,
                Name = family.Name,
                Description = family.Description,
                NumberOfUsers = family.Users.Count(),
                NumberOfPets = family.Pets.Count()
            }).ToList();

            return families;
        }

        public async Task<IEnumerable<FamilyDto>> GetUserFamiliesAsync(string userId)
        {
            var user = Context.Users.Single(user => user.Id == userId);
            
            var families = await Context.Families
                .Where(family => family.Users.Contains(user))
                .Select(family => new FamilyDto
                {
                    Id = family.Id,
                    Name = family.Name,
                    Description = family.Description,
                    NumberOfUsers = family.Users.Count(),
                    NumberOfPets = family.Pets.Count()
                })
                .AsQueryable()
                .ToListAsync();

            return families;
        }

        public async Task<IEnumerable<Pet>> GetFamilyPetsAsync(int familyId)
        {
            var pets = await Context.Pets
                .Include(pet => pet.ExpenseRecords)
                .Include(pet => pet.MedicalRecords)
                .Include(pet => pet.PetTasks)
                .Where(pet => pet.Family.Id == familyId)
                .AsQueryable()
                .ToListAsync();

            return pets;
        }

        public async Task<IEnumerable<User>> GetFamilyMembersAsync(int familyId)
        {
            var family = Context.Families.Single(family => family.Id == familyId);
            var users = await Context.Users
                .Where(user => user.Families.Contains(family))
                .AsQueryable()
                .ToListAsync();

            return users;
        }

        public Family GetFamilyById(int familyId)
        {
            return Context.Families
                .Include(family => family.Users)
                .Include(family => family.Pets)
                .AsSplitQuery()
                .Single(family => family.Id == familyId);
        }

        public async Task AddFamilyAsync(Family family, User user)
        {
            var users = new List<User>();
            users.Add(user);

            family.Users ??= users;

            Context.Families.Add(family);
            await Context.SaveChangesAsync();
        }

        public async Task AddUserToFamilyAsync(int familyId, User user)
        {
            var family = Context.Families
                .Include(family => family.Users)
                .Single(family => family.Id == familyId);

            family.Users.Add(user);
            await Context.SaveChangesAsync();
        }

        public async Task RemoveUserFromFamilyAsync(int familyId, User user)
        {
            
        }

        public async Task<int> AddPetToFamilyAsync(int familyId, Pet pet)
        {
            var family = Context.Families
                .Include(family => family.Pets)
                .AsSplitQuery()
                .Single(family => family.Id == familyId);

            family.Pets.Add(pet);
            await Context.SaveChangesAsync();

            return pet.Id;
        }

        public async Task EditFamilyAsync(Family modifiedFamily)
        {
            var family = Context.Families.Single(family => family.Id == modifiedFamily.Id);

            family.Name = modifiedFamily.Name;
            family.Description = modifiedFamily.Description;

            await Context.SaveChangesAsync();
        }

        public async Task RemoveFamilyAsync(int familyId)
        {
            var family = Context.Families.Single(family => family.Id == familyId);
            Context.Families.Remove(family);

            await Context.SaveChangesAsync();
        }
    }
}