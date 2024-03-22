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
    public partial class EditEmployee
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

        protected override async Task OnInitializedAsync()
        {
            employee = await mainService.GetemployeeByEmployeeId(EmployeeId);

            employersForEmployeeId = await mainService.Getemployers();

            userrolesForEmployeeId = await mainService.Getuserroles();

            adminrolesForEmployeeId = await mainService.Getadminroles();

            rolesForRoleId = await mainService.Getroles();

            locationsForLocationId = await mainService.Getlocations();
        }
        protected bool errorVisible;
        protected PalyavalsztoV3.Models.main.employee employee;

        protected IEnumerable<PalyavalsztoV3.Models.main.employer> employersForEmployeeId;

        protected IEnumerable<PalyavalsztoV3.Models.main.userrole> userrolesForEmployeeId;

        protected IEnumerable<PalyavalsztoV3.Models.main.adminrole> adminrolesForEmployeeId;

        protected IEnumerable<PalyavalsztoV3.Models.main.role> rolesForRoleId;

        protected IEnumerable<PalyavalsztoV3.Models.main.location> locationsForLocationId;

        protected async Task FormSubmit()
        {
            try
            {
                await mainService.Updateemployee(EmployeeId, employee);
                DialogService.Close(employee);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }





        bool hasEmployeeIdValue;

        [Parameter]
        public int EmployeeId { get; set; }

        bool hasEmployeeIdValue;

        [Parameter]
        public int EmployeeId { get; set; }

        bool hasEmployeeIdValue;

        [Parameter]
        public int EmployeeId { get; set; }

        bool hasLocationIdValue;

        [Parameter]
        public int? LocationId { get; set; }

        bool hasRoleIdValue;

        [Parameter]
        public int RoleId { get; set; }
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            employee = new PalyavalsztoV3.Models.main.employee();

            hasEmployeeIdValue = parameters.TryGetValue<int>("EmployeeId", out var hasEmployeeIdResult);

            if (hasEmployeeIdValue)
            {
                employee.EmployeeId = hasEmployeeIdResult;
            }

            hasEmployeeIdValue = parameters.TryGetValue<int>("EmployeeId", out var hasEmployeeIdResult);

            if (hasEmployeeIdValue)
            {
                employee.EmployeeId = hasEmployeeIdResult;
            }

            hasEmployeeIdValue = parameters.TryGetValue<int>("EmployeeId", out var hasEmployeeIdResult);

            if (hasEmployeeIdValue)
            {
                employee.EmployeeId = hasEmployeeIdResult;
            }

            hasLocationIdValue = parameters.TryGetValue<int?>("LocationId", out var hasLocationIdResult);

            if (hasLocationIdValue)
            {
                employee.LocationId = hasLocationIdResult;
            }

            hasRoleIdValue = parameters.TryGetValue<int>("RoleId", out var hasRoleIdResult);

            if (hasRoleIdValue)
            {
                employee.RoleId = hasRoleIdResult;
            }
            await base.SetParametersAsync(parameters);
        }
    }
}