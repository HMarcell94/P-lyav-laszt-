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
    public partial class AddEmployee
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

            employersForEmployeeId = await mainService.Getemployers();

            userrolesForEmployeeId = await mainService.Getuserroles();

            adminrolesForEmployeeId = await mainService.Getadminroles();

            rolesForRoleId = await mainService.Getroles();

            locationsForLocationId = await mainService.Getlocations();
        }
        protected bool errorVisible;
        protected PalyavalasztoV2.Models.main.employee employee;

        protected IEnumerable<PalyavalasztoV2.Models.main.employer> employersForEmployeeId;

        protected IEnumerable<PalyavalasztoV2.Models.main.userrole> userrolesForEmployeeId;

        protected IEnumerable<PalyavalasztoV2.Models.main.adminrole> adminrolesForEmployeeId;

        protected IEnumerable<PalyavalasztoV2.Models.main.role> rolesForRoleId;

        protected IEnumerable<PalyavalasztoV2.Models.main.location> locationsForLocationId;

        protected async Task FormSubmit()
        {
            try
            {
                await mainService.Createemployee(employee);
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
        bool hasLocationIdValue;
        bool hasRoleIdValue;

        [Parameter]
        public int EmployeeId { get; set; }

        [Parameter]
        public int? LocationId { get; set; }

        [Parameter]
        public int RoleId { get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            employee = new PalyavalasztoV2.Models.main.employee();

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
