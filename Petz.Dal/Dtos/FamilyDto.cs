using System.Collections.Generic;

namespace Petz.Dal.Dtos
{
    public class FamilyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int NumberOfUsers { get; set; }
        public int NumberOfPets { get; set; }
    }
}