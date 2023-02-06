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
    public partial class ChainLink3D : GeometryBase3D 
    {
        [ObservableProperty]
        float radius;
        [ObservableProperty]
        float width;
        [ObservableProperty]
        float diameter;
        [ObservableProperty]
        float length;
        [ObservableProperty]
        int copies;
        public ChainLink3D(float diameter, float width, float length, int copies)
        {
            this.width = width;
            
            this.length = length;
            this.diameter = diameter;
            this.copies = copies;

            OriginalMaterial = PhongMaterials.Chrome;
            DrawChainLink();
        }


        public void DrawChainLink()
        {
            MeshBuilder meshBuilder = new MeshBuilder();
            float radius = (width - diameter) / 2;
            float trans = 0f;
            float translate = length + (radius * 2) - diameter;
            float yoffset = 0;
            int segments = 10;
            float interval = 180 / segments;
            int numOfCopies = copies;
            float startPoint = radius - (diameter / 2);
            float endPoint = -length -radius + (diameter / 2);





            for (int j = 0; j < numOfCopies; j++)
            {

                List<Vector3> single_chain_link = new List<Vector3>();

                for (float i = 0; i <= 360; i += interval)
                {
                    if (i > 180)
                        yoffset = -length;
                    else
                        yoffset = 0;

                    float a = i * MathF.PI / 180;
                    float x = radius * MathF.Cos(a);
                    float y = radius * MathF.Sin(a) + yoffset + trans;

                    if (j % 2 == 1)
                    {
                        single_chain_link.Add(new Vector3(0, y, x));

                    }
                    else
                        single_chain_link.Add(new Vector3(x, y, 0));


                }
               
                meshBuilder.AddTube(single_chain_link, diameter, 10, true);
                //meshBuilder.AddCylinder(new Vector3(0, startPoint , 0), new Vector3(0, endPoint ,0),5,10);


                meshBuilder.AddArrow(new Vector3(0, startPoint + trans, 0), new Vector3(0, endPoint + trans, 0), 2, 10);



                //single_chain_link.OrderByDescending(x => x.X);


                MeshGeometry = meshBuilder.ToMeshGeometry3D();
                trans -= translate;
            }

        }

    }
}
