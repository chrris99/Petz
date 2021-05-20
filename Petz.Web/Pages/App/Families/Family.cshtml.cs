using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

using Petz.Dal.Dtos;
using Petz.Dal.Entities;
using Petz.Dal.Services;

namespace Petz.Web.Pages.App.Families
{
    [Authorize]
    public class FamilyModel : PageModel
    {
        private readonly ILogger<FamilyModel> _logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Family Family { get; set; }
        public IEnumerable<User> Members { get; set; }
        public IEnumerable<Pet> Pets { get; set; }

        public FamilyModel(ILogger<FamilyModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync([FromServices] FamilyService familyService)
        {
            Family = familyService.GetFamilyById(Id);

            Members = await familyService.GetFamilyMembersAsync(Id);
            Pets = await familyService.GetFamilyPetsAsync(Id);
        }
    }
}