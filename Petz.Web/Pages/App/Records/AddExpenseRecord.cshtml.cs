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
    public class AddExpenseRecordModel : PageModel
    {
        private readonly ILogger<AddExpenseRecordModel> _logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public ExpenseRecordInputModel ExpenseRecord { get; set; }

        public Pet Pet { get; set; }

        public class ExpenseRecordInputModel
        {
            [Required]
            public string Title { get; set; }

            public string Description { get; set; }

            [Required]
            public int Price { get; set; }
        }

        public AddExpenseRecordModel(ILogger<AddExpenseRecordModel> logger)
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
                ExpenseRecord expenseRecord = new ExpenseRecord
                {
                    Title = ExpenseRecord.Title,
                    Description = ExpenseRecord.Description,
                    Price = ExpenseRecord.Price
                };

                await petService.AddExpenseRecordToPetAsync(Id, expenseRecord);

                return LocalRedirect("~/App/Pets/Pet?id=" + Id);
            }

            return Page();
        }
    }
}