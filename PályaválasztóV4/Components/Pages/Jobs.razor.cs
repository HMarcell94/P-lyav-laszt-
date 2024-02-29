using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace PalyavalsztoV4.Components.Pages
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
        public v4_1Service v4_1Service { get; set; }

        protected IEnumerable<PalyavalsztoV4.Models.v4_1.job> jobs;

        protected RadzenDataGrid<PalyavalsztoV4.Models.v4_1.job> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            jobs = await v4_1Service.Getjobs(new Query { Filter = $@"i => i.JobTitle.Contains(@0) || i.JobDescription.Contains(@0) || i.JobRequirements.Contains(@0) || i.JobLocation.Contains(@0)", FilterParameters = new object[] { search }, Expand = "employer" });
        }
        protected override async Task OnInitializedAsync()
        {
            jobs = await v4_1Service.Getjobs(new Query { Filter = $@"i => i.JobTitle.Contains(@0) || i.JobDescription.Contains(@0) || i.JobRequirements.Contains(@0) || i.JobLocation.Contains(@0)", FilterParameters = new object[] { search }, Expand = "employer" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddJob>("Add job", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PalyavalsztoV4.Models.v4_1.job> args)
        {
            await DialogService.OpenAsync<EditJob>("Edit job", new Dictionary<string, object> { {"JobID", args.Data.JobID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PalyavalsztoV4.Models.v4_1.job job)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await v4_1Service.Deletejob(job.JobID);

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
                await v4_1Service.ExportjobsToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "employer",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "jobs");
            }

            if (args == null || args.Value == "xlsx")
            {
                await v4_1Service.ExportjobsToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "employer",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "jobs");
            }
        }
    }
}