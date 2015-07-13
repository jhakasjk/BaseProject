using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Web.Hosting;
using Humanizer;
using System.Threading;

namespace BaseProject.SignalR
{
    public class BackgroundUptimeServerTimer : IRegisteredObject
    {
        private readonly DateTime _internetBirthDate = On.October.The29th.In(2014);
        private readonly IHubContext _uptimeHub;
        private readonly int _timezoneOffset;
        private Timer _timer;

        public BackgroundUptimeServerTimer() { }

        public BackgroundUptimeServerTimer(int TimeZoneOffset)
        {
            _timezoneOffset = TimeZoneOffset;
            _uptimeHub = GlobalHost.ConnectionManager.GetHubContext<UptimeHub>();
            StartTimer();
        }

        private void StartTimer()
        {
            var delayStartby = 2.Seconds();
            var repeatEvery = 1.Seconds();

            _timer = new Timer(BroadcastUptimeToClients, null, delayStartby, repeatEvery);
        }
        private void BroadcastUptimeToClients(object state)
        {
            TimeSpan uptime = DateTime.Now - _internetBirthDate;

            //_uptimeHub.Clients.All.internetUpTime(uptime.Humanize(5));            
            _uptimeHub.Clients.All.internetUpTime(DateTime.Now.ToString());
        }

        public void Stop(bool immediate)
        {
            _timer.Dispose();

            HostingEnvironment.UnregisterObject(this);
        }
    }
}