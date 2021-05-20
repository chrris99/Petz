using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using Petz.Dal.Entities;

namespace Petz.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(SignInManager<User> signInManager, ILogger<IndexModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect(Url.Content("~/App"));
            }

            return Page();
        }
    }
}
