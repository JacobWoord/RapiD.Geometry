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
        private float upperChainLength;
        private float middleChainLength;
        private float BottomChainLength;
        private float diameter;
        private float innerLength;
        private string name;


        string id = Guid.NewGuid().ToString();

        List<string> linkedIds;

        public DoorPatent3D(float upperChainLength = 4000, float middleChainLength = 1000, float bottomChainLength = 8000, float diameter = 16, float innerLength = 60, string name = "Patent")
        {
            linkedIds = new List<string>();
            this.upperChainLength = upperChainLength;
            this.middleChainLength = middleChainLength;
            this.BottomChainLength = bottomChainLength;
            this.diameter = diameter;
            this.innerLength = innerLength;
            this.name = name;

        }

        public void InitializeModels(ObservableCollection<IModel> models)
        {
            var c1 = new ChainLink3D(20, 70, 100, Vector3.Zero, Vector3.Zero) { Name = "UpperChain", ChainType = ChainType.DoorPatent };
            var c2 = new ChainLink3D(20, 70, 100, Vector3.Zero, Vector3.Zero) { Name = "BottomChain", ChainType = ChainType.DoorPatent };
            var c3 = new ChainLink3D(20, 70, 100, Vector3.Zero, Vector3.Zero) { Name = "Middle", ChainType = ChainType.DoorPatent };
            models.Add(c1);
            models.Add(c2);
            models.Add(c3);
            linkedIds.Add(c1.ID);
            linkedIds.Add(c2.ID);
            linkedIds.Add(c3.ID);
        }
        public void Update(List<Vector3> vector3s, ObservableCollection<IModel> models)
        {
            if (!Check())
                return;

            Vector3 bottomnode = vector3s[7];
            Vector3 topnode = vector3s[5];
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
            return true;
            //var nodeList = door.GetNodeList();
            //float distanceBetweenNodes = Vector3.Distance(nodeList[5], nodeList[7]);
            //if (upperChainLength + BottomChainLength < distanceBetweenNodes)
            //{
            //    MessageBox.Show("fuck");
            //}
            //else if (upperChainLength > BottomChainLength + upperChainLength)
            //{
            //    MessageBox.Show("fuck");
            //}
            //else if (BottomChainLength > upperChainLength + BottomChainLength)
            //{
            //    MessageBox.Show("fuck");
            //}
        }

        public void AddLinkToBottomChain()
        {


        }
    }
}



























