using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

using Petz.Dal.Entities;
using Petz.Dal.Services;

namespace Petz.Web.Pages.App.Families
{
    [Authorize]
    public class AddFamilyModel : PageModel
    {
        private readonly ILogger<AddFamilyModel> _logger;
        private readonly UserManager<User> _userManager;

        public User CurrentUser { get; set; }

        [BindProperty]
        public FamilyInputModel FamilyInput { get; set; }

        public class FamilyInputModel
        {
            [Required]
            public string Name { get; set; }

            public string Description { get; set; }
        }

        public AddFamilyModel(ILogger<AddFamilyModel> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync([FromServices] FamilyService familyService) 
        {
            if(ModelState.IsValid)
            {
                CurrentUser = await _userManager.GetUserAsync(this.User);

                Family family = new Family
                {
                    Name = FamilyInput.Name,
                    Description = FamilyInput.Description
                };

                await familyService.AddFamilyAsync(family, CurrentUser);

                return LocalRedirect("~/App");
            }

            return Page();
        }
    }
}