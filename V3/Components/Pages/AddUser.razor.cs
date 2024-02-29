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
    public partial class AddUser
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

            employeesForemployeeId = await mainService.Getemployees();

            employersForemployeerId = await mainService.Getemployers();
        }
        protected bool errorVisible;
        protected PalyavalsztoV3.Models.main.user user;

        protected IEnumerable<PalyavalsztoV3.Models.main.employee> employeesForemployeeId;

        protected IEnumerable<PalyavalsztoV3.Models.main.employer> employersForemployeerId;

        protected async Task FormSubmit()
        {
            try
            {
                await mainService.Createuser(user);
                DialogService.Close(user);
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





        bool hasemployee_idValue;

        [Parameter]
        public int employee_id { get; set; }

        bool hasemployeer_idValue;

        [Parameter]
        public int employeer_id { get; set; }
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            user = new PalyavalsztoV3.Models.main.user();

            hasemployee_idValue = parameters.TryGetValue<int>("employee_id", out var hasemployee_idResult);

            if (hasemployee_idValue)
            {
                user.employee_id = hasemployee_idResult;
            }

            hasemployeer_idValue = parameters.TryGetValue<int>("employeer_id", out var hasemployeer_idResult);

            if (hasemployeer_idValue)
            {
                user.employeer_id = hasemployeer_idResult;
            }
            await base.SetParametersAsync(parameters);
        }
    }
}