using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

using PalyavalasztoV2.Data;

namespace PalyavalasztoV2
{
    public partial class mainService
    {
        mainContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly mainContext context;
        private readonly NavigationManager navigationManager;

        public mainService(mainContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportadminrolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/adminroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/adminroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportadminrolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/adminroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/adminroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnadminrolesRead(ref IQueryable<PalyavalasztoV2.Models.main.adminrole> items);

        public async Task<IQueryable<PalyavalasztoV2.Models.main.adminrole>> Getadminroles(Query query = null)
        {
            var items = Context.adminroles.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnadminrolesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnadminroleGet(PalyavalasztoV2.Models.main.adminrole item);
        partial void OnGetadminroleByUserId(ref IQueryable<PalyavalasztoV2.Models.main.adminrole> items);


        public async Task<PalyavalasztoV2.Models.main.adminrole> GetadminroleByUserId(int userid)
        {
            var items = Context.adminroles
                              .AsNoTracking()
                              .Where(i => i.UserId == userid);

 
            OnGetadminroleByUserId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnadminroleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnadminroleCreated(PalyavalasztoV2.Models.main.adminrole item);
        partial void OnAfteradminroleCreated(PalyavalasztoV2.Models.main.adminrole item);

        public async Task<PalyavalasztoV2.Models.main.adminrole> Createadminrole(PalyavalasztoV2.Models.main.adminrole adminrole)
        {
            OnadminroleCreated(adminrole);

            var existingItem = Context.adminroles
                              .Where(i => i.UserId == adminrole.UserId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.adminroles.Add(adminrole);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adminrole).State = EntityState.Detached;
                throw;
            }

            OnAfteradminroleCreated(adminrole);

            return adminrole;
        }

        public async Task<PalyavalasztoV2.Models.main.adminrole> CanceladminroleChanges(PalyavalasztoV2.Models.main.adminrole item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnadminroleUpdated(PalyavalasztoV2.Models.main.adminrole item);
        partial void OnAfteradminroleUpdated(PalyavalasztoV2.Models.main.adminrole item);

        public async Task<PalyavalasztoV2.Models.main.adminrole> Updateadminrole(int userid, PalyavalasztoV2.Models.main.adminrole adminrole)
        {
            OnadminroleUpdated(adminrole);

            var itemToUpdate = Context.adminroles
                              .Where(i => i.UserId == adminrole.UserId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adminrole);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfteradminroleUpdated(adminrole);

            return adminrole;
        }

        partial void OnadminroleDeleted(PalyavalasztoV2.Models.main.adminrole item);
        partial void OnAfteradminroleDeleted(PalyavalasztoV2.Models.main.adminrole item);

        public async Task<PalyavalasztoV2.Models.main.adminrole> Deleteadminrole(int userid)
        {
            var itemToDelete = Context.adminroles
                              .Where(i => i.UserId == userid)
                              .Include(i => i.employees)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnadminroleDeleted(itemToDelete);


            Context.adminroles.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfteradminroleDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportapplicationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/applications/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/applications/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportapplicationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/applications/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/applications/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnapplicationsRead(ref IQueryable<PalyavalasztoV2.Models.main.application> items);

        public async Task<IQueryable<PalyavalasztoV2.Models.main.application>> Getapplications(Query query = null)
        {
            var items = Context.applications.AsQueryable();

            items = items.Include(i => i.employee);
            items = items.Include(i => i.role);
            items = items.Include(i => i.supportrole);
            items = items.Include(i => i.job);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnapplicationsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnapplicationGet(PalyavalasztoV2.Models.main.application item);
        partial void OnGetapplicationByApplicationId(ref IQueryable<PalyavalasztoV2.Models.main.application> items);


        public async Task<PalyavalasztoV2.Models.main.application> GetapplicationByApplicationId(int applicationid)
        {
            var items = Context.applications
                              .AsNoTracking()
                              .Where(i => i.ApplicationID == applicationid);

            items = items.Include(i => i.employee);
            items = items.Include(i => i.role);
            items = items.Include(i => i.supportrole);
            items = items.Include(i => i.job);
 
            OnGetapplicationByApplicationId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnapplicationGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnapplicationCreated(PalyavalasztoV2.Models.main.application item);
        partial void OnAfterapplicationCreated(PalyavalasztoV2.Models.main.application item);

        public async Task<PalyavalasztoV2.Models.main.application> Createapplication(PalyavalasztoV2.Models.main.application application)
        {
            OnapplicationCreated(application);

            var existingItem = Context.applications
                              .Where(i => i.ApplicationID == application.ApplicationID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.applications.Add(application);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(application).State = EntityState.Detached;
                throw;
            }

            OnAfterapplicationCreated(application);

            return application;
        }

        public async Task<PalyavalasztoV2.Models.main.application> CancelapplicationChanges(PalyavalasztoV2.Models.main.application item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnapplicationUpdated(PalyavalasztoV2.Models.main.application item);
        partial void OnAfterapplicationUpdated(PalyavalasztoV2.Models.main.application item);

        public async Task<PalyavalasztoV2.Models.main.application> Updateapplication(int applicationid, PalyavalasztoV2.Models.main.application application)
        {
            OnapplicationUpdated(application);

            var itemToUpdate = Context.applications
                              .Where(i => i.ApplicationID == application.ApplicationID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(application);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterapplicationUpdated(application);

            return application;
        }

        partial void OnapplicationDeleted(PalyavalasztoV2.Models.main.application item);
        partial void OnAfterapplicationDeleted(PalyavalasztoV2.Models.main.application item);

        public async Task<PalyavalasztoV2.Models.main.application> Deleteapplication(int applicationid)
        {
            var itemToDelete = Context.applications
                              .Where(i => i.ApplicationID == applicationid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnapplicationDeleted(itemToDelete);


            Context.applications.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterapplicationDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportemployeesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/employees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/employees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportemployeesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/employees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/employees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnemployeesRead(ref IQueryable<PalyavalasztoV2.Models.main.employee> items);

        public async Task<IQueryable<PalyavalasztoV2.Models.main.employee>> Getemployees(Query query = null)
        {
            var items = Context.employees.AsQueryable();

            items = items.Include(i => i.adminrole);
            items = items.Include(i => i.userrole);
            items = items.Include(i => i.employer);
            items = items.Include(i => i.location);
            items = items.Include(i => i.role);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnemployeesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnemployeeGet(PalyavalasztoV2.Models.main.employee item);
        partial void OnGetemployeeByEmployeeId(ref IQueryable<PalyavalasztoV2.Models.main.employee> items);


        public async Task<PalyavalasztoV2.Models.main.employee> GetemployeeByEmployeeId(int employeeid)
        {
            var items = Context.employees
                              .AsNoTracking()
                              .Where(i => i.EmployeeId == employeeid);

            items = items.Include(i => i.adminrole);
            items = items.Include(i => i.userrole);
            items = items.Include(i => i.employer);
            items = items.Include(i => i.location);
            items = items.Include(i => i.role);
 
            OnGetemployeeByEmployeeId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnemployeeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnemployeeCreated(PalyavalasztoV2.Models.main.employee item);
        partial void OnAfteremployeeCreated(PalyavalasztoV2.Models.main.employee item);

        public async Task<PalyavalasztoV2.Models.main.employee> Createemployee(PalyavalasztoV2.Models.main.employee employee)
        {
            OnemployeeCreated(employee);

            var existingItem = Context.employees
                              .Where(i => i.EmployeeId == employee.EmployeeId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.employees.Add(employee);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(employee).State = EntityState.Detached;
                throw;
            }

            OnAfteremployeeCreated(employee);

            return employee;
        }

        public async Task<PalyavalasztoV2.Models.main.employee> CancelemployeeChanges(PalyavalasztoV2.Models.main.employee item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnemployeeUpdated(PalyavalasztoV2.Models.main.employee item);
        partial void OnAfteremployeeUpdated(PalyavalasztoV2.Models.main.employee item);

        public async Task<PalyavalasztoV2.Models.main.employee> Updateemployee(int employeeid, PalyavalasztoV2.Models.main.employee employee)
        {
            OnemployeeUpdated(employee);

            var itemToUpdate = Context.employees
                              .Where(i => i.EmployeeId == employee.EmployeeId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(employee);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfteremployeeUpdated(employee);

            return employee;
        }

        partial void OnemployeeDeleted(PalyavalasztoV2.Models.main.employee item);
        partial void OnAfteremployeeDeleted(PalyavalasztoV2.Models.main.employee item);

        public async Task<PalyavalasztoV2.Models.main.employee> Deleteemployee(int employeeid)
        {
            var itemToDelete = Context.employees
                              .Where(i => i.EmployeeId == employeeid)
                              .Include(i => i.applications)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnemployeeDeleted(itemToDelete);


            Context.employees.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfteremployeeDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportemployersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/employers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/employers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportemployersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/employers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/employers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnemployersRead(ref IQueryable<PalyavalasztoV2.Models.main.employer> items);

        public async Task<IQueryable<PalyavalasztoV2.Models.main.employer>> Getemployers(Query query = null)
        {
            var items = Context.employers.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnemployersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnemployerGet(PalyavalasztoV2.Models.main.employer item);
        partial void OnGetemployerById(ref IQueryable<PalyavalasztoV2.Models.main.employer> items);


        public async Task<PalyavalasztoV2.Models.main.employer> GetemployerById(int id)
        {
            var items = Context.employers
                              .AsNoTracking()
                              .Where(i => i.id == id);

 
            OnGetemployerById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnemployerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnemployerCreated(PalyavalasztoV2.Models.main.employer item);
        partial void OnAfteremployerCreated(PalyavalasztoV2.Models.main.employer item);

        public async Task<PalyavalasztoV2.Models.main.employer> Createemployer(PalyavalasztoV2.Models.main.employer employer)
        {
            OnemployerCreated(employer);

            var existingItem = Context.employers
                              .Where(i => i.id == employer.id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.employers.Add(employer);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(employer).State = EntityState.Detached;
                throw;
            }

            OnAfteremployerCreated(employer);

            return employer;
        }

        public async Task<PalyavalasztoV2.Models.main.employer> CancelemployerChanges(PalyavalasztoV2.Models.main.employer item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnemployerUpdated(PalyavalasztoV2.Models.main.employer item);
        partial void OnAfteremployerUpdated(PalyavalasztoV2.Models.main.employer item);

        public async Task<PalyavalasztoV2.Models.main.employer> Updateemployer(int id, PalyavalasztoV2.Models.main.employer employer)
        {
            OnemployerUpdated(employer);

            var itemToUpdate = Context.employers
                              .Where(i => i.id == employer.id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(employer);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfteremployerUpdated(employer);

            return employer;
        }

        partial void OnemployerDeleted(PalyavalasztoV2.Models.main.employer item);
        partial void OnAfteremployerDeleted(PalyavalasztoV2.Models.main.employer item);

        public async Task<PalyavalasztoV2.Models.main.employer> Deleteemployer(int id)
        {
            var itemToDelete = Context.employers
                              .Where(i => i.id == id)
                              .Include(i => i.employees)
                              .Include(i => i.jobs)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnemployerDeleted(itemToDelete);


            Context.employers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfteremployerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportjobsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/jobs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/jobs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportjobsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/jobs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/jobs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnjobsRead(ref IQueryable<PalyavalasztoV2.Models.main.job> items);

        public async Task<IQueryable<PalyavalasztoV2.Models.main.job>> Getjobs(Query query = null)
        {
            var items = Context.jobs.AsQueryable();

            items = items.Include(i => i.employer);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnjobsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnjobGet(PalyavalasztoV2.Models.main.job item);
        partial void OnGetjobByJobId(ref IQueryable<PalyavalasztoV2.Models.main.job> items);


        public async Task<PalyavalasztoV2.Models.main.job> GetjobByJobId(int jobid)
        {
            var items = Context.jobs
                              .AsNoTracking()
                              .Where(i => i.JobID == jobid);

            items = items.Include(i => i.employer);
 
            OnGetjobByJobId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnjobGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnjobCreated(PalyavalasztoV2.Models.main.job item);
        partial void OnAfterjobCreated(PalyavalasztoV2.Models.main.job item);

        public async Task<PalyavalasztoV2.Models.main.job> Createjob(PalyavalasztoV2.Models.main.job job)
        {
            OnjobCreated(job);

            var existingItem = Context.jobs
                              .Where(i => i.JobID == job.JobID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.jobs.Add(job);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(job).State = EntityState.Detached;
                throw;
            }

            OnAfterjobCreated(job);

            return job;
        }

        public async Task<PalyavalasztoV2.Models.main.job> CanceljobChanges(PalyavalasztoV2.Models.main.job item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnjobUpdated(PalyavalasztoV2.Models.main.job item);
        partial void OnAfterjobUpdated(PalyavalasztoV2.Models.main.job item);

        public async Task<PalyavalasztoV2.Models.main.job> Updatejob(int jobid, PalyavalasztoV2.Models.main.job job)
        {
            OnjobUpdated(job);

            var itemToUpdate = Context.jobs
                              .Where(i => i.JobID == job.JobID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(job);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterjobUpdated(job);

            return job;
        }

        partial void OnjobDeleted(PalyavalasztoV2.Models.main.job item);
        partial void OnAfterjobDeleted(PalyavalasztoV2.Models.main.job item);

        public async Task<PalyavalasztoV2.Models.main.job> Deletejob(int jobid)
        {
            var itemToDelete = Context.jobs
                              .Where(i => i.JobID == jobid)
                              .Include(i => i.applications)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnjobDeleted(itemToDelete);


            Context.jobs.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterjobDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportlocationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/locations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/locations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportlocationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/locations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/locations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnlocationsRead(ref IQueryable<PalyavalasztoV2.Models.main.location> items);

        public async Task<IQueryable<PalyavalasztoV2.Models.main.location>> Getlocations(Query query = null)
        {
            var items = Context.locations.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnlocationsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnlocationGet(PalyavalasztoV2.Models.main.location item);
        partial void OnGetlocationByLocationId(ref IQueryable<PalyavalasztoV2.Models.main.location> items);


        public async Task<PalyavalasztoV2.Models.main.location> GetlocationByLocationId(int locationid)
        {
            var items = Context.locations
                              .AsNoTracking()
                              .Where(i => i.LocationId == locationid);

 
            OnGetlocationByLocationId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnlocationGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnlocationCreated(PalyavalasztoV2.Models.main.location item);
        partial void OnAfterlocationCreated(PalyavalasztoV2.Models.main.location item);

        public async Task<PalyavalasztoV2.Models.main.location> Createlocation(PalyavalasztoV2.Models.main.location location)
        {
            OnlocationCreated(location);

            var existingItem = Context.locations
                              .Where(i => i.LocationId == location.LocationId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.locations.Add(location);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(location).State = EntityState.Detached;
                throw;
            }

            OnAfterlocationCreated(location);

            return location;
        }

        public async Task<PalyavalasztoV2.Models.main.location> CancellocationChanges(PalyavalasztoV2.Models.main.location item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnlocationUpdated(PalyavalasztoV2.Models.main.location item);
        partial void OnAfterlocationUpdated(PalyavalasztoV2.Models.main.location item);

        public async Task<PalyavalasztoV2.Models.main.location> Updatelocation(int locationid, PalyavalasztoV2.Models.main.location location)
        {
            OnlocationUpdated(location);

            var itemToUpdate = Context.locations
                              .Where(i => i.LocationId == location.LocationId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(location);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterlocationUpdated(location);

            return location;
        }

        partial void OnlocationDeleted(PalyavalasztoV2.Models.main.location item);
        partial void OnAfterlocationDeleted(PalyavalasztoV2.Models.main.location item);

        public async Task<PalyavalasztoV2.Models.main.location> Deletelocation(int locationid)
        {
            var itemToDelete = Context.locations
                              .Where(i => i.LocationId == locationid)
                              .Include(i => i.employees)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnlocationDeleted(itemToDelete);


            Context.locations.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterlocationDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportrolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/roles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/roles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportrolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/roles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/roles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnrolesRead(ref IQueryable<PalyavalasztoV2.Models.main.role> items);

        public async Task<IQueryable<PalyavalasztoV2.Models.main.role>> Getroles(Query query = null)
        {
            var items = Context.roles.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnrolesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnroleGet(PalyavalasztoV2.Models.main.role item);
        partial void OnGetroleByRoleId(ref IQueryable<PalyavalasztoV2.Models.main.role> items);


        public async Task<PalyavalasztoV2.Models.main.role> GetroleByRoleId(int roleid)
        {
            var items = Context.roles
                              .AsNoTracking()
                              .Where(i => i.RoleID == roleid);

 
            OnGetroleByRoleId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnroleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnroleCreated(PalyavalasztoV2.Models.main.role item);
        partial void OnAfterroleCreated(PalyavalasztoV2.Models.main.role item);

        public async Task<PalyavalasztoV2.Models.main.role> Createrole(PalyavalasztoV2.Models.main.role role)
        {
            OnroleCreated(role);

            var existingItem = Context.roles
                              .Where(i => i.RoleID == role.RoleID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.roles.Add(role);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(role).State = EntityState.Detached;
                throw;
            }

            OnAfterroleCreated(role);

            return role;
        }

        public async Task<PalyavalasztoV2.Models.main.role> CancelroleChanges(PalyavalasztoV2.Models.main.role item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnroleUpdated(PalyavalasztoV2.Models.main.role item);
        partial void OnAfterroleUpdated(PalyavalasztoV2.Models.main.role item);

        public async Task<PalyavalasztoV2.Models.main.role> Updaterole(int roleid, PalyavalasztoV2.Models.main.role role)
        {
            OnroleUpdated(role);

            var itemToUpdate = Context.roles
                              .Where(i => i.RoleID == role.RoleID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(role);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterroleUpdated(role);

            return role;
        }

        partial void OnroleDeleted(PalyavalasztoV2.Models.main.role item);
        partial void OnAfterroleDeleted(PalyavalasztoV2.Models.main.role item);

        public async Task<PalyavalasztoV2.Models.main.role> Deleterole(int roleid)
        {
            var itemToDelete = Context.roles
                              .Where(i => i.RoleID == roleid)
                              .Include(i => i.applications)
                              .Include(i => i.employees)
                              .Include(i => i.supportroles)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnroleDeleted(itemToDelete);


            Context.roles.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterroleDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportsupportrolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/supportroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/supportroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportsupportrolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/supportroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/supportroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnsupportrolesRead(ref IQueryable<PalyavalasztoV2.Models.main.supportrole> items);

        public async Task<IQueryable<PalyavalasztoV2.Models.main.supportrole>> Getsupportroles(Query query = null)
        {
            var items = Context.supportroles.AsQueryable();

            items = items.Include(i => i.role);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnsupportrolesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnsupportroleGet(PalyavalasztoV2.Models.main.supportrole item);
        partial void OnGetsupportroleByUserId(ref IQueryable<PalyavalasztoV2.Models.main.supportrole> items);


        public async Task<PalyavalasztoV2.Models.main.supportrole> GetsupportroleByUserId(int userid)
        {
            var items = Context.supportroles
                              .AsNoTracking()
                              .Where(i => i.userID == userid);

            items = items.Include(i => i.role);
 
            OnGetsupportroleByUserId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnsupportroleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnsupportroleCreated(PalyavalasztoV2.Models.main.supportrole item);
        partial void OnAftersupportroleCreated(PalyavalasztoV2.Models.main.supportrole item);

        public async Task<PalyavalasztoV2.Models.main.supportrole> Createsupportrole(PalyavalasztoV2.Models.main.supportrole supportrole)
        {
            OnsupportroleCreated(supportrole);

            var existingItem = Context.supportroles
                              .Where(i => i.userID == supportrole.userID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.supportroles.Add(supportrole);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(supportrole).State = EntityState.Detached;
                throw;
            }

            OnAftersupportroleCreated(supportrole);

            return supportrole;
        }

        public async Task<PalyavalasztoV2.Models.main.supportrole> CancelsupportroleChanges(PalyavalasztoV2.Models.main.supportrole item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnsupportroleUpdated(PalyavalasztoV2.Models.main.supportrole item);
        partial void OnAftersupportroleUpdated(PalyavalasztoV2.Models.main.supportrole item);

        public async Task<PalyavalasztoV2.Models.main.supportrole> Updatesupportrole(int userid, PalyavalasztoV2.Models.main.supportrole supportrole)
        {
            OnsupportroleUpdated(supportrole);

            var itemToUpdate = Context.supportroles
                              .Where(i => i.userID == supportrole.userID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(supportrole);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAftersupportroleUpdated(supportrole);

            return supportrole;
        }

        partial void OnsupportroleDeleted(PalyavalasztoV2.Models.main.supportrole item);
        partial void OnAftersupportroleDeleted(PalyavalasztoV2.Models.main.supportrole item);

        public async Task<PalyavalasztoV2.Models.main.supportrole> Deletesupportrole(int userid)
        {
            var itemToDelete = Context.supportroles
                              .Where(i => i.userID == userid)
                              .Include(i => i.applications)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnsupportroleDeleted(itemToDelete);


            Context.supportroles.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAftersupportroleDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportuserrolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/userroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/userroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportuserrolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/main/userroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/main/userroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnuserrolesRead(ref IQueryable<PalyavalasztoV2.Models.main.userrole> items);

        public async Task<IQueryable<PalyavalasztoV2.Models.main.userrole>> Getuserroles(Query query = null)
        {
            var items = Context.userroles.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnuserrolesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnuserroleGet(PalyavalasztoV2.Models.main.userrole item);
        partial void OnGetuserroleByUserId(ref IQueryable<PalyavalasztoV2.Models.main.userrole> items);


        public async Task<PalyavalasztoV2.Models.main.userrole> GetuserroleByUserId(int userid)
        {
            var items = Context.userroles
                              .AsNoTracking()
                              .Where(i => i.UserId == userid);

 
            OnGetuserroleByUserId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnuserroleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnuserroleCreated(PalyavalasztoV2.Models.main.userrole item);
        partial void OnAfteruserroleCreated(PalyavalasztoV2.Models.main.userrole item);

        public async Task<PalyavalasztoV2.Models.main.userrole> Createuserrole(PalyavalasztoV2.Models.main.userrole userrole)
        {
            OnuserroleCreated(userrole);

            var existingItem = Context.userroles
                              .Where(i => i.UserId == userrole.UserId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.userroles.Add(userrole);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(userrole).State = EntityState.Detached;
                throw;
            }

            OnAfteruserroleCreated(userrole);

            return userrole;
        }

        public async Task<PalyavalasztoV2.Models.main.userrole> CanceluserroleChanges(PalyavalasztoV2.Models.main.userrole item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnuserroleUpdated(PalyavalasztoV2.Models.main.userrole item);
        partial void OnAfteruserroleUpdated(PalyavalasztoV2.Models.main.userrole item);

        public async Task<PalyavalasztoV2.Models.main.userrole> Updateuserrole(int userid, PalyavalasztoV2.Models.main.userrole userrole)
        {
            OnuserroleUpdated(userrole);

            var itemToUpdate = Context.userroles
                              .Where(i => i.UserId == userrole.UserId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(userrole);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfteruserroleUpdated(userrole);

            return userrole;
        }

        partial void OnuserroleDeleted(PalyavalasztoV2.Models.main.userrole item);
        partial void OnAfteruserroleDeleted(PalyavalasztoV2.Models.main.userrole item);

        public async Task<PalyavalasztoV2.Models.main.userrole> Deleteuserrole(int userid)
        {
            var itemToDelete = Context.userroles
                              .Where(i => i.UserId == userid)
                              .Include(i => i.employees)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnuserroleDeleted(itemToDelete);


            Context.userroles.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfteruserroleDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}