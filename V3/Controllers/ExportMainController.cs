using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using PalyavalsztoV3.Data;

namespace PalyavalsztoV3.Controllers
{
    public partial class ExportmainController : ExportController
    {
        private readonly mainContext context;
        private readonly mainService service;

        public ExportmainController(mainContext context, mainService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/main/adminroles/csv")]
        [HttpGet("/export/main/adminroles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportadminrolesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getadminroles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/adminroles/excel")]
        [HttpGet("/export/main/adminroles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportadminrolesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getadminroles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/applications/csv")]
        [HttpGet("/export/main/applications/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportapplicationsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getapplications(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/applications/excel")]
        [HttpGet("/export/main/applications/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportapplicationsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getapplications(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/employees/csv")]
        [HttpGet("/export/main/employees/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportemployeesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getemployees(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/employees/excel")]
        [HttpGet("/export/main/employees/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportemployeesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getemployees(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/employers/csv")]
        [HttpGet("/export/main/employers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportemployersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getemployers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/employers/excel")]
        [HttpGet("/export/main/employers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportemployersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getemployers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/jobs/csv")]
        [HttpGet("/export/main/jobs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportjobsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getjobs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/jobs/excel")]
        [HttpGet("/export/main/jobs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportjobsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getjobs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/locations/csv")]
        [HttpGet("/export/main/locations/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportlocationsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getlocations(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/locations/excel")]
        [HttpGet("/export/main/locations/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportlocationsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getlocations(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/roles/csv")]
        [HttpGet("/export/main/roles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportrolesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getroles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/roles/excel")]
        [HttpGet("/export/main/roles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportrolesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getroles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/supportroles/csv")]
        [HttpGet("/export/main/supportroles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportsupportrolesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getsupportroles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/supportroles/excel")]
        [HttpGet("/export/main/supportroles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportsupportrolesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getsupportroles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/users/csv")]
        [HttpGet("/export/main/users/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportusersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getusers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/users/excel")]
        [HttpGet("/export/main/users/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportusersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getusers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/userroles/csv")]
        [HttpGet("/export/main/userroles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportuserrolesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getuserroles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/main/userroles/excel")]
        [HttpGet("/export/main/userroles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportuserrolesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getuserroles(), Request.Query, false), fileName);
        }
    }
}
