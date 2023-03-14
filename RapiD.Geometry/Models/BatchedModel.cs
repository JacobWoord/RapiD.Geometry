using HelixToolkit.SharpDX.Core;

using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System.Windows.Media.Media3D;
using SharpDX.Toolkit.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Material = HelixToolkit.Wpf.SharpDX.Material;
using SharpDX.Direct3D11;
using System.Collections;
using SharpDX.Direct2D1;

namespace RapiD.Geometry.Models
{


    public abstract partial class BatchedModel : ObservableObject, IModel
    {

        [ObservableProperty]
        CullMode cullMode=CullMode.Back;

        [ObservableProperty]
        IList<BatchedMeshGeometryConfig> batchedMeshes;

        [ObservableProperty]
        IList<Material> modelMaterials;

        [ObservableProperty]
        Material baseMaterial = PhongMaterials.Red;

        [ObservableProperty]
        List<Vector3> nodeList = new();



        [ObservableProperty]
        List<Vector3> allNodes = new();

        [ObservableProperty]
        bool isSelected = false;

        [ObservableProperty]
        bool isOpenMenu = true;

        [ObservableProperty]
        System.Windows.Media.Media3D.Transform3DGroup transform = new();




        public List<Vector3> GetNodeList()
        {

            return NodeList;
        }




        public string Name { get; set; }
        public string FileName { get; set; }
        public string Id { get; set; }
        public string ConnectionId { get ; set; }
        public string PatentId { get; set; }
        public float ConnectionLength { get; set; }


        //  public bool IsSelected { get; set; }

        [ObservableProperty]
        SharpDX.Vector3 position;

        public BatchedModel()
        {
            batchedMeshes = new List<BatchedMeshGeometryConfig>();
            modelMaterials = new List<Material>();
        }

        public void RotateTransform(double xaxis = 0, double yaxis = 0, double zaxis = 0, double degrees = 90)
        {
            var axis = new System.Windows.Media.Media3D.Vector3D(xaxis, yaxis, zaxis);
            var rotation = new System.Windows.Media.Media3D.AxisAngleRotation3D(axis, degrees);
            var transform = new System.Windows.Media.Media3D.RotateTransform3D(rotation);

            //NOTE: every transform we add as a child to the Transformgroup removes the previous tranformation! important to update the node list after all transformations ar done!
            Transform.Children.Add(transform);
        }


        public void UpdateNodeList()
        {

            for (int i = 0; i < NodeList.Count; i++)
            {
                NodeList[i] = (Vector3)Vector3.Transform(NodeList[i], Transform.Value.ToMatrix());
                
            }
        }

        //public static List<Vector3> ApplyToVector3List(List<Vector3> vectorList, Matrix3D transformMatrix)
        //{
        //    List<Vector3> transformedVectorList = new List<Vector3>();
        //    foreach (Vector3 vector in vectorList)
        //    {
        //        Vector3D vector3D = new Vector3D(vector.X, vector.Y, vector.Z);
        //        vector3D = Vector3D.Multiply(vector3D, transformMatrix);
        //        transformedVectorList.Add(new Vector3((float)vector3D.X, (float)vector3D.Y, (float)vector3D.Z));
        //    }
        //    return transformedVectorList;
        //}

        public async Task UpdatePositionDoor(Vector3 replacement)
        {

            Vector3 replace = replacement;
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new System.Windows.Media.Media3D.Vector3D(replace.X, replace.Y, replace.Z));
            if (Transform.Children.Count() > 0)
            {
                Transform3DGroup currentTransform = Transform.Clone();
                matrix.Append(currentTransform.Value);
            }
            MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);
           // NodeList = ApplyToVector3List(nodeList, matrixTransform.Value);

            Transform.Children.Clear();
            Transform.Children.Add(matrixTransform);
            UpdateNodeList();

        }


        public async Task UpdatePositionDoor(Plane plane, Vector3 start, Vector3 replacement)
        {


            float dist = Patent3D.DistancePointToPlane(start, plane);
            Vector3 extraLength = Vector3.Zero;

            Vector3 translate = plane.Normal * dist;

            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new System.Windows.Media.Media3D.Vector3D(translate.X, translate.Y, translate.Z));

            if (Transform.Children.Count() > 0)
            {
                Transform3DGroup currentTransform = Transform.Clone();
                matrix.Append(currentTransform.Value);
            }

            MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);
           // NodeList = ApplyToVector3List(nodeList, matrixTransform.Value);

            Transform.Children.Clear();
            Transform.Children.Add(matrixTransform);
            UpdateNodeList();

        }


        public Vector3 DirectionToPlane(Plane plane, Vector3 start)
        {
            Vector3 planeNormal = plane.Normal;
            float distanceToPlane = Vector3.Dot(start, planeNormal) - plane.D;
            Vector3 projectionOnPlane = start - planeNormal * distanceToPlane;

            Vector3 directionToPlane = Vector3.Normalize(projectionOnPlane - start);

            return directionToPlane;
        }



       



























        public void Mirror(MirrorAxis mirrorAxis)
        {
            var scale = new System.Windows.Media.Media3D.ScaleTransform3D();
            if (mirrorAxis == MirrorAxis.X)
                scale.ScaleX = -1;
            else if (mirrorAxis == MirrorAxis.Y)
                scale.ScaleY = -1;
            else if (mirrorAxis == MirrorAxis.Z)
                scale.ScaleZ = -1;

            Transform.Children.Add(scale);
            CullMode = CullMode.Front;
            //AllNodes.Where(x => x.CanTranslate).ToList().ForEach(x => x.Transform3DGroup.Children.Add(scale));

        }


        public async Task OpenFile()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                Debug.WriteLine("No file specified");
                return;
            }

            var configs = await Task.Run(() => Importer3D.OpenFile(FileName));
            var models = configs.Where(x => x.Name.Contains("anchor") == false);

            foreach (var m in models)
            {
                BatchedMeshes.Add(m.BatchedMeshGeometryConfig);
                ModelMaterials.Add(m.MaterialCore.ConvertToMaterial());
            }



            var anchors = configs.Where(x => x.Name.Contains("anchor") == true);

            //positions of nodes in door added to vector3 List
            foreach (var item in anchors)
            {
                nodeList.Add(item.Location);
            }

            //var groups = nodeList.GroupBy(x => x.Z).ToList();
            //var top = groups.OrderBy(x => x.First().Y).ToList().First();          
            //var bot = groups.OrderBy(x => x.First().Y).ToList().First();

           
        }












        public void Deselect()
        {
            IsSelected = false;
          

        }

        public void Select()
        {

            
            IsSelected = !isSelected;


        }

        public void Refresh()
        {
            throw new System.NotImplementedException();
        }

     
    }

}

