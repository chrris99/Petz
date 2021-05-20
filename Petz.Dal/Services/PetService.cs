using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Petz.Dal.Entities;


namespace Petz.Dal.Services
{
    public class PetService
    {
        public ApplicationDbContext Context { get; }

        public PetService(ApplicationDbContext context)
        {
            Context = context;
        }

        public Pet GetPetById(int petId)
        {
            var pet = Context.Pets
                .Include(pet => pet.Family)
                .Include(pet => pet.PetTasks)
                .Include(pet => pet.MedicalRecords)
                .Include(pet => pet.ExpenseRecords)
                .AsSplitQuery()
                .Single(pet => pet.Id == petId);
            
            return pet;
        }


        public async Task RemovePetByIdAsync(int petId)
        {
            var pet = Context.Pets.Single(pet => pet.Id == petId);
            Context.Pets.Remove(pet);

            await Context.SaveChangesAsync();
        }


        public async Task AddTaskToPetAsync(int petId, PetTask task)
        {
            var pet = Context.Pets
                .Include(pet => pet.PetTasks)
                .Single(pet => pet.Id == petId);

            pet.PetTasks.Add(task);
            await Context.SaveChangesAsync();
        }


        public async Task AddExpenseRecordToPetAsync(int petId, ExpenseRecord expenseRecord)
        {
            var pet = Context.Pets
                .Include(pet => pet.ExpenseRecords)
                .Single(pet => pet.Id == petId);

            pet.ExpenseRecords.Add(expenseRecord);
            await Context.SaveChangesAsync();
        }


        public async Task AddMedicalRecordToPetAsync(int petId, MedicalRecord medicalRecord)
        {
            var pet = Context.Pets
                .Include(pet => pet.MedicalRecords)
                .Single(pet => pet.Id == petId);

            pet.MedicalRecords.Add(medicalRecord);
            await Context.SaveChangesAsync();
        }
    }
}