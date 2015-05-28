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

        public void Message(string who, string message) 
        {
            Clients.All.message(who, message);
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