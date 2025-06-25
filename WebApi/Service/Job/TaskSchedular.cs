using Quartz.Spi;
using Quartz;
using WebApi.Configs;
using WebApi.DTO;
using Microsoft.Extensions.Options;

namespace WebApi.Service.Job
{
    public class TaskSchedular : IHostedService
    {
        private readonly JobSettings _settings;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        public TaskSchedular(IOptions<JobSettings> appSettings,
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory)
        {
            _settings = appSettings.Value;
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }
        public IScheduler Scheduler { get; set; }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var jobSchedules = new List<JobSchedule>();

            //Add job check hết hạn
            if (_settings.ExpiredJob != null && _settings.ExpiredJob.Enable)
                jobSchedules.Add(new JobSchedule(Guid.NewGuid(), typeof(ExpiredContractJob), "ExpiredContractJob", _settings.ExpiredJob.Daily));
            
            //Thêm job check gần hết hạn...

            //Creating Schdeular
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;

            foreach (var jobSchedule in jobSchedules)
            {
                var job = CreateJob(jobSchedule);
                var trigger = CreateTrigger(jobSchedule);

                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }

            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
        }

        private IJobDetail CreateJob(JobSchedule schedule)
        {
            return JobBuilder.Create(schedule.JobType)
                .WithIdentity(schedule.JobId.ToString())
                .WithDescription(schedule.JobName)
                .DisallowConcurrentExecution()
                .Build();
        }

        private ITrigger CreateTrigger(JobSchedule schedule)
        {
            return TriggerBuilder.Create()
                .WithIdentity(schedule.JobId.ToString())
                .WithDescription(schedule.JobName)
                .WithCronSchedule(schedule.CronExpression)
                .Build();
        }
    }
}
