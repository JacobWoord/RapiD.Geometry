using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml;
using Quaternion = SharpDX.Quaternion;

namespace RapiD.Geometry.Models
{
    public partial class Squared3D : GeometryBase3D
    {
     
        public Vector3 Size { get; set; }
        public Vector3 BbUpperPoint;
        public Vector3 BbMiddlePoint;
        public Vector3 BbBottomPoint;
       
        public Vector3 SbUpperPoint;
        public Vector3 SbMiddlePoint;
        public Vector3 SbBottomPoint;
       
        public Vector3 CenterPoint;

        public Squared3D(Vector3 position, Vector3 size)
        {
            ID = Guid.NewGuid().ToString();
            OriginalMaterial = PhongMaterials.Blue;
            Position = position;
            Size = size;
            Name = "Squared Box";
            Draw();
        }


        public void UpdatePoints()
        {

            BbUpperPoint = new Vector3(-Size.X / 2, Size.Y / 2, Size.Z / 2) + Position;
            BbMiddlePoint = new Vector3(Position.X - Size.X / 2, Position.Y + Size.Y / 2, Position.Z);
            BbBottomPoint = new Vector3(-Size.X / 2, Size.Y / 2, -Size.Z / 2) + Position;

            SbUpperPoint = new Vector3(Size.X / 2, Size.Y / 2, Size.Z / 2) + Position;
            SbMiddlePoint = new Vector3(Position.X + Size.X / 2, Position.Y + Size.Y / 2, Position.Z);
            SbBottomPoint = new Vector3(Size.X / 2, Size.Y / 2, -Size.Z / 2) + Position;
            float offSetY = 5000;
            float offSetX = 5000;
            CenterPoint = new Vector3(Position.X + Size.X / 2 + -offSetX, Position.Y + Size.Y / 2 + -offSetY, Position.Z - 1000);

        }











       


        //public List<ChainLink3D> CreateNetPatent(Side defineSide)
        //{

          
        //    Vector3 topNode;
        //    Vector3 center = ListPoints[7].Position;
        //    Vector3 middle;
        //    Vector3 bottomNode;
       



        //    if (defineSide == Side.StarBoard)
        //    {
        //        topNode = ListPoints[0].Position;
        //        middle = ListPoints[1].Position;
        //        bottomNode = ListPoints[2].Position;

        //    }
        //    else
        //    {
        //        topNode = ListPoints[3].Position;
        //        middle = ListPoints[4].Position;
        //        bottomNode = ListPoints[5].Position;
        //    }
           

        //    float topLength = 5000;
        //    float bottomLength = 5000;

        

        //    //define plane trough seamline
        //    Plane p1 = new Plane(center, middle, middle+new Vector3(0,0,1000));
        //    Patent3D patent = new Patent3D(bottomNode, topNode, middle, bottomLength, topLength, p1);
 

        //    List<ChainLink3D> NetPatent = new();
        //    NetPatent.Add(new ChainLink3D( patent.topPoint, patent.targetPoint));
        //    NetPatent.Add(new ChainLink3D( patent.bottomPoint, patent.targetPoint));

        //    patent.lengthbottom= 10000;
        //    patent.UpdatePatent();




        //    return NetPatent;

        //}

        public static float AngleBetween(Plane firstPlane, Plane secondPlane)
        {
            float dotProduct = Vector3.Dot(firstPlane.Normal, secondPlane.Normal);
            float angle = (float)Math.Acos(dotProduct);

            return angle;

        }

