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
        private readonly DateTime _internetBirthDate = On.July.The13th.In(2015).At(20,15);
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
            _uptimeHub.Clients.All.internetUpTime(uptime.Humanize(2)+" to go home!!!");
            //DateTime dt = new DateTime(2015, 7, 13, 20, 15, 00);
            //DateTime dt2 = DateTime.Now;
            //_uptimeHub.Clients.All.internetUpTime((dt - dt2).Minutes + " minutes to go Home!!!");
        }

        public void Stop(bool immediate)
        {
            _timer.Dispose();

            HostingEnvironment.UnregisterObject(this);
        }
    }
}