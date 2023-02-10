using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
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

        public Matrix RotationMatrix(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2)
        {
            Vector3 v1 = a2 - a1;
            Vector3 v2 = b2 - b1;
            Vector3 v3 =v1*v2;
            v3.Normalize();

            float dot = Vector3.Dot(v1, v2);
            float v1mag = v1.Length();
            float v2mag = v2.Length();
            var cosangle = dot / (v1mag*v2mag);
            var angle = MathF.Acos(cosangle);

            float c = MathF.Cos(angle);
            float s = MathF.Sin(angle);
            float t = 1 - c;

            float r00 = c + v3.X * v3.X * t;
            float r01 = v3.X * v3.Y * t - v3.Z * s;
            float r02 = v3.X * v3.Z * t + v3.Y * s;

            float r10 = v3.Y * v3.X * t + v3.Z * s;
            float r11 = c + v3.Y * v3.Y * t;
            float r12 = v3.Y * v3.Z * t - v3.X * s;

            float r20 = v3.Z * v3.X * t - v3.Y * s;
            float r21 = v3.Z * v3.Y * t + v3.X * s;
            float r22 = c + v3.Z * v3.Z * t;

            Matrix rotationMatrix = new Matrix();
            rotationMatrix.M11 = r00;
            rotationMatrix.M12 = r01;
            rotationMatrix.M13 = r02;
            rotationMatrix.M21 = r10;
            rotationMatrix.M22 = r11;
            rotationMatrix.M23 = r12;
            rotationMatrix.M31 = r20;
            rotationMatrix.M32 = r21;
            rotationMatrix.M33 = r22;
            rotationMatrix.M44 = 1;

            return rotationMatrix;

       

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
            
            Vector3 buttonOffset = new Vector3(-50, 50, 50);
            Vector3 startVector = new Vector3(-300f, 200f, -300);
            Vector3 endVector = new Vector3(300f, 500f, 500f);

            Vector3 direction = Vector3.Normalize (startVector - endVector );

          
            Matrix rotationMatrix = RotationMatrix(Vector3.Zero, new Vector3(0, 100, 0), startVector, endVector);


     


            Position = startVector + buttonOffset;
            //The for loop is drawing the chainlink 

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
            
                    
                    //Rotates every second chainlink
                    if (j % 2 == 1)                    
                        vec =new Vector3(0, y, x);

                    var newvec = Vector3.TransformCoordinate(vec, rotationMatrix);
                    newvec += startVector;

                    
                   

                    single_chain_link.Add(newvec);
                    

                    
          

                }

                // this three are a reference for a new example direction in wich i want to draw the chain link to

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
