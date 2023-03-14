using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Messages
{
    public record class DoorNodesUpdateMessage
    {
        List<Vector3> doornodes = new();

        public DoorNodesUpdateMessage(List<Vector3> doornodes)
        {
            this.doornodes = doornodes;     
        }
    }
}
