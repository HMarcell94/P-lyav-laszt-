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
    public partial class Roles
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

        protected IEnumerable<PalyavalsztoV3.Models.main.role> roles;

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.role> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            roles = await mainService.Getroles(new Query { Filter = $@"i => i.RoleName.Contains(@0)", FilterParameters = new object[] { search } });
        }
        protected override async Task OnInitializedAsync()
        {
            roles = await mainService.Getroles(new Query { Filter = $@"i => i.RoleName.Contains(@0)", FilterParameters = new object[] { search } });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddRole>("Add role", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PalyavalsztoV3.Models.main.role> args)
        {
            await DialogService.OpenAsync<EditRole>("Edit role", new Dictionary<string, object> { {"RoleID", args.Data.RoleID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.role role)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await mainService.Deleterole(role.RoleID);

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
                    Detail = $"Unable to delete role"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await mainService.ExportrolesToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "roles");
            }

            if (args == null || args.Value == "xlsx")
            {
                await mainService.ExportrolesToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "roles");
            }
        }

        protected PalyavalsztoV3.Models.main.role role;
        protected async Task GetChildData(PalyavalsztoV3.Models.main.role args)
        {
            role = args;
            var applicationsResult = await mainService.Getapplications(new Query { Filter = $@"i => ", Expand = "job,supportrole,role,employee" });
            if (applicationsResult != null)
            {
                args.applications = applicationsResult.ToList();
            }
            var employeesResult = await mainService.Getemployees(new Query { Filter = $@"i => ", Expand = "employer,userrole,adminrole,role,location" });
            if (employeesResult != null)
            {
                args.employees = employeesResult.ToList();
            }
            var supportrolesResult = await mainService.Getsupportroles(new Query { Filter = $@"i => i.RoleID == {args.RoleID}", Expand = "role" });
            if (supportrolesResult != null)
            {
                args.supportroles = supportrolesResult.ToList();
            }
        }

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.application> applicationsDataGrid;

        protected async Task applicationsAddButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.role data)
        {
            var dialogResult = await DialogService.OpenAsync<Addapplication>("Add applications", new Dictionary<string, object> { {"EmployeeId" , data.EmployeeId} });
            await GetChildData(data);
            await applicationsDataGrid.Reload();
        }

        protected async Task applicationsRowSelect(PalyavalsztoV3.Models.main.application args, PalyavalsztoV3.Models.main.role data)
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

                    await GetChildData(role);

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

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.employee> employeesDataGrid;

        protected async Task employeesAddButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.role data)
        {
            var dialogResult = await DialogService.OpenAsync<Addemployee>("Add employees", new Dictionary<string, object> { {"EmployeeId" , data.UserId} });
            await GetChildData(data);
            await employeesDataGrid.Reload();
        }

        protected async Task employeesRowSelect(PalyavalsztoV3.Models.main.employee args, PalyavalsztoV3.Models.main.role data)
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

                    await GetChildData(role);

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

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.supportrole> supportrolesDataGrid;

        protected async Task supportrolesAddButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.role data)
        {
            var dialogResult = await DialogService.OpenAsync<Addsupportrole>("Add supportroles", new Dictionary<string, object> { {"RoleID" , data.RoleID} });
            await GetChildData(data);
            await supportrolesDataGrid.Reload();
        }

        protected async Task supportrolesRowSelect(PalyavalsztoV3.Models.main.supportrole args, PalyavalsztoV3.Models.main.role data)
        {
            var dialogResult = await DialogService.OpenAsync<Editsupportrole>("Edit supportroles", new Dictionary<string, object> { {"userID", args.userID} });
            await GetChildData(data);
            await supportrolesDataGrid.Reload();
        }

        protected async Task supportrolesDeleteButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.supportrole supportrole)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await mainService.Deletesupportrole(supportrole.userID);

                    await GetChildData(role);

                    if (deleteResult != null)
                    {
                        await supportrolesDataGrid.Reload();
                    }
                }
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete supportrole"
                });
            }
        }

        string lastFilter;
        protected async void Grid0Render(DataGridRenderEventArgs<PalyavalsztoV3.Models.main.role> args)
        {
            if (grid0.Query.Filter != lastFilter)
            {
                role = grid0.View.FirstOrDefault();
            }

            if (grid0.Query.Filter != lastFilter && role != null)
            {
                await grid0.SelectRow(role);
            }

            lastFilter = grid0.Query.Filter;
        }
    }
}