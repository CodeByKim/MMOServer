﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Microsoft.Extensions.ObjectPool;

namespace Core
{
    public class ConnectionPooledObjectPolicy : IPooledObjectPolicy<ServerConnection>
    {
        private Server _server;

        public ConnectionPooledObjectPolicy(Server server)
        {
            _server = server;
        }

        public ServerConnection Create()
        {
            ServerConnection connection = new ServerConnection(_server);
            return connection;
        }

        public bool Return(ServerConnection conn)
        {
            conn.Release();
            return true;
        }
    }
}
