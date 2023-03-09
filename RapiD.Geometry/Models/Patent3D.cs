using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace RapiD.Geometry.Models
{
    public partial class Patent3D : ObservableObject
    {

        public List<ConnectionClass> Connections = new();

        public Vector3 topPoint;
        public Vector3 middlePoint;
        public Vector3 bottomPoint;
        public Vector3 targetPoint;
        public float lengthbottom;
        public float lengthtop;
        public Plane plane;

        ConnectionType Type = ConnectionType.Chain;
        
        public string Id { get; set; }

       

      

        public Patent3D(Vector3 bot, Vector3 top, Vector3 mid, float lengthbot, float lengthtop, Plane plane)
        {

            Id = Guid.NewGuid().ToString();


            this.topPoint = top;
            this.middlePoint = mid;
            this.bottomPoint = bot;
            this.lengthbottom = lengthbot;
            this.lengthtop = lengthtop;
            this.plane = plane;

            targetPoint = findThirdPoint(bottomPoint, topPoint, lengthbottom, lengthtop, plane);
            CreateConnections();
        }



     

    



        public Patent3D(List<ConnectionClass> connections,  Plane plane)
        {

           

            Id = Guid.NewGuid().ToString();

            ConnectionClass topconnection = connections.OrderBy(x => x.startVector.Z).ToList().Last();
            ConnectionClass midconnection = connections.OrderBy(x => x.startVector.Z).ToList()[1];
            ConnectionClass botconnection = connections.OrderBy(x => x.startVector.Z).ToList().First();


            this.topPoint = topconnection.startVector;
            this.middlePoint = midconnection.startVector;
            this.bottomPoint = botconnection.startVector;
            this.lengthbottom = Vector3.Distance(botconnection.startVector, botconnection.endVector);
            this.lengthtop = Vector3.Distance(topconnection.startVector, topconnection.endVector);
            this.plane = plane;

            targetPoint = findThirdPoint(bottomPoint, topPoint, lengthbottom, lengthtop, plane);
            UpdateConnections(connections);


        }




        public void CreateConnections()
        {

            targetPoint = findThirdPoint(bottomPoint, topPoint, lengthbottom, lengthtop, plane);


            if (topPoint != Vector3.Zero)
            {
                Connections.Add(new ConnectionClass(topPoint, targetPoint,Type, Id) );
            }

            if (middlePoint != Vector3.Zero)
            {
                Connections.Add(new ConnectionClass(middlePoint, targetPoint, Type, Id));
            }

            if (bottomPoint != Vector3.Zero)
            {
                Connections.Add(new ConnectionClass(bottomPoint, targetPoint, Type, Id) );
            }
        }


        public void UpdateConnections(List<ConnectionClass> connections)
        {
            Connections.Clear();
            foreach (var connection in connections)
            {
                Connections.Add(new ConnectionClass(topPoint, targetPoint,Type, Id));
            }
        }


        public bool Check()
        {

            float distanceBetweenNodes = Vector3.Distance(topPoint, bottomPoint);

            if (lengthtop + lengthbottom < distanceBetweenNodes)
            {
                return false;
            }
            else if (lengthtop > lengthtop + lengthbottom)
            {
                return false;
            }
            else if (lengthbottom > lengthtop + lengthbottom)
            {

                return false;
            }
            else if (lengthtop + lengthbottom < distanceBetweenNodes)
            {

                return false;
            }
            return true;
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
            float l3 = Vector3.Distance(p1, p2);

            //calculate angel between first rib and third rib
            float angle = MathF.Acos((l1 * l1 + l3 * l3 - l2 * l2) / (2 * l1 * l3));

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

            for (float i = 0; i <= 2 * MathF.PI; i += 0.01f)
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

        public void Deselect()
        {
            
        }

        public void Select()
        {
        }
    }
}
