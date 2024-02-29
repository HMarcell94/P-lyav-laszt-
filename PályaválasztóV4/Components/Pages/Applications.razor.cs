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
    public partial class Applications
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

        protected IEnumerable<PalyavalsztoV4.Models.v4_1.application> applications;

        protected RadzenDataGrid<PalyavalsztoV4.Models.v4_1.application> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            applications = await v4_1Service.Getapplications(new Query { Filter = $@"i => i.ApplicationStatus.Contains(@0)", FilterParameters = new object[] { search }, Expand = "job,employee" });
        }
        protected override async Task OnInitializedAsync()
        {
            applications = await v4_1Service.Getapplications(new Query { Filter = $@"i => i.ApplicationStatus.Contains(@0)", FilterParameters = new object[] { search }, Expand = "job,employee" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddApplication>("Add application", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PalyavalsztoV4.Models.v4_1.application> args)
        {
            await DialogService.OpenAsync<EditApplication>("Edit application", new Dictionary<string, object> { {"ApplicationID", args.Data.ApplicationID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PalyavalsztoV4.Models.v4_1.application application)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await v4_1Service.Deleteapplication(application.ApplicationID);

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
                    Detail = $"Unable to delete application"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await v4_1Service.ExportapplicationsToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "job,employee",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "applications");
            }

            if (args == null || args.Value == "xlsx")
            {
                await v4_1Service.ExportapplicationsToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "job,employee",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "applications");
            }
        }
    }
}