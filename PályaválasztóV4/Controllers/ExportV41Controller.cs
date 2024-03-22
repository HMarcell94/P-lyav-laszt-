using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using PalyavalsztoV4.Data;

namespace PalyavalsztoV4.Controllers
{
    public partial class Exportv4_1Controller : ExportController
    {
        private readonly v4_1Context context;
        private readonly v4_1Service service;

        public Exportv4_1Controller(v4_1Context context, v4_1Service service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/v4_1/applications/csv")]
        [HttpGet("/export/v4_1/applications/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportapplicationsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getapplications(), Request.Query, false), fileName);
        }

        [HttpGet("/export/v4_1/applications/excel")]
        [HttpGet("/export/v4_1/applications/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportapplicationsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getapplications(), Request.Query, false), fileName);
        }

        [HttpGet("/export/v4_1/employees/csv")]
        [HttpGet("/export/v4_1/employees/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportemployeesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getemployees(), Request.Query, false), fileName);
        }

        [HttpGet("/export/v4_1/employees/excel")]
        [HttpGet("/export/v4_1/employees/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportemployeesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getemployees(), Request.Query, false), fileName);
        }

        [HttpGet("/export/v4_1/employers/csv")]
        [HttpGet("/export/v4_1/employers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportemployersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getemployers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/v4_1/employers/excel")]
        [HttpGet("/export/v4_1/employers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportemployersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getemployers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/v4_1/jobs/csv")]
        [HttpGet("/export/v4_1/jobs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportjobsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getjobs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/v4_1/jobs/excel")]
        [HttpGet("/export/v4_1/jobs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportjobsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getjobs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/v4_1/roles/csv")]
        [HttpGet("/export/v4_1/roles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportrolesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getroles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/v4_1/roles/excel")]
        [HttpGet("/export/v4_1/roles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportrolesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getroles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/v4_1/users/csv")]
        [HttpGet("/export/v4_1/users/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportusersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.Getusers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/v4_1/users/excel")]
        [HttpGet("/export/v4_1/users/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportusersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.Getusers(), Request.Query, false), fileName);
        }
    }
}
