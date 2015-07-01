using CoreEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Interfaces
{
    public interface IPushNotificationManager
    {
        void SendTest(string Message, string DeviceID, int  DeviceType);

    }
}
