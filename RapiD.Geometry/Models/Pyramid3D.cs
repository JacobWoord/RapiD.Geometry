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
    public partial class Pyramid3D : GeometryBase3D
    {


        [ObservableProperty]
        Vector3 center = new Vector3(0,0,0);

        [ObservableProperty]
        Vector3 forward = new Vector3(0,0,0);

        [ObservableProperty]
        Vector3 up = new Vector3(0,0,0);

        [ObservableProperty]
        double sideLength = 30;

        [ObservableProperty]
        double height = 50;

        [ObservableProperty]
        bool closedBase = true;



        public Pyramid3D()
        {

            // Create transform
            Transform = new System.Windows.Media.TransformGroup();

            // Create triangle mesh for an 3d sphere
            MeshBuilder meshBuilder = new MeshBuilder();
            meshBuilder.AddPyramid(center,sideLength,height);

            // Assign mesh to model
            MeshGeometry = meshBuilder.ToMeshGeometry3D();


            // Assing model material
            OriginalMaterial = PhongMaterials.Green;




        }



    }
}
