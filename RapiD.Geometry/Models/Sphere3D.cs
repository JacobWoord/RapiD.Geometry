using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RapiD.Geometry.Models
{
    public partial class Sphere3D :GeometryBase3D
    {

        [ObservableProperty]
        float radius=500;

        [ObservableProperty]
        int nodeNumber;
        public Sphere3D(Vector3 position)
        {

            Position= position;
           
            // Assing model material
            OriginalMaterial = PhongMaterials.Red;

            Draw();
 
        }
           
            
 


        public override void Draw()
        {


            // Create triangle mesh for an 3d sphere
            MeshBuilder meshBuilder = new MeshBuilder();
            meshBuilder.AddSphere(Position, radius);

            // Assign mesh to model
            MeshGeometry = meshBuilder.ToMeshGeometry3D();


        }

    }
}
