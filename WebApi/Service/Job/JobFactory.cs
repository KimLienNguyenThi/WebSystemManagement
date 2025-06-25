using Quartz.Spi;
using Quartz;

namespace WebApi.Service.Job
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _provider;
        public JobFactory(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            return (IJob)_provider.GetService(jobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        { }
    }
}
