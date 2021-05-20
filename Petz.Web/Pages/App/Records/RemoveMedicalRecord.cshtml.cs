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
    public class RemoveMedicalRecordModel : PageModel
    {
        private readonly ILogger<RemoveMedicalRecordModel> _logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public MedicalRecord CurrentRecord { get; set; }

        public RemoveMedicalRecordModel(ILogger<RemoveMedicalRecordModel> logger)
        {
            _logger = logger;
        }

        public void OnGet([FromServices] RecordService recordService)
        {
            CurrentRecord = recordService.GetMedicalRecordById(Id);
        }

        public async Task<IActionResult> OnPostAsync([FromServices] RecordService recordService)
        {
            CurrentRecord = recordService.GetMedicalRecordById(Id);
            
            await recordService.RemoveMedicalRecordByIdAsync(Id);
            return LocalRedirect("~/App/Pets/Pet?id=" + CurrentRecord.Pet.Id);
        }
    }
}