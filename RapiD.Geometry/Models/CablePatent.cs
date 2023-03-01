using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace RapiD.Geometry.Models
{
    public partial class CablePatent : GeometryBase3D

    {
        private List<Vector3> Centerpoints= new();
        private  float radius;
       
        
        public CablePatent(List<Vector3> centerpoints, float radius = 150)
        {
            this.radius = radius;
            this.Centerpoints = centerpoints;
            this.ID = Guid.NewGuid().ToString();
            Draw();
            OriginalMaterial = PhongMaterials.BlackRubber;

        }
        public override void Draw()
        {
            MeshBuilder meshBuilder = new MeshBuilder();
            meshBuilder.AddTube(Centerpoints, radius, 50, true);

            MeshGeometry = meshBuilder.ToMeshGeometry3D();
        }
    }

    public partial class Warp3D : GeometryBase3D
    {


        private List<Vector3> Centerpoints = new();
        private float radius;
        public Warp3D(List<Vector3> centerpoints, float radius = 150)
        {
            this.radius = radius;
            this.Centerpoints = centerpoints;
            this.ID = Guid.NewGuid().ToString();
            Draw();
            OriginalMaterial = PhongMaterials.Chrome;

        }
        public override void Draw()
        {
            MeshBuilder meshBuilder = new MeshBuilder();
            meshBuilder.AddTube(Centerpoints, radius, 20, true);
            MeshGeometry = meshBuilder.ToMeshGeometry3D();
        }

    }
    public partial class Pees3D: GeometryBase3D
    {



        private List<Vector3> Centerpoints = new();
        private float radius;
        GeometryBase3D model;


        public Pees3D(GeometryBase3D model,  List<Vector3> centerpoints, float radius = 50)
        {

            this.model = model; 
            this.radius = radius;
            this.Centerpoints = centerpoints;
            this.ID = Guid.NewGuid().ToString();
            Draw();
            OriginalMaterial = PhongMaterials.BlackRubber;

        }
        public override void Draw()
        {
            var netExample = model as Squared3D;
            var Position =  new Vector3(-netExample.Size.X / 2, netExample.Size.Y / 2,  netExample.Size.Z / 2) + netExample.Position;
            MeshBuilder meshBuilder = new MeshBuilder();
            meshBuilder.AddTube(Centerpoints, radius, 50, true);
            MeshGeometry = meshBuilder.ToMeshGeometry3D();
        }


    }


    public partial class NetConnections3D : GeometryBase3D
    {


        private List<Vector3> Centerpoints = new();
        private float radius;
        GeometryBase3D model;


        public NetConnections3D(GeometryBase3D model, List<Vector3> centerpoints, float radius = 50)
        {

            this.model = model;
            this.radius = radius;
            this.Centerpoints = centerpoints;
            this.ID = Guid.NewGuid().ToString();
            Draw();
            OriginalMaterial = PhongMaterials.BlackRubber;

        }
        public override void Draw()
        {
            var netExample = model as Squared3D;
            var Position = new Vector3(-netExample.Size.X / 2, netExample.Size.Y / 2, netExample.Size.Z / 2) + netExample.Position;
            MeshBuilder meshBuilder = new MeshBuilder();
            meshBuilder.AddTube(Centerpoints, radius, 50, true);
            MeshGeometry = meshBuilder.ToMeshGeometry3D();
        }

    }
    }



































