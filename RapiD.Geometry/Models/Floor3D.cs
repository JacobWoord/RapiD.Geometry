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
    public partial class Floor3D : GeometryBase3D
    {

        public Vector3 Size { get; set; }

        public Floor3D(Vector3 position,Vector3 size)
        {
            Id = Guid.NewGuid().ToString();
            OriginalMaterial = PhongMaterials.Bisque;
            Position = position;
            Size = size;
            Name = "Squared Box";
            Draw();
        }

        public override void Draw()
        {
            MeshBuilder meshBuilder = new MeshBuilder();

            meshBuilder.AddBox(Position, Size.X, Size.Y, Size.Z);

            MeshGeometry = meshBuilder.ToMeshGeometry3D();
        }


    }
}
