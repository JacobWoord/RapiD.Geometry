using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RapiD.Geometry.Models
{
    public partial class Tube3D : GeometryBase3D
    {

        [ObservableProperty]
        List<Vector3> centerPoints;
        


        [ObservableProperty]
        double diameter =150;

        [ObservableProperty]
        int thetaDiv = 30;

        public Tube3D(Vector3 start, Vector3 end, float diameter=300f)
        {
            OriginalMaterial = PhongMaterials.Black;
            this.centerPoints = new List<Vector3>();
            centerPoints.Add(start);
            centerPoints.Add(end);
            this.diameter = diameter;
            Name = "Tube";
            Draw();


        }


        private void rotate(Vector3[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                vectors[i] = new Vector3(vectors[i].Z, vectors[i].Y, vectors[i].X);

            }
        }


        public override void Draw()
        {
            MeshBuilder meshBuilder = new MeshBuilder();
            MeshGeometry = meshBuilder.ToMeshGeometry3D();
            meshBuilder.AddTube(CenterPoints, Diameter, 40, false);
            MeshGeometry = meshBuilder.ToMeshGeometry3D();
           
        }


















    }
}














