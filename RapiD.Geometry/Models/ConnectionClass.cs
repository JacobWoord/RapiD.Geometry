using Assimp;
using CommunityToolkit.Mvvm.Messaging;
using RapiD.Geometry.Messages;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Models
{
    public partial class ConnectionClass : ObservableObject
    {
        public ChainLink3D chainConnection;
        public CablePatent cableConnection;
        public Tube3D tubeConnection;
        
       
         public Vector3 startVector;
        public Vector3 endVector;
        public ConnectionType type;
        
        public string Id;
        public string PatentId;
        

        public ConnectionClass(Vector3 startVector, Vector3 endVector)
        {
            type = ConnectionType.Chain;
            this.startVector = startVector;
            this.endVector = endVector;
            Id= Guid.NewGuid().ToString();
            CreateConnection();
     
        }

        public void Update()
        {
            WeakReferenceMessenger.Default.Send(new ConnectionEndPointVectorChangedMessage(endVector,Id));
        }


        public void CreateConnection()
        {

            if (type == ConnectionType.Chain)
            {
                var chain = ChainConnection();
                WeakReferenceMessenger.Default.Send(new ConnectionChangedMessage(chain));
                chainConnection = chain;
            }
            else if (type == ConnectionType.RubberCable)
            {
                CableConnection();
            }
            else if (type == ConnectionType.SteelCable)
            {
                SteelCableConnection();
            }
            else if (type == ConnectionType.Rope)
            {

            }
            return;
        }







        public ChainLink3D ChainConnection()
        {
            var chainConnection = new ChainLink3D(startVector, endVector) { ConnectionID = Id};  
           
            return chainConnection;
        }




        public void CableConnection()
        {

        }

        public void SteelCableConnection()
        {

        }

        public void RopeConnection()
        {

        }
    }
}
