using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace RapiD.Geometry.Models
{
    public partial class CablePatent : GeometryBase3D

    {
        private List<Vector3> Centerpoints = new();
        private float radius;


        public CablePatent(List<Vector3> centerpoints, float radius = 150)
        {
            this.radius = radius;
            this.Centerpoints = centerpoints;
            this.ID = Guid.NewGuid().ToString();
            Draw();
            OriginalMaterial = PhongMaterials.BlackRubber;

        }
        public override void Draw()
        {
            MeshBuilder meshBuilder = new MeshBuilder();
            meshBuilder.AddTube(Centerpoints, radius, 50, true);

            MeshGeometry = meshBuilder.ToMeshGeometry3D();
        }
    }
}

   






























