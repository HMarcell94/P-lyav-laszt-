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
    public partial class EditSupportrole
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
        public int userID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            supportrole = await mainService.GetsupportroleByUserId(userID);

            rolesForRoleID = await mainService.Getroles();
        }
        protected bool errorVisible;
        protected PalyavalasztoV2.Models.main.supportrole supportrole;

        protected IEnumerable<PalyavalasztoV2.Models.main.role> rolesForRoleID;

        protected async Task FormSubmit()
        {
            try
            {
                await mainService.Updatesupportrole(userID, supportrole);
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
            supportrole = new PalyavalasztoV2.Models.main.supportrole();

            hasRoleIDValue = parameters.TryGetValue<int>("RoleID", out var hasRoleIDResult);

            if (hasRoleIDValue)
            {
                supportrole.RoleID = hasRoleIDResult;
            }
            await base.SetParametersAsync(parameters);
        }
    }
}