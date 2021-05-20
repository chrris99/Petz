using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

using Petz.Dal.Entities;
using Petz.Dal.Services;

namespace Petz.Web.Pages.App.Tasks
{
    [Authorize]
    public class AddTaskModel : PageModel
    {
        private readonly ILogger<AddTaskModel> _logger;
        private readonly UserManager<User> _userManager;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public TaskInputModel PetTask { get; set; }

        public Pet Pet { get; set; }

        public class TaskInputModel
        {
            [Required]
            public string Title { get; set; }

            public string Description { get; set; }

            public DateTime Date { get; set; }

            public DateTime Time { get; set; }
        }

        public AddTaskModel(ILogger<AddTaskModel> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public void OnGet([FromServices] PetService petService)
        {
            Pet = petService.GetPetById(Id);
        }

        public async Task<IActionResult> OnPostAsync([FromServices] PetService petService, [FromServices] UserService userService) 
        {
            if(ModelState.IsValid)
            {
                var createdBy = userService.GetUserById(_userManager.GetUserId(this.User));
                var dueDate = new DateTime(
                    PetTask.Date.Year, PetTask.Date.Month, PetTask.Date.Day, 
                    PetTask.Time.Hour, PetTask.Time.Minute, PetTask.Time.Second);
                
                PetTask task = new PetTask
                {
                    Title = PetTask.Title,
                    Description = PetTask.Description,
                    Status = PetTaskStatus.ToDo,
                    CreatedBy = createdBy,
                    Date = dueDate
                };

                await petService.AddTaskToPetAsync(Id, task);
                return LocalRedirect("~/App/Pets/Pet?id=" + Id);
            }

            return Page();
        }
    }
}