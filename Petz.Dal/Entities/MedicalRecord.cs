namespace Petz.Dal.Entities
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual User CreatedBy { get; set; }
        public virtual Category Category { get; set; }
        public virtual Pet Pet { get; set; }
    }
}