using System;
using System.Collections.Generic;

namespace Core
{
    public class ServerConnection : Connection
    {
        private Server _server;

        public ServerConnection(Server server)
        {
            _server = server;
        }

        protected override void OnPacketDispatch(string packet)
        {
            Console.WriteLine(packet);
        }

        protected override void OnClose()
        {
            _server.OnLeaveConnection(this);
        }
    }
}
