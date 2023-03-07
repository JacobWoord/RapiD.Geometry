using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Models
{
    public record patentChangedMessage
    {
        public float totalLength { get; }
        public string patentId { get; }
        public string name { get; }

        public patentChangedMessage(float totalLength, string patentId, string name)
        {
            this.totalLength = totalLength;
            this.patentId = patentId;
            this.name = name;
        }
    }
}
