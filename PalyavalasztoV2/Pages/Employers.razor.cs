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

        protected IEnumerable<PalyavalasztoV2.Models.main.Employer> employers;

        protected RadzenDataGrid<PalyavalasztoV2.Models.main.Employer> grid0;

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

        protected async Task EditRow(DataGridRowMouseEventArgs<PalyavalasztoV2.Models.main.Employer> args)
        {
            await DialogService.OpenAsync<EditEmployer>("Edit employer", new Dictionary<string, object> { {"id", args.Data.id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PalyavalasztoV2.Models.main.Employer employer)
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

        protected PalyavalasztoV2.Models.main.Employer employer;
        protected async Task GetChildData(PalyavalasztoV2.Models.main.Employer args)
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
        }

        protected RadzenDataGrid<PalyavalasztoV2.Models.main.Employee> employeesDataGrid;

        protected async Task employeesAddButtonClick(MouseEventArgs args, PalyavalasztoV2.Models.main.Employer data)
        {
            var dialogResult = await DialogService.OpenAsync<AddEmployee>("Add employees", new Dictionary<string, object> { {"EmployeeId" , data.employees} });
            await GetChildData(data);
            await employeesDataGrid.Reload();
        }

        protected async Task employeesRowSelect(DataGridRowMouseEventArgs<PalyavalasztoV2.Models.main.Employee> args, PalyavalasztoV2.Models.main.Employer data)
        {
            var dialogResult = await DialogService.OpenAsync<EditEmployee>("Edit employees", new Dictionary<string, object> { {"EmployeeId", args.Data.EmployeeId} });
            await GetChildData(data);
            await employeesDataGrid.Reload();
        }

        protected async Task employeesDeleteButtonClick(MouseEventArgs args, PalyavalasztoV2.Models.main.Employee employee)
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

        protected RadzenDataGrid<PalyavalasztoV2.Models.main.Job> jobsDataGrid;

        protected async Task jobsAddButtonClick(MouseEventArgs args, PalyavalasztoV2.Models.main.Employer data)
        {
            var dialogResult = await DialogService.OpenAsync<AddJob>("Add jobs", new Dictionary<string, object> { {"EmployerId" , data.id} });
            await GetChildData(data);
            await jobsDataGrid.Reload();
        }

        protected async Task jobsRowSelect(DataGridRowMouseEventArgs<PalyavalasztoV2.Models.main.Job> args, PalyavalasztoV2.Models.main.Employer data)
        {
            var dialogResult = await DialogService.OpenAsync<EditJob>("Edit jobs", new Dictionary<string, object> { {"JobID", args.Data.JobID} });
            await GetChildData(data);
            await jobsDataGrid.Reload();
        }

        protected async Task jobsDeleteButtonClick(MouseEventArgs args, PalyavalasztoV2.Models.main.Job job)
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
    }
}