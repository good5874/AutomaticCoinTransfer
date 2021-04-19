using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticCoinTransfer
{
    public class Scheduler
    {
        private IJobDetail sendCoinsJob;
        private ITrigger sendCoinsTrigger;
        private IScheduler scheduler;

        public void Start()
        {
            #region Exchange
            sendCoinsJob = JobBuilder.Create<SendCoinsJob>()
                .WithIdentity("SendCoinsJob")
                .Build();
            sendCoinsTrigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(Settings.Seconds)
                    .RepeatForever())
                .Build();
            #endregion


            scheduler = new StdSchedulerFactory().GetScheduler().Result;

            scheduler.ScheduleJob(sendCoinsJob, sendCoinsTrigger);

            scheduler.Start();
        }
    }
}
