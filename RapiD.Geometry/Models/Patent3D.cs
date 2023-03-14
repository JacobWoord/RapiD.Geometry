using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RapiD.Geometry.Models
{
    public partial class Patent3D : ObservableObject
    {

        public List<ConnectionClass> Connections = new();
        public List<Vector3> nodelist = new();

        public Vector3 topPoint { get; set; }
        public Vector3 middlePoint { get; set; }
        public Vector3 bottomPoint { get; set; }
        public Vector3 targetPoint { get; set; }
        public Vector3 centerPoint { get; set; }
        public float lengthbottom { get; set; }
        public float lengthtop { get; set; }
        public Plane plane { get; set; }

        ConnectionType Type = ConnectionType.Chain;
        
        public string Id { get; set; }

       

      

        public Patent3D(Vector3 bot, Vector3 top, Vector3 mid, float lengthbot, float lengthtop, Plane plane, List<Vector3> nodeList,Vector3 centerPoint)
        {

            Id = Guid.NewGuid().ToString();
            this.centerPoint = centerPoint;
            this.nodelist = nodeList;
            this.topPoint = top;
            this.middlePoint = mid;
            this.bottomPoint = bot;
            this.lengthbottom = lengthbot;
            this.lengthtop = lengthtop;
            this.plane = plane;
            
            CreateConnections();
            //SetstartPoints();
        }

        
        
        
        //public void SetstartPoints()
        //{

        //    List<Vector3> sortedList = nodelist.OrderByDescending(v => v.Z).ToList();

        //    topPoint= sortedList[0];    
        //    middlePoint= sortedList[5];    
        //    middlePoint= sortedList[7];    


        //}



     

    



        //public Patent3D(List<ConnectionClass> connections,  Plane plane)
        //{

           

        //    Id = Guid.NewGuid().ToString();

        //    ConnectionClass topconnection = connections.OrderBy(x => x.startVector.Z).ToList().Last();
        //    ConnectionClass midconnection = connections.OrderBy(x => x.startVector.Z).ToList()[1];
        //    ConnectionClass botconnection = connections.OrderBy(x => x.startVector.Z).ToList().First();


        //    this.topPoint = topconnection.startVector;
        //    this.middlePoint = midconnection.startVector;
        //    this.bottomPoint = botconnection.startVector;
        //    this.lengthbottom = Vector3.Distance(botconnection.startVector, botconnection.endVector);
        //    this.lengthtop = Vector3.Distance(topconnection.startVector, topconnection.endVector);
        //    this.plane = plane;

        //    UpdateConnections(connections);
        //}




        public bool UpdateTargetCalculation(string connectionId,float connectionLength = 0,Vector3 newstartposition = new Vector3())
        {

            float newLength = connectionLength;

         
            
            Vector3 newStartPosition = newstartposition;

            if (newStartPosition != Vector3.Zero)
            {
                var connection = Connections.FirstOrDefault(x => x.Id == connectionId);
                switch (connection.patentSide)
                {
                    case PatentSide.Up:
                        topPoint = newStartPosition; 
                        break;
                    case PatentSide.Middle:
                        middlePoint = newStartPosition;
                        break;
                    case PatentSide.Down:
                        bottomPoint = newStartPosition;
                        break;
                    default:
                        break;
                }
            }
           //This newTarget Below is just for checking the validation of the length
            SharpDX.Vector3 newTarget = findThirdPoint(bottomPoint, topPoint, lengthbottom, connectionLength, plane);

            if (Check(newTarget, connectionLength))
            {
                string id = connectionId;
                ConnectionClass connection = Connections.FirstOrDefault(c => c.Id == id);
                switch (connection.patentSide)
                {
                    case PatentSide.Up:
                        Connections.Clear();
                        lengthtop = newLength;
                        targetPoint = findThirdPoint(bottomPoint, topPoint, lengthbottom, lengthtop, plane);
                        CreateConnections();
                        return true;


                        break;
                    case PatentSide.Down:
                        Connections.Clear();
                        lengthbottom = newLength;
                        targetPoint = findThirdPoint(bottomPoint, topPoint, lengthbottom, lengthtop, plane);
                        CreateConnections();
                        return true;
                        break;
                    default:
                        return false;
                }

                

            }

            return false;
             

        }




        public void CreateConnections()
        {

            targetPoint = findThirdPoint(bottomPoint, topPoint, lengthbottom, lengthtop, plane);


            if (topPoint != Vector3.Zero)
            {
                Connections.Add(new ConnectionClass(topPoint, targetPoint,Type, Id) { patentSide = PatentSide.Up} );
            }

            if (middlePoint != Vector3.Zero)
            {
                Connections.Add(new ConnectionClass(middlePoint, targetPoint, Type, Id) { patentSide= PatentSide.Middle});
            }

            if (bottomPoint != Vector3.Zero)
            {
                Connections.Add(new ConnectionClass(bottomPoint, targetPoint, Type, Id) { patentSide = PatentSide.Down} );
            }
        }


       


        public bool Check(Vector3 newTarget, float newlength)
        {
            if (float.IsNaN(newTarget.X))
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




        public Vector3 findThirdPoint(Vector3 p1, Vector3 p2, float l1, float l2, Plane plane)
        {
            //get length of third rib
            float l3 = Vector3.Distance(p1, p2);

            //calculate angel between first rib and third rib
            //float angle = MathF.Acos((l1 * l1 + l3 * l3 - l2 * l2) / (2 * l1 * l3));
            float angle = MathF.Acos((l1 * l1 + l3 * l3 - l2 * l2) / (2 * l1 * l3));


            //define direction between two known points
            Vector3 direction = Vector3.Normalize(p2 - p1);
            //set rotationaxis to the crossproduct of these two point
            Vector3 rotationaxis = Vector3.Normalize(Vector3.Cross(p1, p2));

            Matrix rotation = Matrix.RotationAxis(rotationaxis, angle);

            //define the direction of the first rib and calculate the location of the third point 
            Vector3 rib1Direction = Vector3.TransformNormal(direction, rotation);

            Vector3 p3 = p1 + rib1Direction * l1;

            //float anglewithplane = AngleBetween(plane, new Plane(p2, p1, p3));

            //loop from 0 to 360 degree to find the value that is the most close to the known plane
            float maxdif = float.MaxValue;
            float disttozero = float.MaxValue;

            float angletorotate = 0f;

            for (float i =  0; i <= 2*MathF.PI; i += 0.1f)
            {
                Vector3 newvec = rotateVectorAboutAxis(p1, p2, new Vector3(p3.X, p3.Y, p3.Z), i);
                float dist = DistancePointToPlane(newvec, plane);
                float distorigin = Vector3.Distance(newvec, Vector3.Zero);


                if (dist < maxdif &&distorigin<disttozero)
                {
                    maxdif = dist;
                    angletorotate = i;
                    disttozero = distorigin;
                }

            }
            angletorotate = 0f;

            var dist2 = Vector3.Distance(p3, Vector3.Zero);


            return rotateVectorAboutAxis(p1, p2, p3, angletorotate);
        }


        //public Vector3 findThirdPoint(Vector3 p1, Vector3 p2, float l1, float l2, Plane plane)
        //{
        //    float l3 = Vector3.Distance(p1, p2);
        //    float angle = MathF.Acos((l1 * l1 + l3 * l3 - l2 * l2) / (2 * l1 * l3));

        //    Vector3 direction = Vector3.Normalize(p2 - p1);
        //    Vector3 rotationaxis = Vector3.Normalize(Vector3.Cross(p1, p2));

        //    Matrix rotation = Matrix.RotationAxis(rotationaxis, angle);
        //    Vector3 rib1Direction = Vector3.TransformNormal(direction, rotation);

        //    Vector3 p3 = p1 + rib1Direction * l1;

        //    Vector3 planeNormal = Vector3.Normalize(Vector3.Cross(p2 - p1, p3 - p1));

        //    float maxdif = float.MaxValue;
        //    float angletorotate = 0f;

        //    for (float i = MathF.PI / 2; i <= 2 * MathF.PI; i += 0.01f)
        //    {
        //        Vector3 rotatedP3 = rotateVectorAboutAxis(p1, p2, p3, i);
                
        //        Vector3 newPlaneNormal = Vector3.Normalize(Vector3.Cross(p2 - p1, rotatedP3 - p1));
        //        float dist = Vector3.Dot(planeNormal, rotatedP3 - p1);

        //        if (dist < maxdif)
        //        {
        //            maxdif = dist;
        //            angletorotate = i;
        //        }
        //    }

        //    return rotateVectorAboutAxis(p1, p2, p3, angletorotate);
        //}




        //public static Vector3 findThirdPoint(Vector3 p1, Vector3 p2, float l1, float l2, Plane plane)
        //{
        //    // get length of third rib
        //    float l3 = Vector3.Distance(p1, p2);

        //    // calculate angle between first rib and third rib
        //    float angle = MathF.Acos((l1 * l1 + l3 * l3 - l2 * l2) / (2 * l1 * l3));

        //    // define direction between two known points
        //    Vector3 direction = Vector3.Normalize(p2 - p1);

        //    // set rotationaxis to the cross product of these two point
        //    Vector3 rotationaxis = Vector3.Normalize(Vector3.Cross(p1, p2));

        //    // define the direction of the first rib and calculate the location of the third point 
        //    Matrix rotation = Matrix.RotationAxis(rotationaxis, angle);
        //    Vector3 rib1Direction = Vector3.TransformNormal(direction, rotation);
        //    Vector3 p3 = p1 + rib1Direction * l1;

        //    // project the third point onto the plane
        //    float distToPlane = Vector3.Dot(p3 - plane.Normal * plane.D, plane.Normal) / plane.Normal.Length();
        //    Vector3 projectedPoint = p3 - distToPlane * plane.Normal;

        //    // adjust projected point to lie on the plane
        //    float distanceFromPlane = Vector3.Dot(projectedPoint, plane.Normal) - plane.D;
        //    Vector3 adjustedPoint = projectedPoint - distanceFromPlane * plane.Normal;

        //    return adjustedPoint;
        //}


        public static float DistancePointToPlane(Vector3 point, Plane plane)
        {
            // Calculate the distance from the point to the plane using the plane equation:
            // ax + by + cz + d = 0
            // Where a, b, and c are the components of the plane's normal vector, and d is the distance from the origin to the plane.


            //Vector3 pointOnPlane = -plane.Normal * plane.D;
            //Vector3 difference = point - pointOnPlane;

            //float distance = Vector3.Dot(difference, plane.Normal) / plane.Normal.Length();
            //return Math.Abs(distance);



            /*ORGINEEL */
            float distance = Vector3.Dot(plane.Normal, point);
            distance -= plane.D;
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
