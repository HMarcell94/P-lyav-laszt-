using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace PalyavalsztoV3.Components.Pages
{
    public partial class Supportroles
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

        protected IEnumerable<PalyavalsztoV3.Models.main.supportrole> supportroles;

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.supportrole> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            supportroles = await mainService.Getsupportroles(new Query { Expand = "role" });
        }
        protected override async Task OnInitializedAsync()
        {
            supportroles = await mainService.Getsupportroles(new Query { Expand = "role" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddSupportrole>("Add supportrole", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PalyavalsztoV3.Models.main.supportrole> args)
        {
            await DialogService.OpenAsync<EditSupportrole>("Edit supportrole", new Dictionary<string, object> { {"userID", args.Data.userID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.supportrole supportrole)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await mainService.Deletesupportrole(supportrole.userID);

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
                    Detail = $"Unable to delete supportrole"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await mainService.ExportsupportrolesToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "role",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "supportroles");
            }

            if (args == null || args.Value == "xlsx")
            {
                await mainService.ExportsupportrolesToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "role",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "supportroles");
            }
        }

        protected PalyavalsztoV3.Models.main.supportrole supportrole;
        protected async Task GetChildData(PalyavalsztoV3.Models.main.supportrole args)
        {
            supportrole = args;
            var applicationsResult = await mainService.Getapplications(new Query { Filter = $@"i => ", Expand = "job,supportrole,role,employee" });
            if (applicationsResult != null)
            {
                args.applications = applicationsResult.ToList();
            }
        }

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.application> applicationsDataGrid;

        protected async Task applicationsAddButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.supportrole data)
        {
            var dialogResult = await DialogService.OpenAsync<Addapplication>("Add applications", new Dictionary<string, object> { {"EmployeeId" , data.EmployeeId} });
            await GetChildData(data);
            await applicationsDataGrid.Reload();
        }

        protected async Task applicationsRowSelect(PalyavalsztoV3.Models.main.application args, PalyavalsztoV3.Models.main.supportrole data)
        {
            var dialogResult = await DialogService.OpenAsync<Editapplication>("Edit applications", new Dictionary<string, object> { {"ApplicationID", args.ApplicationID} });
            await GetChildData(data);
            await applicationsDataGrid.Reload();
        }

        protected async Task applicationsDeleteButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.application application)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await mainService.Deleteapplication(application.ApplicationID);

                    await GetChildData(supportrole);

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

        string lastFilter;
        protected async void Grid0Render(DataGridRenderEventArgs<PalyavalsztoV3.Models.main.supportrole> args)
        {
            if (grid0.Query.Filter != lastFilter)
            {
                supportrole = grid0.View.FirstOrDefault();
            }

            if (grid0.Query.Filter != lastFilter && supportrole != null)
            {
                await grid0.SelectRow(supportrole);
            }

            lastFilter = grid0.Query.Filter;
        }
    }
}