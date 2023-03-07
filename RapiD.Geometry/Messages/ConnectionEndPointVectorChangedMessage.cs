using SharpDX;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Messages
{
    public record ConnectionEndPointVectorChangedMessage

    {
        public Vector3 endpoint;
        public string connectionId;

        public ConnectionEndPointVectorChangedMessage(Vector3 Endpoint, string connectionId)
        {
            endpoint = Endpoint;
            this.connectionId = connectionId;
        }

    }
}
