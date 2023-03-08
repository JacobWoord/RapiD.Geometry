using HelixToolkit.SharpDX.Core;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RapiD.Geometry.Models
{
    public partial class Element3D : GeometryBase3D
    {
        [ObservableProperty]
        Vector3 startPoint;

        [ObservableProperty]
        Vector3 endPoint;

        [ObservableProperty]
        Color4 color;

        public List<Vector3> vectors = new();
        public ConnectionType ConnectionType { get; set; }

        public float diameter;

        public float width;
        public float elementLength;
        public bool rotateElement;


        public Element3D(Vector3 startPoint, Vector3 endPoint, ConnectionType type, float dia, float width, bool rotateElement)
        {
            ID = Guid.NewGuid().ToString();
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.diameter = dia;
            this.width = width;
            this.elementLength = Vector3.Distance(startPoint, endPoint);
            this.rotateElement = rotateElement;
            this.ConnectionType = type;

            if (ConnectionType == ConnectionType.Chain)
                calcChainelement();

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

        public void calcChainelement()
        {
            if (startPoint == endPoint)
                return;

            float radius = (width - diameter) / 2;

            float length = elementLength + diameter - 2 * radius;
            float trans = 0f;
            float translate = length + (radius * 2) - diameter;
            float yoffset = 0;
            int segments = 10;
            float interval = 180 / segments;

            Vector3 yVector = Vector3.UnitY;

            //Matrix rotationMatrix = RotationMatrix2(yVector, relVector);
            Matrix rotationMatrix = RotationMatrix(Vector3.Zero, yVector, startPoint, endPoint);

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

                float y = radius * MathF.Sin(a) + yoffset + trans + (radius - (diameter / 2));
                //  Debug.WriteLine($"a= {y}");


                Vector3 vec = new Vector3(x, y, 0);
                // Debug.WriteLine($"vec= {vec}");


                //Rotates every second chainlink
                if (rotateElement)
                    vec = new Vector3(0, y, x);


                var newVec = Vector3.TransformCoordinate(vec, rotationMatrix);
                newVec += startPoint;

                vectors.Add(newVec);
            }
        }

    }
}
