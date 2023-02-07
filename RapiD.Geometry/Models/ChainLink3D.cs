using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        [ObservableProperty]
        ObservableCollection<Element3D> elements;
        public ChainLink3D(float diameter, float width, float length, int copies)
        {
            this.width = width;
            this.length = length;
            this.diameter = diameter;
            this.copies = copies;
            this.elements= new ObservableCollection<Element3D>();   

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

            Vector3 startVector = new Vector3(-300, 200f, 0);
            Vector3 endVector = new Vector3(300, 500, 0);

            Vector3 direction = Vector3.Normalize (endVector - startVector);





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

                    Vector3 vec = new Vector3(x, y, 0);
            
                    

                    if (j % 2 == 1)                    
                        vec =new Vector3(0, y, x);

                    
                    vec += startVector;
                    //vec *= direction;

                    single_chain_link.Add(vec);
                    

                    
          

                }

                meshBuilder.AddSphere(Vector3.Zero, 5, 10, 10);
                meshBuilder.AddSphere(startVector, 5, 10, 10);
                meshBuilder.AddSphere(endVector, 5, 10, 10);
            

               
                meshBuilder.AddTube(single_chain_link, diameter, 10, true);
                meshBuilder.AddArrow(new Vector3(0, startPoint + trans, 0), new Vector3(0, endPoint + trans, 0), 2, 10);
                elements.Add(new Element3D(new Vector3(0, startPoint + trans, 0), new Vector3(0, endPoint + trans, 0)));

                



                //single_chain_link.OrderByDescending(x => x.X);


                MeshGeometry = meshBuilder.ToMeshGeometry3D();
                trans -= translate;
            }

        }

    }
}
