using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
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
        public string Name { get; set; }
        public Vector3 Position { get; set; }

        [ObservableProperty]
        HelixToolkit.SharpDX.Core.MeshGeometry3D meshGeometry;

        [ObservableProperty]
        System.Windows.Media.Media3D.Transform3DGroup transform;

        public GeometryBase3D()
        {
            transform = new Transform3DGroup();
            

        }
       
        
        public override string ToString()
        {
            return Name;  //$"{this.GetType()}";
        }





        public virtual void Draw()
        { 

        }

        public void Deselect()
        {
            IsOpenMenu = false;

            IsSelected = false;
            CurrentMaterial = originalMaterial;

        }



        public void Select()
        {

           


            IsOpenMenu = !isOpenMenu;

            IsSelected = !isSelected;

            if (isSelected)
            {


                CurrentMaterial = PhongMaterials.Green;


            }

            else
                CurrentMaterial = originalMaterial;
        }

       
    }
}

        

         
            
            
            
            


               





    

