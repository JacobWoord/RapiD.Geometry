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

       public ChainLink3D chain { get; }
       Tube3D tube;


        public ConnectionChangedMessage(ChainLink3D chain)
        {
            this.chain = chain;
        }
    
    }



}
