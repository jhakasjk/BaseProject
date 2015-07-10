using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using CoreEntities.Models;
using System.Web.Script.Serialization;
using CoreEntities.Enums;

namespace BaseProject.SignalR
{
    //[HubName("UserAvtivitHub")]
    public class UserAvtivitHub : Hub
    {
        //[HubMethodName("Send")]
        public void Send(string name, string message)
        {
            //Clients.All.hello();
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }

        //[HubMethodName("UserAcvititySend")]
        public void UserAcvititySend(string UserSessionViewString)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<UserAvtivitHub>();
            // Call the updateUserActivity method to update clients.
            var view =
            context.Clients.All.updateUserActivity(new ActionOutput { Status = ActionStatus.Successfull, Results = new List<string> { UserSessionViewString } });
        }
    }
}