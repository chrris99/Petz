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
    public class EditFamilyModel : PageModel
    {
        private readonly ILogger<EditFamilyModel> _logger;
        private readonly UserManager<User> _userManager;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public User CurrentUser { get; set; }
        
        [BindProperty]
        public Family Family { get; set; }

        [BindProperty]
        public FamilyInputModel FamilyInput { get; set; }

        public class FamilyInputModel
        {
            [Required]
            public string Name { get; set; }

            public string Description { get; set; }
        }

        public EditFamilyModel(ILogger<EditFamilyModel> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public void OnGet([FromServices] FamilyService familyService) 
        {
            Family = familyService.GetFamilyById(Id);

            FamilyInput = new FamilyInputModel
            {
                Name = Family.Name,
                Description = Family.Description
            };
        }

        public async Task<ActionResult> OnPostAsync([FromServices] FamilyService familyService) 
        {
            if(ModelState.IsValid)
            {
                Family.Id = Id;
                Family.Name = FamilyInput.Name;
                Family.Description = FamilyInput.Description;

                await familyService.EditFamilyAsync(Family);
                return LocalRedirect("~/App/Families/Family?id=" + Id);
            }

            return Page();
        }
    }
}