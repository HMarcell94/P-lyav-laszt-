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
    public partial class AddEmployer
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
            employer = new PalyavalasztoV2.Models.main.employer();
        }
        protected bool errorVisible;
        protected PalyavalasztoV2.Models.main.employer employer;

        protected async Task FormSubmit()
        {
            try
            {
                await mainService.Createemployer(employer);
                DialogService.Close(employer);
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