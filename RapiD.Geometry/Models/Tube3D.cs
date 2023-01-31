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
    public partial class Tube3D : GeometryBase3D
    {

        [ObservableProperty]
        List<Vector3> centerPoints = new List<Vector3>()
        {
             new Vector3(12, -33, 1),
             new Vector3(4, 16, -10),
             new Vector3(24, 6, 32),
             new Vector3(121, 22, 11),
             new Vector3(44, -16, -56),
             new Vector3(50, 66, 77)

        };

        [ObservableProperty]
        double diameter;

        [ObservableProperty]
        int thetaDiv = 30;

        public Tube3D()
        {

            //centerPoints= new List<Vector3>();
            //centerPoints.Add(new Vector3(20,20,30));
            //centerPoints.Add(new Vector3(60,70,100));
            //centerPoints.Add(new Vector3(50,120,60));
            
            Transform = new System.Windows.Media.TransformGroup();

            MeshBuilder meshBuilder= new MeshBuilder();

            meshBuilder.AddTube(centerPoints, 10, 10, true);

            MeshGeometry = meshBuilder.ToMeshGeometry3D();

            OriginalMaterial = PhongMaterials.Blue;


        }
    }
}
