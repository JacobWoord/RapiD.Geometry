using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Models
{
    public partial class Squared3D : GeometryBase3D
    {


        private Vector3 centerPosition;
        private float degrees;
        
        public Squared3D(Vector3 position, float rotateZDegrees = 0)
        {
            degrees = rotateZDegrees;
            OriginalMaterial = PhongMaterials.Black;
            Position= position;
            Name = "Squared Box";
            RotateAroundModelCenter(yaxis: 1 , degrees: degrees);
            Draw();
        }


        public override void Draw()
        {
            MeshBuilder meshBuilder = new MeshBuilder();

            meshBuilder.AddBox(Position,20000,20000, 500);
            
            MeshGeometry = meshBuilder.ToMeshGeometry3D();



        }
    }
}
