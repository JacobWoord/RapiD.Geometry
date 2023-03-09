using RapiD.Geometry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Messages
{
    public record ConnectionListUpdateMessage
    {
        public ConnectionClass connection;

        public ConnectionListUpdateMessage(ConnectionClass connection)
        {
            this.connection = connection;
        }
        
    }
}
