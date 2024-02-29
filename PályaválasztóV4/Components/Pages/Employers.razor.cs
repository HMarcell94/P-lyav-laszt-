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
    public partial class Employers
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

        protected IEnumerable<PalyavalsztoV4.Models.v4_1.employer> employers;

        protected RadzenDataGrid<PalyavalsztoV4.Models.v4_1.employer> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            employers = await v4_1Service.Getemployers(new Query { Filter = $@"i => i.CompanyName.Contains(@0) || i.CompanyDescription.Contains(@0)", FilterParameters = new object[] { search }, Expand = "user" });
        }
        protected override async Task OnInitializedAsync()
        {
            employers = await v4_1Service.Getemployers(new Query { Filter = $@"i => i.CompanyName.Contains(@0) || i.CompanyDescription.Contains(@0)", FilterParameters = new object[] { search }, Expand = "user" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddEmployer>("Add employer", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PalyavalsztoV4.Models.v4_1.employer> args)
        {
            await DialogService.OpenAsync<EditEmployer>("Edit employer", new Dictionary<string, object> { {"EmployerID", args.Data.EmployerID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PalyavalsztoV4.Models.v4_1.employer employer)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await v4_1Service.Deleteemployer(employer.EmployerID);

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
                    Detail = $"Unable to delete employer"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await v4_1Service.ExportemployersToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "user",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "employers");
            }

            if (args == null || args.Value == "xlsx")
            {
                await v4_1Service.ExportemployersToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "user",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "employers");
            }
        }
    }
}