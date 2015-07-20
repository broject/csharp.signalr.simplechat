using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebApplication1
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        static int _hitCount = 0;

        protected string GetIpAddress()
        {
            string ipAddress;
            object tempObject;

            Context.Request.Environment.TryGetValue("server.RemoteIpAddress", out tempObject);

            if (tempObject != null)
            {
                ipAddress = (string)tempObject;
            }
            else
            {
                ipAddress = "";
            }

            return ipAddress;
        }

        public void Message(string who, string message)
        {
            string ip = GetIpAddress();
            who = who + " ( " + ip + " )";
            if (string.IsNullOrEmpty(message)) return;
            if (who.Length > 50) who = who.Substring(0, 30);
            if (message.Length > 160) message = message.Substring(0, 160);

            Clients.All.message(HttpUtility.HtmlEncode(who), HttpUtility.HtmlEncode(message));
        }

        public void Hit()
        {
            _hitCount += 1;
            Clients.All.hit(_hitCount);
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            Clients.All.hit(_hitCount);
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            _hitCount -= 1;
            Clients.All.hit(_hitCount);
            return base.OnDisconnected(stopCalled);
        }
    }
}