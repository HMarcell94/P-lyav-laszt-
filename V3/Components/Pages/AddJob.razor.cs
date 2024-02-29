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
    public partial class AddJob
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

            employersForEmployerId = await mainService.Getemployers();
        }
        protected bool errorVisible;
        protected PalyavalsztoV3.Models.main.job job;

        protected IEnumerable<PalyavalsztoV3.Models.main.employer> employersForEmployerId;

        protected async Task FormSubmit()
        {
            try
            {
                await mainService.Createjob(job);
                DialogService.Close(job);
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





        bool hasEmployerIdValue;

        [Parameter]
        public int EmployerId { get; set; }
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            job = new PalyavalsztoV3.Models.main.job();

            hasEmployerIdValue = parameters.TryGetValue<int>("EmployerId", out var hasEmployerIdResult);

            if (hasEmployerIdValue)
            {
                job.EmployerId = hasEmployerIdResult;
            }
            await base.SetParametersAsync(parameters);
        }
    }
}