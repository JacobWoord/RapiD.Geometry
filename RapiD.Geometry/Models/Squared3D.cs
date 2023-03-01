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
            NetSquared.Add(new Sphere3D(bbUpperPoint + Position) { NodeNumber = 1 });
            NetSquared.Add(new Sphere3D(new Vector3(Position.X - Size.X / 2, Position.Y + Size.Y / 2, Position.Z)) { NodeNumber = 3 });
            NetSquared.Add(new Sphere3D(bbBottomPoint + Position) { NodeNumber = 3 });
            NetSquared.Add(new Sphere3D(SbUpperPoint + Position) { NodeNumber = 4 });
            NetSquared.Add(new Sphere3D(new Vector3(Position.X + Size.X / 2, Position.Y + Size.Y / 2, Position.Z)) { NodeNumber = 5 });
            NetSquared.Add(new Sphere3D(SbBottomPoint + Position) { NodeNumber = 7 });
            NetSquared.Add(new Sphere3D(SbBottomPoint + Position) { NodeNumber = 7 });

            var newcenter = centerPoint - new Vector3(0, 0, 1000);
            NetSquared.Add(new Sphere3D(newcenter) { NodeNumber = 8 });

            foreach (var item in NetSquared)
            {
                ListPoints.Add(item);
            }
            return NetSquared;

        }


        public List<ChainLink3D> CreateNetPatent(Vector3 topVector, Vector3 bottomVector)
        {
            //OutLine Direction ( Direction to point the roration to)
            float lineLength = 100000;
            Vector3 center = listPoints[7].Position;
            Vector3 middle = listPoints[1].Position;
            Vector3 direction = middle - center;
            Vector3 axis= Vector3.Normalize(direction);

            float topLength = 5000;
            float bottomLength = 5000;
            Vector3 bottomNode = bottomVector;
            Vector3 topNode = topVector;
            float distance = Vector3.Distance(topNode, bottomNode);


            float cos_bottom = (bottomLength * bottomLength + distance * distance - topLength * topLength) / (2 * bottomLength * distance);
            float angle_bottom = MathF.Acos(cos_bottom);

            float transwidht = bottomLength * MathF.Sin(angle_bottom);
            float transheigth = bottomLength * cos_bottom;



            Vector3 endNode = bottomNode + new Vector3(0, transwidht, transheigth);
           
      
            List<ChainLink3D> NetPatent = new();
            NetPatent.Add(new ChainLink3D(60, 180, 220, topNode, endNode));
            NetPatent.Add(new ChainLink3D(60, 180, 220, bottomNode, endNode));


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
            float angledeg = angle * 180 / MathF.PI;

            // Calculate the axis of rotation
            Vector3 axis = Vector3.Cross(lineUnit, planeUnit);

            // Create the rotation matrix using Matrix.RotationAxis
            Matrix rotationMatrix = Matrix.RotationAxis(axis, angle);

            return rotationMatrix;

        

        }

        public static Vector3 rotateVectorAboutAxis(Vector3 axis1, Vector3 axis2, Vector3 vectorToRotate, float angle)
        {
            Vector3 lineDirection = Vector3.Cross(axis1, axis2);
            lineDirection.Normalize();

            Vector3 vectorPerpendicularToLine = vectorToRotate - (Vector3.Dot(vectorToRotate, lineDirection) * lineDirection);
            vectorPerpendicularToLine.Normalize();

            Vector3 axisOfRotation = Vector3.Cross(lineDirection, vectorPerpendicularToLine);

            Matrix rotationOntoPlaneMatrix = Matrix.RotationAxis(lineDirection, (float)Math.PI / 2);
            Matrix rotationInPlaneMatrix = Matrix.RotationAxis(axisOfRotation, angle);
            Matrix finalRotationMatrix = rotationInPlaneMatrix * rotationOntoPlaneMatrix;

            Vector3 rotatedVector = Vector3.Transform(vectorToRotate, finalRotationMatrix).ToVector3();


            return rotatedVector;
        }














        public override void Draw()
        {
            MeshBuilder meshBuilder = new MeshBuilder();

            meshBuilder.AddBox(Position,Size.X,Size.Y,Size.Z);



            MeshGeometry = meshBuilder.ToMeshGeometry3D();
        }
    }
}
