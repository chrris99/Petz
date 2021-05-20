using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Petz.Dal.Entities;


namespace Petz.Dal.Services
{
    public class RecordService
    {
        public ApplicationDbContext Context { get; }

        public RecordService(ApplicationDbContext context)
        {
            Context = context;
        }

        public MedicalRecord GetMedicalRecordById(int medicalRecordId)
        {
            var medicalRecord = Context.MedicalRecords
                .Include(record => record.Pet)
                .AsSplitQuery()
                .Single(record => record.Id == medicalRecordId);

            return medicalRecord;
        }

        public ExpenseRecord GetExpenseRecordById(int expenseRecordId)
        {
            var expenseRecord = Context.ExpenseRecords
                .Include(record => record.Pet)
                .AsSplitQuery()
                .Single(record => record.Id == expenseRecordId);
                
            return expenseRecord;
        }

        public async Task RemoveMedicalRecordByIdAsync(int medicalRecordId)
        {
            var medicalRecord = Context.MedicalRecords.Single(record => record.Id == medicalRecordId);
            Context.MedicalRecords.Remove(medicalRecord);

            await Context.SaveChangesAsync();
        }

        public async Task RemoveExpenseRecordByIdAsync(int expenseRecordId)
        {
            var expenseRecord = Context.ExpenseRecords.Single(record => record.Id == expenseRecordId);
            Context.ExpenseRecords.Remove(expenseRecord);

            await Context.SaveChangesAsync();
        }

        public async Task CompleteTaskByIdAsync(int taskId)
        {
            var task = Context.PetTasks.Single(task => task.Id == taskId);
            task.Status = PetTaskStatus.Done;

            await Context.SaveChangesAsync();
        }
    }
}