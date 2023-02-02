using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RapiD.Geometry.Models
{
    public partial class  Triange_3D : GeometryBase3D
    {

        [ObservableProperty]
        Vector3 point1= new Vector3(50,50,50);

        [ObservableProperty]
        Vector3 point2 = new Vector3(100,100,100);

        [ObservableProperty]
        Vector3 point3 = new Vector3(200,200,200);




        public Triange_3D(Vector3 p1 , Vector3 p2, Vector3 p3)
        {

            this.point1= p1;
            this.point2= p2;    
            this.point3= p3;    

            Transform = new Transform3DGroup();

            MeshBuilder meshbuilder  = new MeshBuilder();
            meshbuilder.AddTriangle(point1, point2, point3);    

            MeshGeometry = meshbuilder.ToMeshGeometry3D();


            OriginalMaterial = PhongMaterials.Yellow;



        }
    }
}
