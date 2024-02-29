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
        public mainService mainService { get; set; }

        protected IEnumerable<PalyavalsztoV3.Models.main.employer> employers;

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.employer> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            employers = await mainService.Getemployers(new Query { Filter = $@"i => i.Name.Contains(@0) || i.Description.Contains(@0) || i.PasswordHash.Contains(@0)", FilterParameters = new object[] { search } });
        }
        protected override async Task OnInitializedAsync()
        {
            employers = await mainService.Getemployers(new Query { Filter = $@"i => i.Name.Contains(@0) || i.Description.Contains(@0) || i.PasswordHash.Contains(@0)", FilterParameters = new object[] { search } });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddEmployer>("Add employer", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PalyavalsztoV3.Models.main.employer> args)
        {
            await DialogService.OpenAsync<EditEmployer>("Edit employer", new Dictionary<string, object> { {"id", args.Data.id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.employer employer)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await mainService.Deleteemployer(employer.id);

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
                await mainService.ExportemployersToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "employers");
            }

            if (args == null || args.Value == "xlsx")
            {
                await mainService.ExportemployersToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "employers");
            }
        }

        protected PalyavalsztoV3.Models.main.employer employer;
        protected async Task GetChildData(PalyavalsztoV3.Models.main.employer args)
        {
            employer = args;
            var employeesResult = await mainService.Getemployees(new Query { Filter = $@"i => ", Expand = "employer,userrole,adminrole,role,location" });
            if (employeesResult != null)
            {
                args.employees = employeesResult.ToList();
            }
            var jobsResult = await mainService.Getjobs(new Query { Filter = $@"i => i.EmployerId == {args.id}", Expand = "employer" });
            if (jobsResult != null)
            {
                args.jobs = jobsResult.ToList();
            }
            var usersResult = await mainService.Getusers(new Query { Filter = $@"i => ", Expand = "employee,employer" });
            if (usersResult != null)
            {
                args.users = usersResult.ToList();
            }
        }

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.employee> employeesDataGrid;

        protected async Task employeesAddButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.employer data)
        {
            var dialogResult = await DialogService.OpenAsync<Addemployee>("Add employees", new Dictionary<string, object> { {"EmployeeId" , data.UserId} });
            await GetChildData(data);
            await employeesDataGrid.Reload();
        }

        protected async Task employeesRowSelect(PalyavalsztoV3.Models.main.employee args, PalyavalsztoV3.Models.main.employer data)
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

                    await GetChildData(employer);

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

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.job> jobsDataGrid;

        protected async Task jobsAddButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.employer data)
        {
            var dialogResult = await DialogService.OpenAsync<Addjob>("Add jobs", new Dictionary<string, object> { {"EmployerId" , data.id} });
            await GetChildData(data);
            await jobsDataGrid.Reload();
        }

        protected async Task jobsRowSelect(PalyavalsztoV3.Models.main.job args, PalyavalsztoV3.Models.main.employer data)
        {
            var dialogResult = await DialogService.OpenAsync<Editjob>("Edit jobs", new Dictionary<string, object> { {"JobID", args.JobID} });
            await GetChildData(data);
            await jobsDataGrid.Reload();
        }

        protected async Task jobsDeleteButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.job job)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await mainService.Deletejob(job.JobID);

                    await GetChildData(employer);

                    if (deleteResult != null)
                    {
                        await jobsDataGrid.Reload();
                    }
                }
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete job"
                });
            }
        }

        protected RadzenDataGrid<PalyavalsztoV3.Models.main.user> usersDataGrid;

        protected async Task usersAddButtonClick(MouseEventArgs args, PalyavalsztoV3.Models.main.employer data)
        {
            var dialogResult = await DialogService.OpenAsync<Adduser>("Add users", new Dictionary<string, object> { {"employee_id" , data.EmployeeId} });
            await GetChildData(data);
            await usersDataGrid.Reload();
        }

        protected async Task usersRowSelect(PalyavalsztoV3.Models.main.user args, PalyavalsztoV3.Models.main.employer data)
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

                    await GetChildData(employer);

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
        protected async void Grid0Render(DataGridRenderEventArgs<PalyavalsztoV3.Models.main.employer> args)
        {
            if (grid0.Query.Filter != lastFilter)
            {
                employer = grid0.View.FirstOrDefault();
            }

            if (grid0.Query.Filter != lastFilter && employer != null)
            {
                await grid0.SelectRow(employer);
            }

            lastFilter = grid0.Query.Filter;
        }
    }
}