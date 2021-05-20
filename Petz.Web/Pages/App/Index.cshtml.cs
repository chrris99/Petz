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

namespace Petz.Web.Pages.App
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<User> _userManager;

        [BindProperty]
        public int Id { get; set; }

        public UserDto CurrentUser { get; set; }

        public IndexModel(ILogger<IndexModel> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task OnGetAsync([FromServices] UserService userService)
        {
            CurrentUser = await userService.GetUserByIdAsync(_userManager.GetUserId(this.User));
        }

        public async Task<IActionResult> OnPostCompleteAsync([FromServices] UserService userService, [FromServices] RecordService recordService)
        {
            CurrentUser = await userService.GetUserByIdAsync(_userManager.GetUserId(this.User));
            
            await recordService.CompleteTaskByIdAsync(Id);
            return LocalRedirect("~/App");
        }
    }
}