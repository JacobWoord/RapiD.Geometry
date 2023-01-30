using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Models
{
    public partial class Sphere3D :GeometryBase3D
    {

        [ObservableProperty]
        float radius=50;
        public Sphere3D()
        {
            // Create transform
            Transform = new System.Windows.Media.TransformGroup();

            // Create triangle mesh for an 3d sphere
            MeshBuilder meshBuilder = new MeshBuilder();
            meshBuilder.AddSphere(Vector3.Zero, radius);

            // Assign mesh to model
            MeshGeometry=meshBuilder.ToMeshGeometry3D();
          

            // Assing model material
            Material = PhongMaterials.Green;
        }

    }
}
