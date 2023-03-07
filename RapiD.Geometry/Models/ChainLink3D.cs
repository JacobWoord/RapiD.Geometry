using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Xml.Schema;
using Xceed.Wpf.Toolkit.Converters;

namespace RapiD.Geometry.Models
{
    public partial class ChainLink3D : GeometryBase3D
    {



        private float upperChainLength;
        private float bottomChainLength;
        private float middleChainLength;
        private float radius;

        [ObservableProperty]
        private float width;
       
        
        
        [ObservableProperty]
        float diameter;


        [ObservableProperty]
        float elementlength; // is innerlength of link
        

        [ObservableProperty]
        public ChainType chainType;

        [ObservableProperty]
        List<ChainLink3D> patent = new();
       
        

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

        [ObservableProperty]
        float chainLength;

        [ObservableProperty]
        float numberOfCopies;

        [ObservableProperty]
        string patentId;

        public string ConnectionId;

        partial void OnEndPointVectorChanged(Vector3 value)
        {
            Draw();
        }


        public ChainLink3D(Vector3 startPointVector , Vector3 endPointVector, float chainLengthCm = 2000, float diameter = 20, float width = 50, float innerLength = 100)
        {
            this.ID=Guid.NewGuid().ToString();
            this.width = width;
            this.copies = copies;
            this.elements = new ObservableCollection<Element3D>();
            this.startPointVector = startPointVector;
            this.endPointVector = endPointVector;

            //Calculation
           
            this.elementlength = innerLength;
            this.diameter = diameter;


            OriginalMaterial = PhongMaterials.Yellow;
            Draw();
        }


        public List<ChainLink3D> CreateDoorPatent(List<Vector3> vectors,float upperChainLength=4000, float bottomChainLength = 4000, float middelChainLength = 3000)
        {
            //List<ChainLink3D> patentToReturn = new();

            this.upperChainLength = upperChainLength;
            this.middleChainLength = middelChainLength;
            this.bottomChainLength = bottomChainLength;
            Vector3 topNode = vectors[7];
            Vector3 bottomNode = vectors[5];
            Vector3 middleNode = vectors[2];
            float distance = Vector3.Distance(topNode, bottomNode);

            //Creates Upper Chain
            float averageNumberOfLinksUP = upperChainLength / (elementlength + diameter * 2);
            float finalNumberOfLinklsUP = MathF.Round(averageNumberOfLinksUP);
            float finalChainLengthUp = finalNumberOfLinklsUP * elementlength;


            //Creates Bottom Chain
            float averageNumberOfLinksBOT = bottomChainLength / (elementlength + diameter * 2);
            float finalNumberOfLinklsBOT = MathF.Round(averageNumberOfLinksBOT);
            float finalChainLengthBOT = finalNumberOfLinklsBOT * elementlength;

            //Side lengths in mm
            float upperLength = finalChainLengthUp;
            float bottomLength = finalChainLengthBOT;



            float cos_bottom = (bottomLength * bottomLength + distance * distance - upperLength * upperLength) / (2 * bottomLength * distance);
            float angle_bottom = MathF.Acos(cos_bottom);

            float transwidht = bottomLength * MathF.Sin(angle_bottom);
            float transheigth = bottomLength * cos_bottom;

            Vector3 endNode = bottomNode + new Vector3(0, -transwidht, transheigth);

            patent.Add(new ChainLink3D( topNode, endNode, upperChainLength));
            patent.Add(new ChainLink3D( bottomNode, endNode, bottomChainLength));
            patent.Add(new ChainLink3D( middleNode, endNode, middelChainLength));

            return patent;
        }












        public float CalcChainLength()
        {
            return Vector3.Distance(startPointVector, endPointVector);
        }

       

        public float CalcDistance()
        {
            float distance = Vector3.Distance(startPointVector, endPointVector);

            return distance; 

        }
        
     
        public void SetNewEndPosition(Vector3 newEndPos)
        {
            this.EndPointVector = newEndPos;
        }

        public void SetNewStartPosition(Vector3 newStartPos)
        {
            this.StartPointVector = newStartPos;
        }


        //public void SetNewPosition(Vector3 newStartPos)
        //{
        //    this.StartPointVector = newStartPos;
        //}




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

        public void UpdateProperties( float diameter, float width, float innerLength)
        {
            this.Elementlength = innerLength;
            this.width= width;
            this.diameter =diameter;
            Draw();
        }

          public void UpdatePositions(Vector3 start,Vector3 end)
        {
            startPointVector = start;
            endPointVector = end;
            this.chainLength = Vector3.Distance(startPointVector, endPointVector);
            Draw();
        }
        public override void Draw()
        {
            if (startPointVector == endPointVector)
                return;
            MeshBuilder meshBuilder = new MeshBuilder();
            this.radius = (width - diameter) / 2;
            this.ChainLength = Vector3.Distance(startPointVector, endPointVector); 
            float length = this.elementlength + diameter - 2 * radius;
            float trans = 0f;
            float translate = length + (radius * 2) - diameter;
            float yoffset = 0;
            int segments = 10;
            float interval = 180 / segments;

            



            Vector3 buttonOffset = new Vector3(-50, 50, 50);

            Vector3 startVector = startPointVector;
            Vector3 endVector = endPointVector;

            float distanceBetweenTwoPoints = Vector3.Distance(startVector, endVector);
            float numOfCopies = MathF.Round(distanceBetweenTwoPoints / elementlength);
            this.numberOfCopies = numOfCopies;


            Vector3 relVector = endVector - startVector;
            Vector3 yVector = Vector3.UnitY;

            //Matrix rotationMatrix = RotationMatrix2(yVector, relVector);
            Matrix rotationMatrix = RotationMatrix(Vector3.Zero, yVector, startVector, endVector);



            meshBuilder.AddSphere(Vector3.Zero, 5, 10, 10);
            meshBuilder.AddSphere(startVector, 30, 10, 10);
            meshBuilder.AddSphere(endVector, 100, 10, 10);


            Debug.WriteLine($"distance:{distanceBetweenTwoPoints}");
            Position = startVector + buttonOffset;
           
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
