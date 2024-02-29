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
    public partial class AddSupportrole
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

            rolesForRoleID = await mainService.Getroles();
        }
        protected bool errorVisible;
        protected PalyavalsztoV3.Models.main.supportrole supportrole;

        protected IEnumerable<PalyavalsztoV3.Models.main.role> rolesForRoleID;

        protected async Task FormSubmit()
        {
            try
            {
                await mainService.Createsupportrole(supportrole);
                DialogService.Close(supportrole);
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





        bool hasRoleIDValue;

        [Parameter]
        public int RoleID { get; set; }
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            supportrole = new PalyavalsztoV3.Models.main.supportrole();

            hasRoleIDValue = parameters.TryGetValue<int>("RoleID", out var hasRoleIDResult);

            if (hasRoleIDValue)
            {
                supportrole.RoleID = hasRoleIDResult;
            }
            await base.SetParametersAsync(parameters);
        }
    }
}