using RapiD.Geometry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Messages
{
    public record ConnectionChangedMessage
    {
        public ConnectionType connectionType; 
        public float ConnectionLength; 

        public ConnectionChangedMessage(ConnectionType conType)
        {
            this.connectionType = conType;
        }

        public ConnectionChangedMessage(float length)
        {
            this.ConnectionLength = length;
        }








    }



}
