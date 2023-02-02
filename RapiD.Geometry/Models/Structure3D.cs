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
    public partial class Structure3D : GeometryBase3D
    {
        [ObservableProperty]
        Vector3 cilinderpos1 = new Vector3(-30,30,20); 

        [ObservableProperty]
        Vector3 cilinderpos2 = new Vector3(20, -20, 20);

        [ObservableProperty]
        double diameter = 20;

        public Structure3D()
        {
            OriginalMaterial = PhongMaterials.Chrome;
            DrawStructure();
        }

        public void DrawStructure()
        {


            MeshBuilder meshbuilder = new MeshBuilder();
            MeshGeometry = meshbuilder.ToMeshGeometry3D();
            meshbuilder.AddCylinder(cilinderpos1, cilinderpos2, diameter);
            meshbuilder.AddCylinder(new Vector3(-300,30,20), new Vector3(55,-40,60), diameter);



            //for(int j = 0; j < 50; j++)
            //{
            //int count = 0;    

            //    Vector3 updatedPos1 = cilinderpos1;
            //    Vector3 updatedPos2 = cilinderpos2;


            //    if(count > 2)
            //{
            //    count = 0;
            //    updatedPos1.Z -= 30;
            //    updatedPos2.Z += 30;
            //    updatedPos1.X -= 30;
            //    updatedPos1.Y -= 30;
            //    updatedPos2.X -= 30;
            //    updatedPos2.Y -= 30;
            //}
            //else
            //{
            //    updatedPos1.Z += 30;
            //    updatedPos2.Z -= 30;
            //    updatedPos1.X += 30;
            //    updatedPos1.Y += 30;
            //    updatedPos2.X += 30;
            //    updatedPos2.Y += 30;
            //}


            //    cilinderpos1 = updatedPos1;
            //    cilinderpos2 = updatedPos2;

            //    meshbuilder.AddCylinder(cilinderpos1, cilinderpos2, diameter);
            //    count++;
            //}




        }





    }
}
