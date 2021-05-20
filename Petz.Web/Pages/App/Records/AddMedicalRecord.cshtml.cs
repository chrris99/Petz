using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

using Petz.Dal.Entities;
using Petz.Dal.Services;

namespace Petz.Web.Pages.App.Records
{
    [Authorize]
    public class AddMedicalRecordModel : PageModel
    {
        private readonly ILogger<AddMedicalRecordModel> _logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public MedicalRecordInputModel MedicalRecord { get; set; }

        public Pet Pet { get; set; }

        public class MedicalRecordInputModel
        {
            [Required]
            public string Title { get; set; }

            public string Description { get; set; }
        }

        public AddMedicalRecordModel(ILogger<AddMedicalRecordModel> logger)
        {
            _logger = logger;
        }

        public void OnGet([FromServices] PetService petService)
        {
            Pet = petService.GetPetById(Id);
        }

        public async Task<IActionResult> OnPostAsync([FromServices] PetService petService) 
        {
            if(ModelState.IsValid)
            {
                MedicalRecord medicalRecord = new MedicalRecord
                {
                    Title = MedicalRecord.Title,
                    Description = MedicalRecord.Description
                };

                await petService.AddMedicalRecordToPetAsync(Id, medicalRecord);

                return LocalRedirect("~/App/Pets/Pet?id=" + Id);
            }

            return Page();
        }
    }
}