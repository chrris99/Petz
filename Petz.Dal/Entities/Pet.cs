using System;
using System.Collections.Generic;

namespace Petz.Dal.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Animal Type { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        
        public virtual Family Family { get; set; }
        public virtual ICollection<PetTask> PetTasks { get; set; }
        public virtual ICollection<ExpenseRecord> ExpenseRecords { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }


    }
}