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
    public partial class Cillinder3D : GeometryBase3D
    {

        [ObservableProperty]
        float diameter = 30;


        [ObservableProperty]
        Vector3 p1 = new Vector3(50, 90, 150);

        [ObservableProperty]
        Vector3 p2 = new Vector3(200, 100, 200);


        public Cillinder3D(Vector3 p1, Vector3 p2)
        {
            this.p1 = p1;
            this.p2 = p2;

            // Create transform
            // Assing model material
            OriginalMaterial = PhongMaterials.Blue;

            DrawCilinder();
        }

        public void DrawCilinder()
        {
            // Create triangle mesh for an 3d Cillinder
            MeshBuilder meshBuilder = new MeshBuilder();
            meshBuilder.AddCylinder(p1, p2, diameter);
            // Assign mesh to model
            MeshGeometry = meshBuilder.ToMeshGeometry3D();
        }



    }
}

















