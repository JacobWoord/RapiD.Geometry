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
using Quaternion = SharpDX.Quaternion;

namespace RapiD.Geometry.Models
{
    public partial class Squared3D : GeometryBase3D
    {
        [ObservableProperty]
        List<Sphere3D> listPoints = new();

        public Vector3 Size { get; set; }


        public Squared3D(Vector3 position, Vector3 size)
        {
            OriginalMaterial = PhongMaterials.Blue;
            Position = position;
            Size = size;
            Name = "Squared Box";
            Draw();
        }

        public List<Sphere3D> AddNetPoints()
        {
            Vector3 bbUpperPoint = new Vector3(-Size.X / 2, Size.Y / 2, Size.Z / 2);
            Vector3 bbBottomPoint = new Vector3(-Size.X / 2, Size.Y / 2, -Size.Z / 2);

            Vector3 SbUpperPoint = new Vector3(Size.X / 2, Size.Y / 2, Size.Z / 2);
            Vector3 SbBottomPoint = new Vector3(Size.X / 2, Size.Y / 2, -Size.Z / 2);
            float offSetY = 5000;
            float offSetX = 5000;
            var centerPoint = new Vector3(Position.X + Size.X / 2 + -offSetX, Position.Y + Size.Y / 2 + -offSetY, Position.Z);
            List<Sphere3D> NetSquared;
            NetSquared = new List<Sphere3D>();
             NetSquared.Add(new Sphere3D(bbUpperPoint + Position) { NodeNumber = 0 });  
            NetSquared.Add(new Sphere3D(new Vector3(Position.X - Size.X / 2, Position.Y + Size.Y / 2, Position.Z)) { NodeNumber = 1 });
            NetSquared.Add(new Sphere3D(bbBottomPoint + Position) { NodeNumber = 2 });
            NetSquared.Add(new Sphere3D(SbUpperPoint + Position) { NodeNumber = 3 });
            NetSquared.Add(new Sphere3D(new Vector3(Position.X + Size.X / 2, Position.Y + Size.Y / 2, Position.Z)) { NodeNumber = 4 });
            NetSquared.Add(new Sphere3D(SbBottomPoint + Position) { NodeNumber = 5 });
            NetSquared.Add(new Sphere3D(SbBottomPoint + Position) { NodeNumber = 6 });

            var newcenter = centerPoint - new Vector3(0, 0, 1000);
            NetSquared.Add(new Sphere3D(newcenter) { NodeNumber = 8 });

            foreach (var item in NetSquared)
            {
                ListPoints.Add(item);
            }
            return NetSquared;

        }


        public List<ChainLink3D> CreateNetPatent(Side defineSide)
        {
            Vector3 topNode;
            Vector3 center = ListPoints[7].Position;
            Vector3 middle;
            Vector3 bottomNode;

            Vector3 newnode;



            if (defineSide == Side.StarBoard)
            {
                topNode = ListPoints[0].Position;
                middle = ListPoints[1].Position;
                bottomNode = ListPoints[2].Position;

            }
            else
            {
                topNode = ListPoints[3].Position;
                middle = ListPoints[4].Position;
                bottomNode = ListPoints[5].Position;
            }
            
            
            
            //OutLine Direction ( Direction to point the roration to)
            float lineLength = 100000;
           
            Vector3 direction = middle - center;
            Vector3 axis= Vector3.Normalize(direction);

            float topLength = 5000;
            float bottomLength = 5000;
           
            float distance = Vector3.Distance(topNode, bottomNode);


            float cos_bottom = (bottomLength * bottomLength + distance * distance - topLength * topLength) / (2 * bottomLength * distance);
            float angle_bottom = MathF.Acos(cos_bottom);

            float transwidht = bottomLength * MathF.Sin(angle_bottom);
            float transheigth = bottomLength * cos_bottom;
            Vector3 endNode = bottomNode + new Vector3(0, transwidht, transheigth);



            Plane p1 = new Plane(center, middle, middle+new Vector3(0,0,1000));
            Plane p2 = new Plane(topNode, bottomNode, endNode);

            float angle = AngleBetween(p1, p2);
            float angledeg = angle * 180 / MathF.PI;

            if (defineSide == Side.StarBoard)
            {
                 newnode = rotateVectorAboutAxis(bottomNode, topNode, endNode, angle);

            }
            else
            {
                newnode = rotateVectorAboutAxis(bottomNode, topNode, endNode, -angle);

            }

            newnode = CalculateThirdPointOnPlane(bottomNode, topNode, bottomLength, topLength, p1, center);

            float olddistbot = Vector3.Distance(bottomNode, endNode);
            float newdistbot = Vector3.Distance(bottomNode, newnode);
            float olddisttop = Vector3.Distance(topNode, endNode);
            float newdisttop = Vector3.Distance(topNode, newnode);

            
            List<ChainLink3D> NetPatent = new();
            NetPatent.Add(new ChainLink3D(60, 180, 220, topNode, newnode));
            NetPatent.Add(new ChainLink3D(60, 180, 220, bottomNode, newnode));


            return NetPatent;

        }

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



