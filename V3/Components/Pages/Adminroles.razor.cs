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
    public partial class Adminroles
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

        protected IEnumerable<PalyavalsztoV3.Models.main.adminrole> adminroles;

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.adminrole> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            adminroles = await mainService.Getadminroles();
        }
        protected override async Task OnInitializedAsync()
        {
            adminroles = await mainService.Getadminroles();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddAdminrole>("Add adminrole", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PalyavalsztoV3.Models.main.adminrole> args)
        {
            await DialogService.OpenAsync<EditAdminrole>("Edit adminrole", new Dictionary<string, object> { {"UserId", args.Data.UserId} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.adminrole adminrole)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await mainService.Deleteadminrole(adminrole.UserId);

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
                    Detail = $"Unable to delete adminrole"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await mainService.ExportadminrolesToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "adminroles");
            }

            if (args == null || args.Value == "xlsx")
            {
                await mainService.ExportadminrolesToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "adminroles");
            }
        }

        protected PalyavalsztoV3.Models.main.adminrole adminrole;
        protected async Task GetChildData(PalyavalsztoV3.Models.main.adminrole args)
        {
            adminrole = args;
            var employeesResult = await mainService.Getemployees(new Query { Filter = $@"i => i.EmployeeId == {args.UserId}", Expand = "employer,userrole,adminrole,role,location" });
            if (employeesResult != null)
            {
                args.employees = employeesResult.ToList();
            }
        }

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.employee> employeesDataGrid;

        protected async Task employeesAddButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.adminrole data)
        {
            var dialogResult = await DialogService.OpenAsync<Addemployee>("Add employees", new Dictionary<string, object> { {"EmployeeId" , data.UserId} });
            await GetChildData(data);
            await employeesDataGrid.Reload();
        }

        protected async Task employeesRowSelect(PalyavalsztoV3.Models.main.employee args, PalyavalsztoV3.Models.main.adminrole data)
        {
            var dialogResult = await DialogService.OpenAsync<Editemployee>("Edit employees", new Dictionary<string, object> { {"EmployeeId", args.EmployeeId} });
            await GetChildData(data);
            await employeesDataGrid.Reload();
        }

        protected async Task employeesDeleteButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.employee employee)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await mainService.Deleteemployee(employee.EmployeeId);

                    await GetChildData(adminrole);

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

        string lastFilter;
        protected async void Grid0Render(DataGridRenderEventArgs<PalyavalsztoV3.Models.main.adminrole> args)
        {
            if (grid0.Query.Filter != lastFilter)
            {
                adminrole = grid0.View.FirstOrDefault();
            }

            if (grid0.Query.Filter != lastFilter && adminrole != null)
            {
                await grid0.SelectRow(adminrole);
            }

            lastFilter = grid0.Query.Filter;
        }
    }
}