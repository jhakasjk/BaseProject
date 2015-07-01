using CoreEntities.Classes;
using CoreEntities.Enums;
using CoreEntities.Interfaces;
using CoreEntities.Models;
using DataAccessLayer.Model.DataModel;
using Newtonsoft.Json;
using PushSharp;
using PushSharp.Android;
using PushSharp.Apple;
using PushSharp.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace BusinessEntities.Managers
{
    public class PushNotificationManager : BaseManager, IPushNotificationManager
    {
        #region Variables
        PushBroker push;
        public static readonly string GCMKey = "AIzaSyAucgwVr6TGOS45CcKT5_NyaHKkECPQxpM";
        public static readonly string APNSCetrtificate = "~/Resources/Certificates.p12";
        public static readonly string APNSCetrtificatePassword = "<your password>";
        List<int> DeviceTypes;
        #endregion

        public PushNotificationManager()
        {
            #region Push EventBinding
            //create the puchbroker object
            push = new PushBroker();
            //Wire up the events for all the services that the broker registers
            push.OnNotificationSent += NotificationSent;
            push.OnChannelException += ChannelException;
            push.OnServiceException += ServiceException;
            push.OnNotificationFailed += NotificationFailed;
            push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
            push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
            push.OnChannelCreated += ChannelCreated;
            push.OnChannelDestroyed += ChannelDestroyed;
            #endregion
            DeviceTypes = new List<int>();
            DeviceTypes.Add((int)RegisterVia.Android);
            DeviceTypes.Add((int)RegisterVia.AndroidFacebook);
            DeviceTypes.Add((int)RegisterVia.IPhone);
            DeviceTypes.Add((int)RegisterVia.IPhoneFacebook);
        }

        #region Push Notification Events
        //Currently it will raise only for android devices
        static void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
        {
            //Do something here
        }

        //this even raised when a notification is successfully sent
        static void NotificationSent(object sender, INotification notification)
        {
            //Do something here
        }

        //this is raised when a notification is failed due to some reason
        static void NotificationFailed(object sender, INotification notification, Exception notificationFailureException)
        {
            //Do something here
            using (var context = new DBEntities())
            {
                context.ErrorLogs.Add(new ErrorLog
                {
                    FormData = "",
                    LoggedInDetails = "",
                    QueryData = "",
                    RouteData = "",
                    InnerException = notificationFailureException.GetBaseException().ToString(),
                    LoggedAt = DateTime.UtcNow,
                    Message = notificationFailureException.Message,
                    StackTrace = notificationFailureException.StackTrace
                });
                context.SaveChanges();
            }
        }

        //this is fired when there is exception is raised by the channel
        static void ChannelException
            (object sender, IPushChannel channel, Exception exception)
        {
            //Do something here
        }

        //this is fired when there is exception is raised by the service
        static void ServiceException(object sender, Exception exception)
        {
            //Do something here
            using (var context = new DBEntities())
            {
                context.ErrorLogs.Add(new ErrorLog
                {
                    FormData = "",
                    LoggedInDetails = "",
                    QueryData = "",
                    RouteData = "",
                    InnerException = exception.ToString(),
                    LoggedAt = DateTime.UtcNow,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace
                });
                context.SaveChanges();
            }
        }

        //this is raised when the particular device subscription is expired
        static void DeviceSubscriptionExpired(object sender,
        string expiredDeviceSubscriptionId,
            DateTime timestamp, INotification notification)
        {
            //Do something here
        }

        //this is raised when the channel is destroyed
        static void ChannelDestroyed(object sender)
        {
            //Do something here
        }

        //this is raised when the channel is created
        static void ChannelCreated(object sender, IPushChannel pushChannel)
        {
            //Do something here
        }
        #endregion

        private void SendNotification(List<PushNotificationDevice> Devices, string alert)
        {
            PushNotificationMessage msg = new PushNotificationMessage
            {
                alert = alert,
                badge = 0,
                sound = "sound.caf"
            };
            foreach (var Device in Devices)
            {
                string jsonMessage = JsonConvert.SerializeObject(msg);
                switch (Device.DeviceType)
                {
                    case (int)RegisterVia.Android:
                    case (int)RegisterVia.AndroidFacebook:
                        //---------------------------
                        // ANDROID GCM NOTIFICATIONS
                        //---------------------------
                        //Configure and start Android GCM
                        //IMPORTANT: The API KEY comes from your Google APIs Console App, 
                        //under the API Access section, 
                        //  by choosing 'Create new Server key...'
                        //  You must ensure the 'Google Cloud Messaging for Android' service is 
                        //enabled in your APIs Console
                        push.RegisterGcmService(new GcmPushChannelSettings(GCMKey)); // Set API Key for GCM                         
                        //Fluent construction of an Android GCM Notification
                        //IMPORTANT: For Android you MUST use your own RegistrationId 
                        //here that gets generated within your Android app itself!
                        push.QueueNotification(new GcmNotification().ForDeviceRegistrationId(Device.DeviceToken)    // Set Actual Device Token
                                                                    .WithJson(jsonMessage));
                        break;
                    case (int)RegisterVia.IPhone:
                    case (int)RegisterVia.IPhoneFacebook:
                        //-------------------------
                        // APPLE NOTIFICATIONS
                        //-------------------------
                        //Configure and start Apple APNS
                        // IMPORTANT: Make sure you use the right Push certificate.  Apple allows you to
                        //generate one for connecting to Sandbox, and one for connecting to Production.  You must
                        // use the right one, to match the provisioning profile you build your
                        //   app with!
                        var appleCert = File.ReadAllBytes(HostingEnvironment.MapPath(APNSCetrtificate));
                        //IMPORTANT: If you are using a Development provisioning Profile, you must use
                        // the Sandbox push notification server 
                        //  (so you would leave the first arg in the ctor of ApplePushChannelSettings as
                        // 'false')
                        //  If you are using an AdHoc or AppStore provisioning profile, you must use the 
                        //Production push notification server
                        //  (so you would change the first arg in the ctor of ApplePushChannelSettings to 
                        //'true')
                        push.RegisterAppleService(new ApplePushChannelSettings(false, appleCert, APNSCetrtificatePassword));
                        //Extension method
                        //Fluent construction of an iOS notification
                        //IMPORTANT: For iOS you MUST MUST MUST use your own DeviceToken here that gets
                        // generated within your iOS app itself when the Application Delegate
                        //  for registered for remote notifications is called, 
                        // and the device token is passed back to you
                        push.QueueNotification(new AppleNotification()
                                                    .ForDeviceToken(Device.DeviceToken)//the recipient device id
                                                    .WithAlert(msg.alert)//the message
                                                    .WithBadge(0)
                                                    .WithSound("sound.caf")
                                                    );
                        break;
                }
            }
            push.StopAllServices(waitForQueuesToFinish: true);
        }

        void IPushNotificationManager.SendTest(string Message, string DeviceToken, int DeviceType)
        {
            List<PushNotificationDevice> Devices = new List<PushNotificationDevice>();
            Devices.Add(new PushNotificationDevice { DeviceToken = DeviceToken, DeviceType = DeviceType });
            SendNotification(Devices, "Testing Notification");
            Thread thread = new Thread(delegate()
            {
                SendNotification(Devices, "A new joke added: " + Message);
            });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
