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
    public partial class EditApplication
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

        [Parameter]
        public int ApplicationID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            application = await mainService.GetapplicationByApplicationId(ApplicationID);

            jobsForJobId = await mainService.Getjobs();

            supportrolesForEmployeeId = await mainService.Getsupportroles();

            rolesForEmployeeId = await mainService.Getroles();

            employeesForEmployeeId = await mainService.Getemployees();
        }
        protected bool errorVisible;
        protected PalyavalasztoV2.Models.main.application application;

        protected IEnumerable<PalyavalasztoV2.Models.main.job> jobsForJobId;

        protected IEnumerable<PalyavalasztoV2.Models.main.supportrole> supportrolesForEmployeeId;

        protected IEnumerable<PalyavalasztoV2.Models.main.role> rolesForEmployeeId;

        protected IEnumerable<PalyavalasztoV2.Models.main.employee> employeesForEmployeeId;

        protected async Task FormSubmit()
        {
            try
            {
                await mainService.Updateapplication(ApplicationID, application);
                DialogService.Close(application);
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
        bool hasJobIdValue;

        [Parameter]
        public int EmployeeId { get; set; }

        [Parameter]
        public int JobId { get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            application = new PalyavalasztoV2.Models.main.application();

            hasEmployeeIdValue = parameters.TryGetValue<int>("EmployeeId", out var hasEmployeeIdResult);
            if (hasEmployeeIdValue)
            {
                application.EmployeeId = hasEmployeeIdResult;
            }

            hasJobIdValue = parameters.TryGetValue<int>("JobId", out var hasJobIdResult);
            if (hasJobIdValue)
            {
                application.JobId = hasJobIdResult;
            }

            await base.SetParametersAsync(parameters);
        }
    }
}