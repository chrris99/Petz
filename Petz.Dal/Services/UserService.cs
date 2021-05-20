using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Petz.Dal.Dtos;
using Petz.Dal.Entities;


namespace Petz.Dal.Services
{
    public class UserService
    {
        public ApplicationDbContext Context { get; }

        public UserService(ApplicationDbContext context)
        {
            Context = context;
        }

        public User GetUserById(string userId)
        {
            return Context.Users.Single(user => user.Id == userId);
        }

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = Context.Users.Single(user => user.Id == userId);
            var families = await Context.Families
                .Include(family => family.Users)
                .AsSplitQuery()
                .Where(family => family.Users.Contains(user))
                .AsQueryable()
                .ToListAsync();

            var pets = await Context.Pets
                .Include(pet => pet.MedicalRecords)
                .Include(pet => pet.ExpenseRecords)
                .AsSplitQuery()
                .Where(pet => families.Contains(pet.Family))
                .AsQueryable()
                .ToListAsync();

            var tasks = await Context.PetTasks
                .Include(task => task.Pet)
                .AsSplitQuery()
                .Where(task => pets.Contains(task.Pet) && task.Status != Entities.PetTaskStatus.Done)
                .AsQueryable()
                .ToListAsync();

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                
                Families = families,
                Pets = pets,
                UncompletedTasks = tasks
            };
        }
    
        public User GetUserByEmail(string userEmail)
        {
            return Context.Users.Single(user => user.Email == userEmail);
        }
    
        public async Task EditUserAsync(User modifiedUser)
        {
            var user = Context.Users.Single(user => user.Id == modifiedUser.Id);
            user.Name = modifiedUser.Name;

            await Context.SaveChangesAsync();
        }
    }
}