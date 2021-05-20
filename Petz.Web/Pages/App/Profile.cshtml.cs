using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using Petz.Dal.Entities;
using Petz.Dal.Services;

namespace Petz.Web.Pages.App
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly ILogger<ProfileModel> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly long _fileSizeLimit;
        private List<string> _allowedImageExtensions;


        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FamilyId { get; set; }

        public string CurrentUserId { get; set; }

        [BindProperty]
        public User UserProfile { get; set; }

        [BindProperty]
        public IFormFile UserImage { get; set; }

        public ProfileModel(ILogger<ProfileModel> logger, UserManager<User> userManager, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _logger = logger;
            _userManager = userManager;
            _environment = environment;
            _fileSizeLimit = configuration.GetValue<long>("FileSizeLimit");
            _allowedImageExtensions = configuration.GetSection("AllowedImageExtensions").Get<List<string>>();
        }

        public void OnGet([FromServices] UserService userService)
        {
            CurrentUserId = _userManager.GetUserId(this.User);
            UserProfile = userService.GetUserById(Id);
        }

        public async Task<IActionResult> OnPostAsync([FromServices] UserService userService)
        {
            User user = new User {
                Id = Id,
                Name = UserProfile.Name
            };

            await userService.EditUserAsync(user);

            if (UserImage != null)
            {
                var fileName = UserImage.FileName;
                var extension = Path.GetExtension(fileName).ToLowerInvariant();

                if(string.IsNullOrEmpty(extension) || !_allowedImageExtensions.Contains(extension))
                {
                    ModelState.AddModelError("UserImage", "The uploaded image is not of proper extension.");
                    return Page();
                }

                var filePath = Path.Combine(_environment.WebRootPath, $"img/users/{Id}{extension}");
                using(var stream = System.IO.File.Create(filePath))
                {
                    await UserImage.CopyToAsync(stream);
                }
            }
            
            return LocalRedirect("~/App/Families/Family?id=" + FamilyId);
        }
    }
}