using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Models
{
    public partial class SecondDoor : GeometryBase3D
    {

    
        [ObservableProperty]
        List<Vector3> nodeList = new();
       
        
        
        
        public SecondDoor(string filename)
        {
            MeshBuilder meshBuilder = new MeshBuilder();
         
            OriginalMaterial = PhongMaterials.Yellow;

        }



    }
}
