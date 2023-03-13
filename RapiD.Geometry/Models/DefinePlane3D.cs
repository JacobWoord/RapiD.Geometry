using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Models
{
    public class DefinePlane3D : GeometryBase3D
    {

        public float planeDistance;
        public Vector3 Normal;

        public DefinePlane3D (Plane plane, float width, float height)
        {
            planeDistance = plane.D;
            Normal = plane.Normal;
            OriginalMaterial = PhongMaterials.Yellow;
            // Define the mesh for the plane using a rectangular quad
            MeshBuilder meshBuilder = new MeshBuilder();
            Vector3 planeRight = Vector3.Cross(Normal, new Vector3(0, 0, 1));
            planeRight.Normalize();
            Vector3 planeUp = Vector3.Cross(Normal, planeRight);
            planeUp.Normalize();
            Vector3 planeCenter = Normal * planeDistance;
            Vector3 planeTopLeft = planeCenter - (planeRight * width / 2) + (planeUp * height / 2);
            Vector3 planeTopRight = planeCenter + (planeRight * width / 2) + (planeUp * height / 2);
            Vector3 planeBottomLeft = planeCenter - (planeRight * width / 2) - (planeUp * height / 2);
            Vector3 planeBottomRight = planeCenter + (planeRight * width / 2) - (planeUp * height / 2);
            meshBuilder.AddQuad(planeTopLeft, planeTopRight, planeBottomRight, planeBottomLeft);


            MeshGeometry = meshBuilder.ToMeshGeometry3D();

            // Create and return the MeshGeometry3D object




        }

    }
}
