﻿namespace WebApi.DTO
{
    public class JobSchedule
    {
        public Guid JobId { get; set; }
        public Type JobType { get; }
        public string JobName { get; }
        public string CronExpression { get; }
        public JobSchedule(Guid Id,
            Type jobType,
            string jobName,
            string cronExpression)
        {
            JobId = Id;
            JobType = jobType;
            JobName = jobName;
            CronExpression = cronExpression;
        }
    }
}
