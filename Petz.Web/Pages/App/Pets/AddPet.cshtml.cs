using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using Petz.Dal.Entities;
using Petz.Dal.Services;

namespace Petz.Web.Pages.App
{
    [Authorize]
    public class AddPetModel : PageModel
    {
        private readonly ILogger<AddPetModel> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly long _fileSizeLimit;
        private List<string> _allowedImageExtensions;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Family Family { get; set; }

        [BindProperty]
        public PetInputModel PetInput { get; set; }

        [BindProperty]
        public IFormFile PetImage { get; set; }

        public enum Animal {
            Dog,
            Cat,
            Fish,
            Hamster    
        }
        
        public class PetInputModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public Animal Type { get; set; }

            public string Breed { get; set; }
        }

        public AddPetModel(ILogger<AddPetModel> logger, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
            _fileSizeLimit = configuration.GetValue<long>("FileSizeLimit");
            _allowedImageExtensions = configuration.GetSection("AllowedImageExtensions").Get<List<string>>();
        }

        public void OnGet([FromServices] FamilyService familyService)
        {
            Family = familyService.GetFamilyById(Id); 
        }

        public async Task<IActionResult> OnPostAsync([FromServices] FamilyService familyService) 
        {
            if(ModelState.IsValid)
            {
                var fileName = PetImage.FileName;
                var extension = Path.GetExtension(fileName).ToLowerInvariant();

                if(string.IsNullOrEmpty(extension) || !_allowedImageExtensions.Contains(extension))
                {
                    ModelState.AddModelError("PetImage", "The uploaded image is not of proper extension.");
                    return Page();
                }

                Family = familyService.GetFamilyById(Id); 

                Pet pet = new Pet
                {
                    Name = PetInput.Name,
                    Type = (Petz.Dal.Entities.Animal) PetInput.Type,
                    Breed = PetInput.Breed
                };

                var petId = await familyService.AddPetToFamilyAsync(Id, pet);

                if(PetImage != null)
                {
                    var filePath = Path.Combine(_environment.WebRootPath, $"img/pets/{petId}{extension}");
                    using(var stream = System.IO.File.Create(filePath))
                    {
                        await PetImage.CopyToAsync(stream);
                    }
                }
                
                return LocalRedirect("~/App/Families/Family?id=" + Id);
            }

            return Page();
        }
    }
}