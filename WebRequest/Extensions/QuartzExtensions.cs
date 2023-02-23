using Microsoft.Extensions.Configuration;
using Quartz;
using System;

namespace WebRequest.Extensions;
public static class QuartzExtensions
{
    public static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz,
                                                IConfiguration config) where T : IJob
    {
        var jobName = typeof(T).Name;
        var cronSchedule = config[$"Quartz:{jobName}:Schedule"];
        var group = config[$"Quartz:{jobName}:Group"];

        if (string.IsNullOrEmpty(cronSchedule))
            throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {jobName}");

        int instances = int.Parse(config[$"Quartz:{jobName}:Instances"]);
        for (int i = 0; i < instances; i++)
        {
            var jobKey = new JobKey($"{jobName}:Instance:{i + 1}", group);
            quartz.AddJob<T>(jobKey)
                  .AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity($"{jobName}:Trigger:{i + 1}")
                    .WithCronSchedule(cronSchedule));
        }
    }
}