using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace PalyavalasztoV2.Pages
{
    public partial class Jobs
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public mainService mainService { get; set; }

        protected IEnumerable<PalyavalasztoV2.Models.main.job> jobs;

        protected RadzenDataGrid<PalyavalasztoV2.Models.main.job> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            jobs = await mainService.Getjobs(new Query { Filter = $@"i => i.Title.Contains(@0) || i.Description.Contains(@0)", FilterParameters = new object[] { search }, Expand = "employer" });
        }
        protected override async Task OnInitializedAsync()
        {
            jobs = await mainService.Getjobs(new Query { Filter = $@"i => i.Title.Contains(@0) || i.Description.Contains(@0)", FilterParameters = new object[] { search }, Expand = "employer" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddJob>("Add job", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PalyavalasztoV2.Models.main.job> args)
        {
            await DialogService.OpenAsync<EditJob>("Edit job", new Dictionary<string, object> { {"JobID", args.Data.JobID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PalyavalasztoV2.Models.main.job job)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await mainService.Deletejob(job.JobID);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete job"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await mainService.ExportjobsToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "employer",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "jobs");
            }

            if (args == null || args.Value == "xlsx")
            {
                await mainService.ExportjobsToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "employer",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "jobs");
            }
        }

        protected PalyavalasztoV2.Models.main.job job;
        protected async Task GetChildData(PalyavalasztoV2.Models.main.job args)
        {
            job = args;
            var applicationsResult = await mainService.Getapplications(new Query { Filter = $@"i => ", Expand = "job,supportrole,role,employee" });
            if (applicationsResult != null)
            {
                args.applications = applicationsResult.ToList();
            }
        }

        protected RadzenDataGrid<PalyavalasztoV2.Models.main.application> applicationsDataGrid;

        protected async Task applicationsAddButtonClick(MouseEventArgs args, PalyavalasztoV2.Models.main.job data)
        {
            var dialogResult = await DialogService.OpenAsync<AddApplication>("Add applications", new Dictionary<string, object> { {"EmployeeId" , data.EmployerId} });
            await GetChildData(data);
            await applicationsDataGrid.Reload();
        }

        protected async Task applicationsRowSelect(DataGridRowMouseEventArgs<PalyavalasztoV2.Models.main.application> args, PalyavalasztoV2.Models.main.job data)
        {
            var dialogResult = await DialogService.OpenAsync<EditApplication>("Edit applications", new Dictionary<string, object> { {"ApplicationID", args.Data.ApplicationID} });
            await GetChildData(data);
            await applicationsDataGrid.Reload();
        }

        protected async Task applicationsDeleteButtonClick(MouseEventArgs args, PalyavalasztoV2.Models.main.application application)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await mainService.Deleteapplication(application.ApplicationID);

                    await GetChildData(job);

                    if (deleteResult != null)
                    {
                        await applicationsDataGrid.Reload();
                    }
                }
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete application"
                });
            }
        }
    }
}