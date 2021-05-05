using System.Collections.Generic;

namespace Petz.Dal.Entities
{
    public class Family
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }
    }
}