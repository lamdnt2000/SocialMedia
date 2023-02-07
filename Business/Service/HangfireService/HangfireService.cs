using Business.Repository.ChannelCrawlRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Models.OrganizationModel;
using DataAccess.Models.Pagination;
using Firebase.Auth;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.HangfireService
{
    public class HangfireService : IHangfireService
    {
        private readonly IStorageConnection _storeConnection = JobStorage.Current.GetConnection();

        private readonly IChannelCrawlRepository _channelCrawlRepository;
        private readonly string ClassName = "Job";
        public HangfireService(IChannelCrawlRepository channelCrawlRepository)
        {

            _channelCrawlRepository = channelCrawlRepository;
        }

        public async Task<IEnumerable<ChannelCrawl>> CreateSchedule(List<int> ids)
        {
            return await _channelCrawlRepository.GetAllAsync(x => ids.Contains(x.Id));
        }
        public void DeleteJob(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);
        }

        public async Task<PaginationList<ChannelCrawlDto>> GetAllChannelSchedule(HangfireChannelFilter filter)
        {
            var recurringJobs = _storeConnection.GetRecurringJobs().Where(x => x.Id!="Token").Select(x=> x.Id.Split(": ")[1]).ToList();
            var result =  await _channelCrawlRepository.GetChannelSchedule(recurringJobs, filter);
            var items = MapperConfig.GetMapper().Map<List<ChannelCrawlDto>>(result.Items);
            return new PaginationList<ChannelCrawlDto>
            {
                Items = items,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalItem = result.TotalItem,
                TotalPage = result.TotalPage
            };

        }

        public IEnumerable<object> GetCurrentConcurrentJob()
        {
            return _storeConnection.GetRecurringJobs().Select(x => new
            {
                x.Id,
                x.CreatedAt,
                x.LastExecution,
                x.NextExecution
            }).OrderByDescending(x => x.CreatedAt);


        }

        public string TriggerJob(string jobId)
        {
            var recurringJobs = _storeConnection.GetRecurringJobs();

            var job = recurringJobs.Where(x => x.Id == jobId).FirstOrDefault();
            if (job == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            if (job.LastExecution == null || (job.LastExecution != null && DateUtil.CompareMinute(job.LastExecution.Value, DateTime.Now, 30)))
            {
                var result = RecurringJob.TriggerJob(jobId);
                return result;
            }
            else
            {
                throw new Exception(ClassName + " Recent " + EXCUSED);
            }
        }
    }
}
