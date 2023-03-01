using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Models
{
    public partial class DoorPatent3D : GeometryBase3D
    {
        private float upperChainLength;
        private float middleChainLength;
        private float BottomChainLength;
        private  float diameter;
        private  float innerLength;
        private  string name;

        public DoorPatent3D(float upperChainLength = 8000, float middleChainLength = 2000, float bottomChainLength = 8000, float diameter = 16, float innerLength = 60, string name= "Patent")
        {
         
            
            this.upperChainLength= upperChainLength; 
            this.middleChainLength= middleChainLength;
            this.BottomChainLength= bottomChainLength;
            this.diameter = diameter;
            this.innerLength = innerLength;
            this.name = name;
        }

        public List<ChainLink3D> CreateDoorPatent(Door door)
        {
            var nodeList = door.GetNodeList();
          
            Vector3 bottomnode = nodeList[7];
            Vector3 topnode = nodeList[5];
            float elementLength = innerLength;
            float distance = Vector3.Distance(bottomnode, topnode);






        




            //Creates Upper Chain
            float averageNumberOfLinksUP = upperChainLength / (innerLength + diameter * 2);
            float finalNumberOfLinklsUP = MathF.Round(averageNumberOfLinksUP);
            float finalChainLengthUp = finalNumberOfLinklsUP * elementLength;
          

            //Creates Bottom Chain
            float averageNumberOfLinksBOT = upperChainLength / (innerLength + diameter * 2);
            float finalNumberOfLinklsBOT = MathF.Round(averageNumberOfLinksBOT);
            float finalChainLengthBOT = finalNumberOfLinklsBOT * elementLength;

            //Side lengths in mm
            float upperLength = finalChainLengthUp;
            float bottomLength = finalChainLengthBOT;



            float cos_bottom = (bottomLength * bottomLength + distance * distance - upperLength * upperLength)/(2*bottomLength*distance);
            float angle_bottom = MathF.Acos(cos_bottom);

            float transwidht = bottomLength * MathF.Sin(angle_bottom);
            float transheigth = bottomLength * cos_bottom;

            Vector3 endNode = bottomnode + new Vector3(0, -transwidht, transheigth);


            List<ChainLink3D> patent = new();
            patent.Add(new ChainLink3D(60, 180, 220, topnode, endNode));
            patent.Add(new ChainLink3D(60, 180, 220, bottomnode, endNode));

            return patent;
                
                

         




           
            



        }
    }
}
