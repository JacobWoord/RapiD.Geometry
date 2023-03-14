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

       
        public Vector3 TopNode;

        
        [ObservableProperty]
        Vector3 middleNode;

        [ObservableProperty]
        Vector3 bottomNode;

        public Side doorSide;

        [ObservableProperty]
        List<Vector3> nodeList = new List<Vector3>();
      



        public Door(string filename, string name = "bord")
        {
            Name = name + "Bord";
            Id = Guid.NewGuid().ToString();

            FileName= filename;

            
        }

        

        
        public Vector3 GetTopNode()
        {
            var nodelist = GetNodeList();
            TopNode = nodelist[5];
            return TopNode;
        }

        public Vector3 GetBottomNode()
        {
            var nodelist = GetNodeList();
            BottomNode = nodelist[7];
            return BottomNode;
        }

        public Vector3 GetMiddleNode()
        {
            var nodelist = GetNodeList();
            MiddleNode = nodelist[2];
            return MiddleNode;
        }





        public void ChangePosition()
        {


        }


        public void Draw()
        {


        }
       

    }
}
