using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Material = HelixToolkit.Wpf.SharpDX.Material;

namespace RapiD.Geometry.Models
{
    public partial class GeometryBase3D : ObservableObject,IModel
    {


        [ObservableProperty]
        string fileName;
               
        [ObservableProperty]
        bool isSelected = false;

        [ObservableProperty]
        HelixToolkit.Wpf.SharpDX.Material currentMaterial = PhongMaterials.Red;

        [ObservableProperty]
        bool isOpenMenu = false;

        private HelixToolkit.Wpf.SharpDX.Material originalMaterial;

        public bool linkedButton = false;
        public string ConnectionId { get; set; }


        

        public bool GetMenuState() { return isOpenMenu; }

    


        public Material OriginalMaterial
        {
            get { return originalMaterial; }
            set {
                originalMaterial = value;
                CurrentMaterial = value;
            }
        }

        public string ID { get ; set; }
        public string ConnectionID { get ; set; }
        public string Name { get; set; }

        [ObservableProperty]
        Vector3 position;

        [ObservableProperty]
        HelixToolkit.SharpDX.Core.MeshGeometry3D meshGeometry;

        [ObservableProperty]
        System.Windows.Media.Media3D.Transform3DGroup transform;

        [ObservableProperty]
        CullMode cullMode = CullMode.Back;

        public GeometryBase3D(string connectionId = "")
        {
            ID= Guid.NewGuid().ToString();  
            transform = new Transform3DGroup();
            this.ConnectionID = connectionId;   
            

        }
        public void UpdatePosition()
        {
            meshGeometry.UpdateBounds();
            this.Position = meshGeometry.Bound.Center;
        }

        public override string ToString()
        {
            return Name;  //$"{this.GetType()}";
        }

       public void RotateAroundModelCenter(double xaxis = 0, double yaxis = 0, double zaxis = 0, double degrees = 90)
        {
           
            var axis = new System.Windows.Media.Media3D.Vector3D(xaxis, yaxis, zaxis);
            var rotation = new System.Windows.Media.Media3D.AxisAngleRotation3D(axis, degrees);
            var transform = new System.Windows.Media.Media3D.RotateTransform3D(rotation,position.ToPoint3D());
            this.Transform.Children.Add(transform);
            transform.ToVector3();
        }
        public void Translate(double x = 0, double y = 0, double z = 0)
        {
            var trans = new TranslateTransform3D(x, y, z);
            this.Transform.Children.Add(trans);
            Position += new Vector3((float)x,(float)y,(float)z);
        }



        public virtual void Draw()
        { 

        }

        public void Deselect()
        {

            IsSelected = false;
            CurrentMaterial = originalMaterial;

        }



        public void Select()
        {
            //IsOpenMenu = !isOpenMenu;
            IsSelected = !isSelected;
            if (isSelected)
            {
                CurrentMaterial = PhongMaterials.Yellow;
            }
            else
                CurrentMaterial = originalMaterial;
        }
    }

  
}

           










       

        

         
            
            
            
            


               





    

