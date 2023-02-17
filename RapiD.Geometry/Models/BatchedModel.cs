using HelixToolkit.SharpDX.Core;
using HelixToolkit.SharpDX.Core.Assimp;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

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




        public List<Vector3> GetNodeList() { return nodeList; }


        public System.Windows.Media.Media3D.Transform3DGroup Transform3DGroup { get; set; }
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



     


        public async Task OpenFile()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                Debug.WriteLine("No file specified");
                return;
            }

            var configs = Importer3D.OpenFile(FileName);
            var models = configs.Where(x => x.Name.Contains("anchor") == false);
            foreach (var m in models)
            {
                //var test = m.BatchedMeshGeometryConfig;
                //m.Location = new Vector3(-200, 200, 200);
                //Debug.WriteLine(m.Location.ToString());

                BatchedMeshes.Add(m.BatchedMeshGeometryConfig);
           
                ModelMaterials.Add(m.MaterialCore.ConvertToMaterial());

              
            }

            var anchors = configs.Where(x => x.Name.Contains("anchor") == true);
            //put all node in list
            foreach (var item in models)
            {
                allNodes.Add(item.Location);
            }

            //positions of nodes in door addes to vector3 List
            foreach (var item in anchors)
            {
                nodeList.Add(item.Location);
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

