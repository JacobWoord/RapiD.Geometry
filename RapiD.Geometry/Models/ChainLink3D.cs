using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Xml.Schema;

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
        float copies;
        [ObservableProperty]
        ObservableCollection<Element3D> elements;
        [ObservableProperty]
        float count;

        [ObservableProperty]
        Vector3 startPointVector;

        [ObservableProperty]
        Vector3 endPointVector;



        public ChainLink3D(float diameter, float width,float length, Vector3 startPointVector, Vector3 endPointVector)
        {
            this.ID=Guid.NewGuid().ToString();
            this.width = width;
            this.length = length;
            this.diameter = diameter;
            this.copies = copies;
            this.elements = new ObservableCollection<Element3D>();
            this.startPointVector = startPointVector;
            this.endPointVector = endPointVector;

            OriginalMaterial = PhongMaterials.Yellow;
            Draw();
        }


     

        public Matrix RotationMatrix(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2)
        {
            Vector3 v1 = a2 - a1;
            Vector3 v2 = b2 - b1;

            float angle = (float)Math.Acos(Vector3.Dot(v1, v2) / (v1.Length() * v2.Length()));
            Vector3 axis = Vector3.Cross(v1, v2);
            axis = Vector3.Normalize(axis);


            Matrix rotationMatrix = Matrix.RotationAxis(axis, angle);

            return rotationMatrix;



        }

        public Matrix RotationMatrix2(Vector3 a, Vector3 b)
        {
            float angle = (float)Math.Acos(Vector3.Dot(a, b) / (a.Length() * b.Length()));
            Vector3 axis = Vector3.Cross(a, b);
            axis = Vector3.Normalize(axis);

            Matrix rotationMatrix = Matrix.RotationAxis(axis, angle);
            return rotationMatrix;
        }


  



        public override void Draw()
        {
            MeshBuilder meshBuilder = new MeshBuilder();
            float radius = (width - diameter) / 2;
            float trans = 0f;
            float translate = length + (radius * 2) - diameter;
            float yoffset = 0;
            int segments = 10;
            float interval = 180 / segments;
           

            float elementlength = (length + 2 * radius - diameter);
            Vector3 buttonOffset = new Vector3(-50, 50, 50);

            Vector3 startVector = startPointVector;
            Vector3 endVector = endPointVector;

            float distanceBetweenTwoPoints = Vector3.Distance(startVector, endVector);
            float numOfCopies = MathF.Round(distanceBetweenTwoPoints / elementlength);


            Vector3 relVector = endVector - startVector;
            Vector3 yVector = Vector3.UnitY;

            //Matrix rotationMatrix = RotationMatrix2(yVector, relVector);
            Matrix rotationMatrix = RotationMatrix(Vector3.Zero, yVector, startVector, endVector);



            meshBuilder.AddSphere(Vector3.Zero, 5, 10, 10);
            meshBuilder.AddSphere(startVector, 30, 10, 10);
            meshBuilder.AddSphere(endVector, 100, 10, 10);


            Debug.WriteLine($"distance:{distanceBetweenTwoPoints}");
            Position = startVector + buttonOffset;
            Name = "ChainStructure";
            //The for loop is drawing the chainlink 

            for (int j = 0; j < numOfCopies; j++)
            {

                
                Count++;

                List<Vector3> single_chain_link = new List<Vector3>();

                for (float i = 360; i >= 0; i -= interval)
                {

                   // Debug.WriteLine($"i= {i}");
                    if (i < 180)
                        yoffset = length;
                    else
                        yoffset = 0;

                    float a = i * MathF.PI / 180;
                   // Debug.WriteLine($"a= {a}");

                    float x = radius * MathF.Cos(a);
                  //  Debug.WriteLine($"x= {x}");

                    float y = radius * MathF.Sin(a) + yoffset + trans + (radius - (diameter / 2)) ;
                  //  Debug.WriteLine($"a= {y}");


                    Vector3 vec = new Vector3(x, y, 0);
                   // Debug.WriteLine($"vec= {vec}");


                    //Rotates every second chainlink
                    if (j % 2 == 1)
                        vec = new Vector3(0, y, x);


                    var newVec = Vector3.TransformCoordinate(vec, rotationMatrix);
                    newVec += startVector;




                    single_chain_link.Add(newVec);





                }

                // this three are a reference for a new example direction in wich i want to draw the chain link to
                meshBuilder.AddTube(single_chain_link, diameter, 10, true);


                Vector3 sp = Vector3.Zero + trans * Vector3.UnitY;
                Vector3 ep = sp + elementlength * Vector3.UnitY;
                
                sp = Vector3.TransformCoordinate(sp, rotationMatrix);
                sp += startVector;
                ep = Vector3.TransformCoordinate(ep, rotationMatrix);
                ep += startVector;
                meshBuilder.AddArrow(sp, ep, 2, 10);

               

                elements.Add(new Element3D(sp, ep));
                MeshGeometry = meshBuilder.ToMeshGeometry3D();
                trans += translate;

                //single_chain_link.OrderByDescending(x => x.X);

            }



            Debug.WriteLine($"Count={Count}");







        }

    }
}
