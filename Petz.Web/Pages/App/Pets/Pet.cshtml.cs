using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
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
    public class PetModel : PageModel
    {
        private readonly ILogger<PetModel> _logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public int TaskId { get; set; }

        public Pet Pet { get; set; }
        public List<PetTask> UncompletedTasks { get; set; }

        public PetModel(ILogger<PetModel> logger)
        {
            _logger = logger;
        }

        public void OnGet([FromServices] PetService petService)
        {
            Pet = petService.GetPetById(Id);
            UncompletedTasks = Pet.PetTasks.Where(task => task.Status != PetTaskStatus.Done).ToList();
        }

        public async Task<IActionResult> OnPostCompleteAsync([FromServices] PetService petService, [FromServices] RecordService recordService)
        {
            Pet = petService.GetPetById(Id);

            await recordService.CompleteTaskByIdAsync(TaskId);
            return LocalRedirect("~/App/Pets/Pet?id=" + Pet.Id);
        }
    }
}