        public static Matrix rotationMatrixPlaneVector2(Vector3 lineStart, Vector3 lineEnd, Plane plane)
        {
            Vector3 startPoint = lineStart;
            Vector3 endPoint = lineEnd;
            Vector3 planeNormal = plane.Normal;

            // Calculate the vector that defines the line
            Vector3 lineVector = endPoint - startPoint;

            // Calculate the unit vectors for the line and plane
            Vector3 lineUnit = Vector3.Normalize(lineVector);
            Vector3 planeUnit =Vector3.Normalize(Vector3.Cross(lineUnit, planeNormal));

            // Calculate the angle of rotation
            float angle = (float)Math.Acos(Vector3.Dot(lineUnit, planeUnit));
            

            // Calculate the axis of rotation
            Vector3 axis = Vector3.Cross(lineUnit, planeUnit);

            // Create the rotation matrix using Matrix.RotationAxis
            Matrix rotationMatrix = Matrix.RotationAxis(axis, angle);

            return rotationMatrix;

        

        }

        public static Vector3 rotateVectorAboutAxis(Vector3 axis1, Vector3 axis2, Vector3 vectorToRotate, float angle)
        {
            // Define the point to be rotated
            Vector3 point = vectorToRotate;

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






        public static Vector3 CalculateThirdPointOnPlane(Vector3 A, Vector3 B, float d1, float d2, Plane p, Vector3 point)
        {
            // Calculate vector AB from A to B
            Vector3 AB = B - A;
            var pointonplane =point;


            // Calculate scalar parameter t for the line passing through A and B
            float t = Vector3.Dot(p.Normal, pointonplane - A) / Vector3.Dot(p.Normal, AB);

            // Calculate the point P on the line AB that lies on the plane P
            Vector3 P = A + t * AB;

            // Calculate the distance from P to A and B
            float distAP = Vector3.Distance(P, A);
            float distBP = Vector3.Distance(P, B);

            // Check if the distances from P to A and B are equal to d1 and d2
            if (Math.Abs(distAP - d1) > 1e-6 || Math.Abs(distBP - d2) > 1e-6)
            {
                // Calculate the projection of the point onto the plane
                float d = Vector3.Dot(p.Normal, pointonplane - P) / Vector3.Dot(p.Normal, p.Normal);
                Vector3 projection = P + d * p.Normal;
                P = projection;
            }

            // Return the exact location of the third point on the plane P
            return P;
        }










        public override void Draw()
        {
            MeshBuilder meshBuilder = new MeshBuilder();

            meshBuilder.AddBox(Position,Size.X,Size.Y,Size.Z);



            MeshGeometry = meshBuilder.ToMeshGeometry3D();
        }
    }
}
