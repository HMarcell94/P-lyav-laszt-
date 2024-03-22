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
    public partial class Locations
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

        protected IEnumerable<PalyavalasztoV2.Models.main.location> locations;

        protected RadzenDataGrid<PalyavalasztoV2.Models.main.location> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            locations = await mainService.Getlocations(new Query { Filter = $@"i => i.Address.Contains(@0)", FilterParameters = new object[] { search } });
        }
        protected override async Task OnInitializedAsync()
        {
            locations = await mainService.Getlocations(new Query { Filter = $@"i => i.Address.Contains(@0)", FilterParameters = new object[] { search } });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddLocation>("Add location", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PalyavalasztoV2.Models.main.location> args)
        {
            await DialogService.OpenAsync<EditLocation>("Edit location", new Dictionary<string, object> { {"LocationId", args.Data.LocationId} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PalyavalasztoV2.Models.main.location location)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await mainService.Deletelocation(location.LocationId);

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
                    Detail = $"Unable to delete location"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await mainService.ExportlocationsToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "locations");
            }

            if (args == null || args.Value == "xlsx")
            {
                await mainService.ExportlocationsToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "locations");
            }
        }

        protected PalyavalasztoV2.Models.main.location location;
        protected async Task GetChildData(PalyavalasztoV2.Models.main.location args)
        {
            location = args;
            var employeesResult = await mainService.Getemployees(new Query { Filter = $@"i => ", Expand = "employer,userrole,adminrole,role,location" });
            if (employeesResult != null)
            {
                args.employees = employeesResult.ToList();
            }
        }

        protected RadzenDataGrid<PalyavalasztoV2.Models.main.employee> employeesDataGrid;

        protected async Task employeesAddButtonClick(MouseEventArgs args, PalyavalasztoV2.Models.main.location data)
        {
            var dialogResult = await DialogService.OpenAsync<AddEmployee>("Add employees", new Dictionary<string, object> { {"EmployeeId" , data.employees} });
            await GetChildData(data);
            await employeesDataGrid.Reload();
        }

        protected async Task employeesRowSelect(DataGridRowMouseEventArgs<PalyavalasztoV2.Models.main.employee> args, PalyavalasztoV2.Models.main.location data)
        {
            var dialogResult = await DialogService.OpenAsync<EditEmployee>("Edit employees", new Dictionary<string, object> { {"EmployeeId", args.Data.EmployeeId} });
            await GetChildData(data);
            await employeesDataGrid.Reload();
        }

        protected async Task employeesDeleteButtonClick(MouseEventArgs args, PalyavalasztoV2.Models.main.employee employee)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await mainService.Deleteemployee(employee.EmployeeId);

                    await GetChildData(location);

                    if (deleteResult != null)
                    {
                        await employeesDataGrid.Reload();
                    }
                }
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete employee"
                });
            }
        }
    }
}