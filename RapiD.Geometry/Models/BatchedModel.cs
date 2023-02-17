using Assimp;
using HelixToolkit.SharpDX.Core;

using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Material = HelixToolkit.Wpf.SharpDX.Material;


namespace RapiD.Geometry.Models
{
    public abstract partial class BatchedModel : ObservableObject, IModel
    {

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
        bool isOpenMenu = false;

        [ObservableProperty]
        System.Windows.Media.Media3D.Transform3DGroup transform = new();




        public List<Vector3> GetNodeList() { return nodeList; }




        public string Name { get; set; }
        public string FileName { get; set; }
        public string ID { get; set; }
        //  public bool IsSelected { get; set; }

        public Vector3 Position { get; set; }

        public BatchedModel()
        {
            batchedMeshes = new List<BatchedMeshGeometryConfig>();
            modelMaterials = new List<Material>();
        }



        public void RotateTranform(IModel door)
        {
            RotateTransform3D rotateTransform3D = new RotateTransform3D(new AxisAngleRotation3D(new System.Windows.Media.Media3D.Vector3D(0, 1, 0), -180d));
            (door as BatchedModel).Transform.Children.Add(rotateTransform3D);


        }


        public void UpdatePositionDoor(IModel door)
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new System.Windows.Media.Media3D.Vector3D(8000f, 0f, 0f));
            MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);
            (door as BatchedModel).Transform.Children.Add(matrixTransform);
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
        }





        public void UpdateNodeList()
        {
            for (int i = 0; i < nodeList.Count; i++)
            {
                nodeList[i] = (Vector3)Vector3.Transform(nodeList[i], Transform.Value.ToMatrix());
            }
        }








        public void Deselect()
        {
            IsSelected = false;
            IsOpenMenu = false;

        }

        public void Select()
        {
            
            IsOpenMenu=  !isOpenMenu;
            IsSelected = !isSelected;

          
        }

     

        public bool GetMenuState()
        {
            return IsOpenMenu;
        }
    }

}

