using HelixToolkit.SharpDX.Core;
using HelixToolkit.SharpDX.Core.Assimp;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Models
{
    public abstract partial class BatchedModel : ObservableObject,IModel
    {

        [ObservableProperty]
        IList<BatchedMeshGeometryConfig> batchedMeshes;

        [ObservableProperty]
        IList<Material> modelMaterials;

        [ObservableProperty]
        Material baseMaterial = PhongMaterials.Red;



        public System.Windows.Media.Media3D.Transform3DGroup Transform3DGroup { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string ID { get; set; }
        public bool IsSelected { get; set; }

        public Vector3 Position { get; set; }

        public BatchedModel()
        {
            batchedMeshes= new List<BatchedMeshGeometryConfig>();
            modelMaterials= new List<Material>();
        }
        public async Task OpenFile()
        {
            if(string.IsNullOrEmpty(FileName))
            {
                Debug.WriteLine("No file specified");
                return;
            }

            var configs = Importer3D.OpenFile(FileName);
            var models = configs.Where(x => x.Name.Contains("anchor") == false);
            foreach (var m in models)
            {
                BatchedMeshes.Add(m.BatchedMeshGeometryConfig);
                ModelMaterials.Add(m.MaterialCore.ConvertToMaterial());
            }

            var anchors = configs.Where(x => x.Name.Contains("anchor") == true);

                

         }

        public void Deselect()
        {

        }

        public void Select()
        {

            //IsSelected = !isSelected;

          
        }


    }

}

