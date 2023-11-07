using Quartz.Impl;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart
{
      public partial class Scheduler
      {
          public void StartCronJob()
          {
            IJobDetail job = JobBuilder.Create<MyJob>()
            .Build();

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("myTrigger")
            .WithCronSchedule("0 0/2 * 1/1 * ? *")
            .ForJob(job)
            .Build();
      
            scheduler.ScheduleJob(job, trigger);
            scheduler.Start();
            scheduler.TriggerJob(trigger.JobKey);

          }
      }

    
}
