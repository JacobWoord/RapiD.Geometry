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
    public partial class InfoButton3D: GeometryBase3D
    {

        [ObservableProperty]
        double radius = 60 ;


        [ObservableProperty]
        Vector3 position2;

        [ObservableProperty]
        int idNumber;

        public InfoButton3D(Vector3 position, int linkedGuid)
        {
            Name = "InfoButton";
            this.idNumber= linkedGuid;
            this.position2 = position;
            OriginalMaterial= PhongMaterials.Red;
            DrawButtonOnGeometry(position);
            

        }


        [RelayCommand]
        void DrawButtonOnGeometry(Vector3 position)
        {
            MeshBuilder meshbuilder = new MeshBuilder();
            //meshbuilder.AddSphere(position,Radius);
            meshbuilder.AddSphere(position, 20d);
            MeshGeometry = meshbuilder.ToMeshGeometry3D();


        }


    }
}
