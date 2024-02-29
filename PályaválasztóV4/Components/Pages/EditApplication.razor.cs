using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace PalyavalsztoV4.Components.Pages
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
        public v4_1Service v4_1Service { get; set; }

        [Parameter]
        public int ApplicationID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            application = await v4_1Service.GetapplicationByApplicationId(ApplicationID);

            jobsForJobID = await v4_1Service.Getjobs();

            employeesForEmployeeID = await v4_1Service.Getemployees();
        }
        protected bool errorVisible;
        protected PalyavalsztoV4.Models.v4_1.application application;

        protected IEnumerable<PalyavalsztoV4.Models.v4_1.job> jobsForJobID;

        protected IEnumerable<PalyavalsztoV4.Models.v4_1.employee> employeesForEmployeeID;

        protected async Task FormSubmit()
        {
            try
            {
                await v4_1Service.Updateapplication(ApplicationID, application);
                DialogService.Close(application);
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                canEdit = !(ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException);
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
           v4_1Service.Reset();
            hasChanges = false;
            canEdit = true;

            application = await v4_1Service.GetapplicationByApplicationId(ApplicationID);
        }
    }
}