using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media.Media3D;

namespace RapiD.Geometry.Models
{
    public partial class Door : BatchedModel
    {

        [ObservableProperty]
        Transform3DGroup transform3DGroup = new();

        [ObservableProperty]
        float upperChainLength = 2000;

        [ObservableProperty]
        float middleChainLength = 2000 ;

        [ObservableProperty]
        float bottomChainLength = 2000;

        [ObservableProperty]
        float innerChainLength = 40;

        public Side doorSide;



        public Door(string filename, string name = "bord")
        {
            Name = name + "Bord";
            ID = Guid.NewGuid().ToString();

            FileName= filename;
        }

        
        public Warp3D DrawWarp()
        {
            var nodeList = GetNodeList();
            List<Vector3> centerPoints= new();
            centerPoints.Add(nodeList[3]);
            centerPoints.Add(new Vector3(nodeList[3].X, nodeList[3].Y +100000, nodeList[3].Z));
            var warp = new Warp3D(centerPoints);
            return warp;
        }
        
        
       



        public void ChangePosition()
        {


        }


        public void Draw()
        {


        }
       

    }
}
