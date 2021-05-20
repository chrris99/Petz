using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Petz.Dal.Entities;

namespace Petz.Dal.Dtos
{
    public class UserDto : IdentityUser
    {
        public IEnumerable<Family> Families { get; set; }
        public IEnumerable<Pet> Pets { get; set; }
        public IEnumerable<PetTask> UncompletedTasks { get; set; }
    }
}