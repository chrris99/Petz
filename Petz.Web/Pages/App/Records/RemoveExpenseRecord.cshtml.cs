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


namespace Petz.Web.Pages.App.Records
{
    [Authorize]
    public class RemoveExpenseRecordModel : PageModel
    {
        private readonly ILogger<RemoveExpenseRecordModel> _logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public ExpenseRecord CurrentRecord { get; set; }

        public RemoveExpenseRecordModel(ILogger<RemoveExpenseRecordModel> logger)
        {
            _logger = logger;
        }

        public void OnGet([FromServices] RecordService recordService)
        {
            CurrentRecord = recordService.GetExpenseRecordById(Id);
        }

        public async Task<IActionResult> OnPostAsync([FromServices] RecordService recordService)
        {
            CurrentRecord = recordService.GetExpenseRecordById(Id);
            
            await recordService.RemoveExpenseRecordByIdAsync(Id);
            return LocalRedirect("~/App/Pets/Pet?id=" + CurrentRecord.Pet.Id);
        }
    }
}