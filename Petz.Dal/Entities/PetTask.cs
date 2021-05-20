using System;

namespace Petz.Dal.Entities
{
    public class PetTask
    {   
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PetTaskStatus Status { get; set; }
        public DateTime Date { get; set; }

        public virtual Pet Pet { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User CompletedBy { get; set; }
    }
}