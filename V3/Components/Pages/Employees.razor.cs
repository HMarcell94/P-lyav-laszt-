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
    public partial class Employees
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

        protected IEnumerable<PalyavalsztoV3.Models.main.employee> employees;

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.employee> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            employees = await mainService.Getemployees(new Query { Filter = $@"i => i.Name.Contains(@0) || i.Skills.Contains(@0) || i.PasswordHash.Contains(@0)", FilterParameters = new object[] { search }, Expand = "employer,userrole,adminrole,role,location" });
        }
        protected override async Task OnInitializedAsync()
        {
            employees = await mainService.Getemployees(new Query { Filter = $@"i => i.Name.Contains(@0) || i.Skills.Contains(@0) || i.PasswordHash.Contains(@0)", FilterParameters = new object[] { search }, Expand = "employer,userrole,adminrole,role,location" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddEmployee>("Add employee", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PalyavalsztoV3.Models.main.employee> args)
        {
            await DialogService.OpenAsync<EditEmployee>("Edit employee", new Dictionary<string, object> { {"EmployeeId", args.Data.EmployeeId} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.employee employee)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await mainService.Deleteemployee(employee.EmployeeId);

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
                    Detail = $"Unable to delete employee"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await mainService.ExportemployeesToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "employer,userrole,adminrole,role,location",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "employees");
            }

            if (args == null || args.Value == "xlsx")
            {
                await mainService.ExportemployeesToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "employer,userrole,adminrole,role,location",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "employees");
            }
        }

        protected PalyavalsztoV3.Models.main.employee employee;
        protected async Task GetChildData(PalyavalsztoV3.Models.main.employee args)
        {
            employee = args;
            var applicationsResult = await mainService.Getapplications(new Query { Filter = $@"i => i.EmployeeId == {args.EmployeeId}", Expand = "job,supportrole,role,employee" });
            if (applicationsResult != null)
            {
                args.applications = applicationsResult.ToList();
            }
            var usersResult = await mainService.Getusers(new Query { Filter = $@"i => i.employee_id == {args.EmployeeId}", Expand = "employee,employer" });
            if (usersResult != null)
            {
                args.users = usersResult.ToList();
            }
        }

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.application> applicationsDataGrid;

        protected async Task applicationsAddButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.employee data)
        {
            var dialogResult = await DialogService.OpenAsync<Addapplication>("Add applications", new Dictionary<string, object> { {"EmployeeId" , data.EmployeeId} });
            await GetChildData(data);
            await applicationsDataGrid.Reload();
        }

        protected async Task applicationsRowSelect(PalyavalsztoV3.Models.main.application args, PalyavalsztoV3.Models.main.employee data)
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

                    await GetChildData(employee);

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

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.user> usersDataGrid;

        protected async Task usersAddButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.employee data)
        {
            var dialogResult = await DialogService.OpenAsync<Adduser>("Add users", new Dictionary<string, object> { {"employee_id" , data.EmployeeId} });
            await GetChildData(data);
            await usersDataGrid.Reload();
        }

        protected async Task usersRowSelect(PalyavalsztoV3.Models.main.user args, PalyavalsztoV3.Models.main.employee data)
        {
            var dialogResult = await DialogService.OpenAsync<Edituser>("Edit users", new Dictionary<string, object> { {"id", args.id} });
            await GetChildData(data);
            await usersDataGrid.Reload();
        }

        protected async Task usersDeleteButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.user user)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await mainService.Deleteuser(user.id);

                    await GetChildData(employee);

                    if (deleteResult != null)
                    {
                        await usersDataGrid.Reload();
                    }
                }
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete user"
                });
            }
        }

        string lastFilter;
        protected async void Grid0Render(DataGridRenderEventArgs<PalyavalsztoV3.Models.main.employee> args)
        {
            if (grid0.Query.Filter != lastFilter)
            {
                employee = grid0.View.FirstOrDefault();
            }

            if (grid0.Query.Filter != lastFilter && employee != null)
            {
                await grid0.SelectRow(employee);
            }

            lastFilter = grid0.Query.Filter;
        }
    }
}