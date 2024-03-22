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

using PalyavalsztoV4.Data;

namespace PalyavalsztoV4
{
    public partial class v4_1Service
    {
        v4_1Context Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly v4_1Context context;
        private readonly NavigationManager navigationManager;

        public v4_1Service(v4_1Context context, NavigationManager navigationManager)
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


        public async Task ExportapplicationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/v4_1/applications/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/v4_1/applications/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportapplicationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/v4_1/applications/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/v4_1/applications/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnapplicationsRead(ref IQueryable<PalyavalsztoV4.Models.v4_1.application> items);

        public async Task<IQueryable<PalyavalsztoV4.Models.v4_1.application>> Getapplications(Query query = null)
        {
            var items = Context.applications.AsQueryable();

            items = items.Include(i => i.employee);
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

        partial void OnapplicationGet(PalyavalsztoV4.Models.v4_1.application item);
        partial void OnGetapplicationByApplicationId(ref IQueryable<PalyavalsztoV4.Models.v4_1.application> items);


        public async Task<PalyavalsztoV4.Models.v4_1.application> GetapplicationByApplicationId(int applicationid)
        {
            var items = Context.applications
                              .AsNoTracking()
                              .Where(i => i.ApplicationID == applicationid);

            items = items.Include(i => i.employee);
            items = items.Include(i => i.job);
 
            OnGetapplicationByApplicationId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnapplicationGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnapplicationCreated(PalyavalsztoV4.Models.v4_1.application item);
        partial void OnAfterapplicationCreated(PalyavalsztoV4.Models.v4_1.application item);

        public async Task<PalyavalsztoV4.Models.v4_1.application> Createapplication(PalyavalsztoV4.Models.v4_1.application application)
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

        public async Task<PalyavalsztoV4.Models.v4_1.application> CancelapplicationChanges(PalyavalsztoV4.Models.v4_1.application item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnapplicationUpdated(PalyavalsztoV4.Models.v4_1.application item);
        partial void OnAfterapplicationUpdated(PalyavalsztoV4.Models.v4_1.application item);

        public async Task<PalyavalsztoV4.Models.v4_1.application> Updateapplication(int applicationid, PalyavalsztoV4.Models.v4_1.application application)
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

        partial void OnapplicationDeleted(PalyavalsztoV4.Models.v4_1.application item);
        partial void OnAfterapplicationDeleted(PalyavalsztoV4.Models.v4_1.application item);

        public async Task<PalyavalsztoV4.Models.v4_1.application> Deleteapplication(int applicationid)
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
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/v4_1/employees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/v4_1/employees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportemployeesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/v4_1/employees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/v4_1/employees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnemployeesRead(ref IQueryable<PalyavalsztoV4.Models.v4_1.employee> items);

        public async Task<IQueryable<PalyavalsztoV4.Models.v4_1.employee>> Getemployees(Query query = null)
        {
            var items = Context.employees.AsQueryable();

            items = items.Include(i => i.user);

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

        partial void OnemployeeGet(PalyavalsztoV4.Models.v4_1.employee item);
        partial void OnGetemployeeByEmployeeId(ref IQueryable<PalyavalsztoV4.Models.v4_1.employee> items);


        public async Task<PalyavalsztoV4.Models.v4_1.employee> GetemployeeByEmployeeId(int employeeid)
        {
            var items = Context.employees
                              .AsNoTracking()
                              .Where(i => i.EmployeeID == employeeid);

            items = items.Include(i => i.user);
 
            OnGetemployeeByEmployeeId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnemployeeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnemployeeCreated(PalyavalsztoV4.Models.v4_1.employee item);
        partial void OnAfteremployeeCreated(PalyavalsztoV4.Models.v4_1.employee item);

        public async Task<PalyavalsztoV4.Models.v4_1.employee> Createemployee(PalyavalsztoV4.Models.v4_1.employee employee)
        {
            OnemployeeCreated(employee);

            var existingItem = Context.employees
                              .Where(i => i.EmployeeID == employee.EmployeeID)
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

        public async Task<PalyavalsztoV4.Models.v4_1.employee> CancelemployeeChanges(PalyavalsztoV4.Models.v4_1.employee item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnemployeeUpdated(PalyavalsztoV4.Models.v4_1.employee item);
        partial void OnAfteremployeeUpdated(PalyavalsztoV4.Models.v4_1.employee item);

        public async Task<PalyavalsztoV4.Models.v4_1.employee> Updateemployee(int employeeid, PalyavalsztoV4.Models.v4_1.employee employee)
        {
            OnemployeeUpdated(employee);

            var itemToUpdate = Context.employees
                              .Where(i => i.EmployeeID == employee.EmployeeID)
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

        partial void OnemployeeDeleted(PalyavalsztoV4.Models.v4_1.employee item);
        partial void OnAfteremployeeDeleted(PalyavalsztoV4.Models.v4_1.employee item);

        public async Task<PalyavalsztoV4.Models.v4_1.employee> Deleteemployee(int employeeid)
        {
            var itemToDelete = Context.employees
                              .Where(i => i.EmployeeID == employeeid)
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
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/v4_1/employers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/v4_1/employers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportemployersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/v4_1/employers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/v4_1/employers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnemployersRead(ref IQueryable<PalyavalsztoV4.Models.v4_1.employer> items);

        public async Task<IQueryable<PalyavalsztoV4.Models.v4_1.employer>> Getemployers(Query query = null)
        {
            var items = Context.employers.AsQueryable();

            items = items.Include(i => i.user);

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

        partial void OnemployerGet(PalyavalsztoV4.Models.v4_1.employer item);
        partial void OnGetemployerByEmployerId(ref IQueryable<PalyavalsztoV4.Models.v4_1.employer> items);


        public async Task<PalyavalsztoV4.Models.v4_1.employer> GetemployerByEmployerId(int employerid)
        {
            var items = Context.employers
                              .AsNoTracking()
                              .Where(i => i.EmployerID == employerid);

            items = items.Include(i => i.user);
 
            OnGetemployerByEmployerId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnemployerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnemployerCreated(PalyavalsztoV4.Models.v4_1.employer item);
        partial void OnAfteremployerCreated(PalyavalsztoV4.Models.v4_1.employer item);

        public async Task<PalyavalsztoV4.Models.v4_1.employer> Createemployer(PalyavalsztoV4.Models.v4_1.employer employer)
        {
            OnemployerCreated(employer);

            var existingItem = Context.employers
                              .Where(i => i.EmployerID == employer.EmployerID)
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

        public async Task<PalyavalsztoV4.Models.v4_1.employer> CancelemployerChanges(PalyavalsztoV4.Models.v4_1.employer item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnemployerUpdated(PalyavalsztoV4.Models.v4_1.employer item);
        partial void OnAfteremployerUpdated(PalyavalsztoV4.Models.v4_1.employer item);

        public async Task<PalyavalsztoV4.Models.v4_1.employer> Updateemployer(int employerid, PalyavalsztoV4.Models.v4_1.employer employer)
        {
            OnemployerUpdated(employer);

            var itemToUpdate = Context.employers
                              .Where(i => i.EmployerID == employer.EmployerID)
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

        partial void OnemployerDeleted(PalyavalsztoV4.Models.v4_1.employer item);
        partial void OnAfteremployerDeleted(PalyavalsztoV4.Models.v4_1.employer item);

        public async Task<PalyavalsztoV4.Models.v4_1.employer> Deleteemployer(int employerid)
        {
            var itemToDelete = Context.employers
                              .Where(i => i.EmployerID == employerid)
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
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/v4_1/jobs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/v4_1/jobs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportjobsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/v4_1/jobs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/v4_1/jobs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnjobsRead(ref IQueryable<PalyavalsztoV4.Models.v4_1.job> items);

        public async Task<IQueryable<PalyavalsztoV4.Models.v4_1.job>> Getjobs(Query query = null)
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

        partial void OnjobGet(PalyavalsztoV4.Models.v4_1.job item);
        partial void OnGetjobByJobId(ref IQueryable<PalyavalsztoV4.Models.v4_1.job> items);


        public async Task<PalyavalsztoV4.Models.v4_1.job> GetjobByJobId(int jobid)
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

        partial void OnjobCreated(PalyavalsztoV4.Models.v4_1.job item);
        partial void OnAfterjobCreated(PalyavalsztoV4.Models.v4_1.job item);

        public async Task<PalyavalsztoV4.Models.v4_1.job> Createjob(PalyavalsztoV4.Models.v4_1.job job)
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

        public async Task<PalyavalsztoV4.Models.v4_1.job> CanceljobChanges(PalyavalsztoV4.Models.v4_1.job item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnjobUpdated(PalyavalsztoV4.Models.v4_1.job item);
        partial void OnAfterjobUpdated(PalyavalsztoV4.Models.v4_1.job item);

        public async Task<PalyavalsztoV4.Models.v4_1.job> Updatejob(int jobid, PalyavalsztoV4.Models.v4_1.job job)
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

        partial void OnjobDeleted(PalyavalsztoV4.Models.v4_1.job item);
        partial void OnAfterjobDeleted(PalyavalsztoV4.Models.v4_1.job item);

        public async Task<PalyavalsztoV4.Models.v4_1.job> Deletejob(int jobid)
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
    
        public async Task ExportrolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/v4_1/roles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/v4_1/roles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportrolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/v4_1/roles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/v4_1/roles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnrolesRead(ref IQueryable<PalyavalsztoV4.Models.v4_1.role> items);

        public async Task<IQueryable<PalyavalsztoV4.Models.v4_1.role>> Getroles(Query query = null)
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

        partial void OnroleGet(PalyavalsztoV4.Models.v4_1.role item);
        partial void OnGetroleByRoleId(ref IQueryable<PalyavalsztoV4.Models.v4_1.role> items);


        public async Task<PalyavalsztoV4.Models.v4_1.role> GetroleByRoleId(int roleid)
        {
            var items = Context.roles
                              .AsNoTracking()
                              .Where(i => i.RoleID == roleid);

 
            OnGetroleByRoleId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnroleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnroleCreated(PalyavalsztoV4.Models.v4_1.role item);
        partial void OnAfterroleCreated(PalyavalsztoV4.Models.v4_1.role item);

        public async Task<PalyavalsztoV4.Models.v4_1.role> Createrole(PalyavalsztoV4.Models.v4_1.role role)
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

        public async Task<PalyavalsztoV4.Models.v4_1.role> CancelroleChanges(PalyavalsztoV4.Models.v4_1.role item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnroleUpdated(PalyavalsztoV4.Models.v4_1.role item);
        partial void OnAfterroleUpdated(PalyavalsztoV4.Models.v4_1.role item);

        public async Task<PalyavalsztoV4.Models.v4_1.role> Updaterole(int roleid, PalyavalsztoV4.Models.v4_1.role role)
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

        partial void OnroleDeleted(PalyavalsztoV4.Models.v4_1.role item);
        partial void OnAfterroleDeleted(PalyavalsztoV4.Models.v4_1.role item);

        public async Task<PalyavalsztoV4.Models.v4_1.role> Deleterole(int roleid)
        {
            var itemToDelete = Context.roles
                              .Where(i => i.RoleID == roleid)
                              .Include(i => i.users)
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
    
        public async Task ExportusersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/v4_1/users/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/v4_1/users/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportusersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/v4_1/users/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/v4_1/users/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnusersRead(ref IQueryable<PalyavalsztoV4.Models.v4_1.user> items);

        public async Task<IQueryable<PalyavalsztoV4.Models.v4_1.user>> Getusers(Query query = null)
        {
            var items = Context.users.AsQueryable();

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

            OnusersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnuserGet(PalyavalsztoV4.Models.v4_1.user item);
        partial void OnGetuserByUserId(ref IQueryable<PalyavalsztoV4.Models.v4_1.user> items);


        public async Task<PalyavalsztoV4.Models.v4_1.user> GetuserByUserId(int userid)
        {
            var items = Context.users
                              .AsNoTracking()
                              .Where(i => i.UserID == userid);

            items = items.Include(i => i.role);
 
            OnGetuserByUserId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnuserGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnuserCreated(PalyavalsztoV4.Models.v4_1.user item);
        partial void OnAfteruserCreated(PalyavalsztoV4.Models.v4_1.user item);

        public async Task<PalyavalsztoV4.Models.v4_1.user> Createuser(PalyavalsztoV4.Models.v4_1.user user)
        {
            OnuserCreated(user);

            var existingItem = Context.users
                              .Where(i => i.UserID == user.UserID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.users.Add(user);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(user).State = EntityState.Detached;
                throw;
            }

            OnAfteruserCreated(user);

            return user;
        }

        public async Task<PalyavalsztoV4.Models.v4_1.user> CanceluserChanges(PalyavalsztoV4.Models.v4_1.user item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnuserUpdated(PalyavalsztoV4.Models.v4_1.user item);
        partial void OnAfteruserUpdated(PalyavalsztoV4.Models.v4_1.user item);

        public async Task<PalyavalsztoV4.Models.v4_1.user> Updateuser(int userid, PalyavalsztoV4.Models.v4_1.user user)
        {
            OnuserUpdated(user);

            var itemToUpdate = Context.users
                              .Where(i => i.UserID == user.UserID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(user);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfteruserUpdated(user);

            return user;
        }

        partial void OnuserDeleted(PalyavalsztoV4.Models.v4_1.user item);
        partial void OnAfteruserDeleted(PalyavalsztoV4.Models.v4_1.user item);

        public async Task<PalyavalsztoV4.Models.v4_1.user> Deleteuser(int userid)
        {
            var itemToDelete = Context.users
                              .Where(i => i.UserID == userid)
                              .Include(i => i.employees)
                              .Include(i => i.employers)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnuserDeleted(itemToDelete);


            Context.users.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfteruserDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}