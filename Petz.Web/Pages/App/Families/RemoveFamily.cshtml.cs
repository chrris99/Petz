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
    public class RemoveFamilyModel : PageModel
    {
        private readonly ILogger<RemoveFamilyModel> _logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Family CurrentFamily { get; set; }

        public RemoveFamilyModel(ILogger<RemoveFamilyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet([FromServices] FamilyService familyService)
        {
            CurrentFamily = familyService.GetFamilyById(Id);
        }

        public async Task<IActionResult> OnPostAsync([FromServices] FamilyService familyService)
        {
            await familyService.RemoveFamilyAsync(Id);
            return LocalRedirect("~/App/");
        }
    }
}