using ModernWpf.Controls;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RapiD.Geometry.Models
{
    public class DoorPatent3D
    {
        public float upperChainLength;
        public float middleChainLength;
        public float BottomChainLength;
        public float diameter = 20;
       

        public float innerLength;
        public float width;
        public string name;
        public Vector3 topNode;
        public Vector3 bottomNode;
        private Vector3 middleNode;
        public Side doorSide;
        public List<Vector3> vectors =new();
        public ObservableCollection<IModel> modelCollection = new();

       public string id = Guid.NewGuid().ToString();

        List<string> linkedIds;

        public DoorPatent3D(Side side,float upperChainLength = 4000, float middleChainLength = 1000, float bottomChainLength = 4000, float width = 80, float diameter = 30, float innerLength = 80, string name = "Patent")
        {
            linkedIds = new List<string>();
            this.upperChainLength = upperChainLength;
            this.middleChainLength = middleChainLength;
            this.BottomChainLength = bottomChainLength;        
            this.innerLength = innerLength;
            this.name = name;
            this.doorSide = side;
            this.width = width;

        }

        public void InitializeModels(ObservableCollection<IModel> models)
        {
            var c1 = new ChainLink3D( Vector3.Zero, Vector3.Zero) { Name = "UpperChain", ChainType = ChainType.DoorPatentBb , PatentId = this.id };
            var c2 = new ChainLink3D( Vector3.Zero, Vector3.Zero) { Name = "BottomChain", ChainType = ChainType.DoorPatentBb, PatentId = this.id };
            var c3 = new ChainLink3D( Vector3.Zero, Vector3.Zero) { Name = "Middle", ChainType = ChainType.DoorPatentBb, PatentId = this.id};
            models.Add(c1);
            models.Add(c2);
            models.Add(c3);
            linkedIds.Add(c1.ID);
            linkedIds.Add(c2.ID);
            linkedIds.Add(c3.ID);
        }


        public void Update()
        {

            if (!Check())
            {
                MessageBox.Show("erewr");
                return;
            }
            Vector3 bottomnode = vectors[7];
            Vector3 topnode = vectors[5];
            this.topNode = topnode;
            this.bottomNode = bottomnode;
            this.middleNode = bottomnode;
            Vector3 middlenode = vectors[2];
            float elementLength = innerLength;
            float distance = Vector3.Distance(bottomnode, topnode);


            //Creates Upper Chain
            float averageNumberOfLinksUP = upperChainLength / (innerLength + diameter * 2);
            float finalNumberOfLinklsUP = MathF.Round(averageNumberOfLinksUP);
            float finalChainLengthUp = finalNumberOfLinklsUP * elementLength;


            //Creates Bottom Chain
            float averageNumberOfLinksBOT = BottomChainLength / (innerLength + diameter * 2);
            float finalNumberOfLinklsBOT = MathF.Round(averageNumberOfLinksBOT);
            float finalChainLengthBOT = finalNumberOfLinklsBOT * elementLength;

            //Side lengths in mm
            float upperLength = finalChainLengthUp;
            float bottomLength = finalChainLengthBOT;



            float cos_bottom = (bottomLength * bottomLength + distance * distance - upperLength * upperLength) / (2 * bottomLength * distance);
            float angle_bottom = MathF.Acos(cos_bottom);

            float transwidht = bottomLength * MathF.Sin(angle_bottom);
            float transheigth = bottomLength * cos_bottom;

            Vector3 endNode = bottomnode + new Vector3(0, -transwidht, transheigth);


            var c1 = modelCollection.Where(x => x.ID == linkedIds[0]).First();
            (c1 as ChainLink3D).UpdatePositions(topnode, endNode);
            var c2 = modelCollection.Where(x => x.ID == linkedIds[1]).First();
            (c2 as ChainLink3D).UpdatePositions(bottomnode, endNode);
            var c3 = modelCollection.Where(x => x.ID == linkedIds[2]).First();
            (c3 as ChainLink3D).UpdatePositions(middlenode, endNode);


        }

        public void Update(List<Vector3> vector3s, ObservableCollection<IModel> models)
        {
            this.modelCollection = models;
            this.vectors = vector3s;


            if (!Check())
            {
                MessageBox.Show("erewr");
                return;
            }
            Vector3 bottomnode = vector3s[7];
            Vector3 topnode = vector3s[5];
            this.topNode = topnode;
            this.bottomNode = bottomnode;
            this.middleNode = bottomnode;
            Vector3 middlenode = vector3s[2];
            float elementLength = innerLength;
            float distance = Vector3.Distance(bottomnode, topnode);


            //Creates Upper Chain
            float averageNumberOfLinksUP = upperChainLength / (innerLength + diameter * 2);
            float finalNumberOfLinklsUP = MathF.Round(averageNumberOfLinksUP);
            float finalChainLengthUp = finalNumberOfLinklsUP * elementLength;


            //Creates Bottom Chain
            float averageNumberOfLinksBOT = BottomChainLength / (innerLength + diameter * 2);
            float finalNumberOfLinklsBOT = MathF.Round(averageNumberOfLinksBOT);
            float finalChainLengthBOT = finalNumberOfLinklsBOT * elementLength;

            //Side lengths in mm
            float upperLength = finalChainLengthUp;
            float bottomLength = finalChainLengthBOT;



            float cos_bottom = (bottomLength * bottomLength + distance * distance - upperLength * upperLength) / (2 * bottomLength * distance);
            float angle_bottom = MathF.Acos(cos_bottom);

            float transwidht = bottomLength * MathF.Sin(angle_bottom);
            float transheigth = bottomLength * cos_bottom;

            Vector3 endNode = bottomnode + new Vector3(0, -transwidht, transheigth);


            var c1 = models.Where(x => x.ID == linkedIds[0]).First();
            (c1 as ChainLink3D).UpdatePositions(topnode, endNode);
            var c2 = models.Where(x => x.ID == linkedIds[1]).First();
            (c2 as ChainLink3D).UpdatePositions(bottomnode, endNode);
            var c3 = models.Where(x => x.ID == linkedIds[2]).First();
            (c3 as ChainLink3D).UpdatePositions(middlenode, endNode);

        }







        public bool Check()
        {

            float distanceBetweenNodes = Vector3.Distance(topNode, bottomNode);

            if (upperChainLength + BottomChainLength < distanceBetweenNodes)
            {
                return false;            
            }
            else if (upperChainLength > BottomChainLength + upperChainLength)
            {
               return false;
            }
            else if (BottomChainLength > upperChainLength + BottomChainLength)
            {
                
                return false;   
            }
            else if ( upperChainLength + BottomChainLength < distanceBetweenNodes)
            {

                return false;
            }
            return true;
        }

        public void AddLinkToBottomChain()
        {


        }
    }
}



























