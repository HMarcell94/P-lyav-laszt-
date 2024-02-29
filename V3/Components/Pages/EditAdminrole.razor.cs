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
    public partial class EditAdminrole
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
        public int UserId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            adminrole = await mainService.GetadminroleByUserId(UserId);
        }
        protected bool errorVisible;
        protected PalyavalsztoV3.Models.main.adminrole adminrole;

        protected async Task FormSubmit()
        {
            try
            {
                await mainService.Updateadminrole(UserId, adminrole);
                DialogService.Close(adminrole);
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
    }
}