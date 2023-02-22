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
    public partial class InfoButton3D : GeometryBase3D
    {

        [ObservableProperty]
        double radius = 60;

        [ObservableProperty]
        string idNumber;

        public InfoButton3D(Vector3 position, string linkedGuid)
        {
            Name = "InfoButton";
            ID = linkedGuid;
            this.idNumber = linkedGuid.ToString();
            this.Position = position;
            OriginalMaterial = PhongMaterials.Red;
            Draw();


        }

        public override void Draw()
        {
            MeshBuilder meshbuilder = new MeshBuilder();
            meshbuilder.AddSphere(Position,Radius);
            MeshGeometry = meshbuilder.ToMeshGeometry3D();


        }


    }
}
