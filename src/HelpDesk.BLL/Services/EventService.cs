using Hangfire;
using Hangfire.Storage;
using HelpDesk.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Services
{
    /// <inheritdoc cref="IEventService<T>"/>
    public class EventService : IEventService
    {
        private readonly IGetUserFromAD _getuser;
        private readonly IProfileService _profile;

        public EventService(IGetUserFromAD getUser, IProfileService profile)
        {
            _getuser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            _profile = profile ?? throw new ArgumentNullException(nameof(profile));
        }
        public async Task AddJobScheduller(string cron)
        {
            if (cron is null)
            {
                throw new ArgumentNullException(nameof(cron));
            }
            //await Task.Run(() =>
            //{
            //    RecurringJob.AddOrUpdate(() => JobAddUserToBase().GetAwaiter().GetResult(), cron);
            //});
            await Task.Run(() =>
            { RecurringJob.AddOrUpdate(() => Console.WriteLine("Minutely Job"), cron); });
        }

        public async Task EditJobScheduller(string id, string cron)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            //await Task.Run(() =>
            //{
            //    RecurringJob.AddOrUpdate(id,() => JobAddUserToBase().GetAwaiter().GetResult(), cron);
            //});

            await Task.Run(() =>
            { RecurringJob.AddOrUpdate(id,() => Console.WriteLine("Minutely Job"), cron); });
        }

        public async Task <RecurringJobDto> GetJobScheduller(string id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            List<RecurringJobDto> recurringJobs = new List<RecurringJobDto>();

            await Task.Run(() => recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs());

            var job = recurringJobs.FirstOrDefault(c => c.Id.Equals(id));
            if (job is null)
            {
                return new RecurringJobDto();
            }
            return job;
        }

        public async Task DeleteJobScheduller(string id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await Task.Run(() => RecurringJob.RemoveIfExists(id));        
        }

        public async Task JobAddUserToBase()
        {
            var listUsers = await _getuser.ADGetUsers();
            if (listUsers.Any())
            {
                await _profile.AddAsyncUsers(listUsers);
            }
        }
    }
}
