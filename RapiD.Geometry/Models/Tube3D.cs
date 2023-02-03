using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RapiD.Geometry.Models
{
    public partial class Tube3D : GeometryBase3D
    {

        [ObservableProperty]
        List<Vector3> centerPoints = new List<Vector3>()
        {

             new Vector3(-30, 60, 0),
             new Vector3(30, 60, 0),
             new Vector3(30, -60, 0),
             new Vector3(-30, -60, 0),
             new Vector3(-30, 60, 0),


        };



        [ObservableProperty]
        double diameter;

        [ObservableProperty]
        int thetaDiv = 30;

        public Tube3D()
        {
            OriginalMaterial = PhongMaterials.Yellow;


            DrawTube();


        }



        // Transform3DGroup transformGroup = new Transform3DGroup();

        // Matrix3D matrix = new Matrix3D();
        // matrix.Translate(new Vector3D(0d, 0d, 0d));
        // MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);


        //transformGroup.Children.Add(matrixTransform);

        private void rotate(Vector3[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                vectors[i] = new Vector3(vectors[i].Z, vectors[i].Y, vectors[i].X);

            }
        }


        public void DrawTube()
        {



            MeshBuilder meshBuilder = new MeshBuilder();
            //MeshGeometry = meshBuilder.ToMeshGeometry3D();
            //meshBuilder.AddTube(centerPoints, 10, 10, false);




            int copies = 10;
            float length = 60f;
            float width = 50f;
            float trans = 0f;
            float angle = 30f;
            float positiveRadius = width / 2f;
            float radial = MathF.PI / 180f * angle;



            float radius = 25f;

            trans += length - 10;


            float point1X = 0 - width / 2;
            float point1Y = length / 2;

            float point2X = MathF.Sin(radial) * radius;
            float point2Y = length / 2;
           // Debug.Show($"radial: {radial} point:{point2X}");

            // Create a single link
            List<Vector3> single_chain_link = new List<Vector3> ();
            /* single_chain_link[0] = new Vector3(point1X, point1Y, 0);

             single_chain_link[1] = new Vector3(point2X, 42.5f, 0);


             single_chain_link[2] = new Vector3(-12.5f, 51.65f, 0);
             single_chain_link[3] = new Vector3(0, 55f, 0);
             single_chain_link[4] = new Vector3(12.5f, 51.65f, 0);
             single_chain_link[5] = new Vector3(21.65f, 42f, 0);
             single_chain_link[6] = new Vector3(25f, 30, 0);

             single_chain_link[7] = new Vector3(25f, -30, 0);
             single_chain_link[8] = new Vector3(21.65f, -42.5f, 0);
             single_chain_link[9] = new Vector3(12.5f, -51.65f, 0);
             single_chain_link[10] = new Vector3(0, -55f, 0);
             single_chain_link[11] = new Vector3(-12.5f, -51.65f, 0);
             single_chain_link[12] = new Vector3(-21.65f, -42.5f, 0);
             single_chain_link[13] = new Vector3(-25f, -30f, 0);
             single_chain_link[14] = new Vector3(-25f, 32f, 0);

             */
            float yoffset = 0;
            int segments = 12;
            float interval = 180 / segments;
            for (float i = 0; i <= 360; i+=interval)
            {
                if (i > 180)
                    yoffset = -length;

                float a = i * MathF.PI / 180;
                float x = radius*MathF.Cos(a);
                float y = radius *MathF.Sin(a);

                single_chain_link.Add(new Vector3(x, y+yoffset, 0));

          
            
            }
            single_chain_link.OrderByDescending(x => x.X);










            //single_chain_link[1] = new Vector3(width, trans, 0);
            //single_chain_link[2] = new Vector3(width, length+trans, 0);
            //single_chain_link[3] = new Vector3(0, length+trans, 0);
            //single_chain_link[4] = single_chain_link[0];
            

            // Rotate every uneven link
            //if (k % 2 == 1)
            //{
            //    for (int i = 0; i < single_chain_link.Length; i++)
            //    {
            //        single_chain_link[i] = new Vector3(width, length+trans , width);

            //    }
            //}

            meshBuilder.AddTube(single_chain_link, 10, 16, true);
            //for (int i = 0; i < single_chain_link.Length; i++)
            //{
            //    meshBuilder.AddSphere(single_chain_link[i], 5);
            //}



            MeshGeometry = meshBuilder.ToMeshGeometry3D();

            //for (int i = 0; i < 1; i++)

            //{
            //    for (int j = 0; j < centerPoints.Count; j++)
            //    {
            //        Vector3 updatedVector = centerPoints[j];
            //        updatedVector.Z += 0;

            //        centerPoints[j] = updatedVector;

            //        meshBuilder.AddTube(centerPoints, 10, 10, false);
            //    }
            //}

        }
    }
}














