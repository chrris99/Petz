using Microsoft.AspNetCore.Identity;

namespace Petz.Dal.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public virtual Family Family { get; set; }
    }
}