        public static Matrix rotationMatrixPlaneVector(Vector3 lineStart, Vector3 lineEnd, Plane plane)
        {
            Vector3 startPoint = lineStart; 
            Vector3 endPoint = lineEnd;
            Vector3 planeNormal = plane.Normal;

            // Step 1
            Vector3 lineVector = endPoint - startPoint;

            // Step 2
            lineVector.Normalize();

            // Step 3
            Vector3 planePerpendicular = Vector3.Cross(lineVector, planeNormal);

            // Step 4
            planePerpendicular.Normalize();

            // Step 5
            Vector3 thirdVector = Vector3.Cross(lineVector, planePerpendicular);

            // Step 6
            Matrix rotationMatrix = new Matrix(
                lineVector.X, planePerpendicular.X, thirdVector.X, 0,
                lineVector.Y, planePerpendicular.Y, thirdVector.Y, 0,
                lineVector.Z, planePerpendicular.Z, thirdVector.Z, 0,
                0, 0, 0, 1);

            return rotationMatrix;
        }




        public static Vector3 rotateVectorAboutAxis(Vector3 axis1, Vector3 axis2, Vector3 vectorToRotate, float angle)
        {
            // Define the point to be rotated
            Vector3 point = new Vector3(vectorToRotate.X, vectorToRotate.Y, vectorToRotate.Z);

            // Define two points that define the axis of rotation
            Vector3 point1 = axis1;
            Vector3 point2 = axis2;

            // Calculate the translation vector that will move the line to pass through the origin
            Vector3 translation = -point1;

            // Translate the point and the line
            point += translation;
            point1 += translation;
            point2 += translation;

            // Calculate the axis of rotation and angle of rotation as before
            Vector3 axis = Vector3.Normalize(point2 - point1);


            // Create the rotation quaternion
            Quaternion rotation = Quaternion.RotationAxis(axis, angle);

            // Apply the rotation to the point
            Vector3 rotatedPoint = Vector3.Transform(point, rotation);

            // Translate the rotated point back to its original position
            rotatedPoint -= translation;

            return rotatedPoint;

        }




        public static Vector3 findThirdPoint(Vector3 p1, Vector3 p2, float l1, float l2, Plane plane)
        {
            //get length of third rib
            float l3 = Vector3.Distance(p1,p2);

            //calculate angel between first rib and third rib
            float angle =MathF.Acos((l1 * l1 + l3 * l3 - l2 * l2) /(2 * l1 * l3));

            //define direction between two known points
            Vector3 direction = Vector3.Normalize(p2 - p1);
            //set rotationaxis to the crossproduct of these two point
            Vector3 rotationaxis = Vector3.Normalize(Vector3.Cross(p1, p2));
     

            //define the direction of the first rib and calculate the location of the third point 
            Matrix rotation = Matrix.RotationAxis(rotationaxis, angle);
            Vector3 rib1Direction = Vector3.TransformNormal(direction, rotation);

            Vector3 p3 = p1 + rib1Direction * l1;

            //float anglewithplane = AngleBetween(plane, new Plane(p2, p1, p3));

            //loop from 0 to 360 degree to find the value that is the most close to the known plane
            float maxdif = float.MaxValue;
            float angletorotate = 0f;
          
            for (float i = 0; i <= 2*MathF.PI; i+=0.01f)
            {
                Vector3 newvec = rotateVectorAboutAxis(p1, p2, p3, i);
                float dist = DistancePointToPlane(newvec, plane);


                if (dist < maxdif)
                {
                    maxdif = dist;
                    angletorotate = i;
                }
            }
        
           return rotateVectorAboutAxis(p1, p2, p3, angletorotate);

        }


        public static float DistancePointToPlane(Vector3 point, Plane plane)
        {
            // Calculate the distance from the point to the plane using the plane equation:
            // ax + by + cz + d = 0
            // Where a, b, and c are the components of the plane's normal vector, and d is the distance from the origin to the plane.
            float distance = Vector3.Dot(plane.Normal, point) + plane.D;

            // Return the absolute value of the distance, since the sign indicates which side of the plane the point is on.
            return Math.Abs(distance) / plane.Normal.Length();
        }




        public override void Draw()
        {
            MeshBuilder meshBuilder = new MeshBuilder();

            meshBuilder.AddBox(Position,Size.X,Size.Y,Size.Z);

            UpdatePoints();

            MeshGeometry = meshBuilder.ToMeshGeometry3D();
        }
    }
}
