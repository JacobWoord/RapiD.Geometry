using HelixToolkit.SharpDX.Core;

using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System.Windows.Media.Media3D;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Material = HelixToolkit.Wpf.SharpDX.Material;
using SharpDX.Direct3D11;

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

            return nodeList;
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

            for (int i = 0; i < nodeList.Count; i++)
            {
                NodeList[i] = (Vector3)Vector3.Transform(NodeList[i], Transform.Value.ToMatrix());
            }
        }


        public async Task UpdatePositionDoor(float xaxis = 0 , float yaxis = 0, float zaxis = 0)
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new System.Windows.Media.Media3D.Vector3D(xaxis, yaxis, zaxis));
            MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);


            Transform.Children.Add(matrixTransform);

        }

        public async Task UpdatePositionDoor(Vector3 direction, float length, Vector3 start )
        {

            Vector3 translate = direction * length;
      
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(start.X, start.Y, start.Z) + new Vector3D(translate.X, translate.Y, translate.Z));
            MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);

            Transform.Children.Add(matrixTransform);

          

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

