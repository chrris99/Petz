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
    public class AddUserModel : PageModel
    {
        private readonly ILogger<AddUserModel> _logger;
        private readonly UserManager<User> _userManager;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Family CurrentFamily { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }


        public AddUserModel(ILogger<AddUserModel> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public void OnGet()
        {
            _logger.LogInformation(Id.ToString());
        }

        public async Task<IActionResult> OnPostAsync([FromServices] FamilyService familyService, [FromServices] UserService userService) 
        {
            if(ModelState.IsValid)
            {
                CurrentFamily = familyService.GetFamilyById(Id);

                try
                {
                    var user = userService.GetUserByEmail(UserEmail);
                    await familyService.AddUserToFamilyAsync(Id, user);

                    return LocalRedirect("~/App/Families/Family?id=" + Id);
                }
                catch (System.ArgumentNullException)
                {
                    _logger.LogError("ArgumentNullException occured when getting user by email!");
                    ErrorMessage = "An unexpected error occured, please try again later!";

                    if (!string.IsNullOrEmpty(ErrorMessage))
                    {
                        ModelState.AddModelError(string.Empty, ErrorMessage);
                    }

                    return Page();
                }
                catch (System.InvalidOperationException)
                {
                    _logger.LogError("InvalidOperationException occured when getting user by email!");
                    ErrorMessage = "No registered user with the following email exists.";

                    if (!string.IsNullOrEmpty(ErrorMessage))
                    {
                        ModelState.AddModelError(string.Empty, ErrorMessage);
                    }

                    return Page();
                }
            }

            return Page();
        }
    }
}