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


namespace Petz.Web.Pages.App.Pets
{
    [Authorize]
    public class RemovePetModel : PageModel
    {
        private readonly ILogger<RemovePetModel> _logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Pet CurrentPet { get; set; }

        public RemovePetModel(ILogger<RemovePetModel> logger)
        {
            _logger = logger;
        }

        public void OnGet([FromServices] PetService petService)
        {
            CurrentPet = petService.GetPetById(Id);
        }

        public async Task<IActionResult> OnPostAsync([FromServices] PetService petService)
        {
            CurrentPet = petService.GetPetById(Id);
            
            await petService.RemovePetByIdAsync(Id);
            return LocalRedirect("~/App/Families/Family?id=" + CurrentPet.Family.Id);
        }
    }
}