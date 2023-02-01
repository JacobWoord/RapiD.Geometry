using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RapiD.Geometry.Models
{
    public partial class GeometryBase3D : ObservableObject
    {
        

        [ObservableProperty]
        int id;

        [ObservableProperty]
        Vector3 position;

        [ObservableProperty]
        bool isSelected = false;

        [ObservableProperty]
        Material currentMaterial = PhongMaterials.Red;

        private Material originalMaterial; 
        public Material OriginalMaterial
        {
            get { return originalMaterial; }
            set {
                originalMaterial = value;
                CurrentMaterial = value; 
            } 
        }

        [ObservableProperty]
        MeshGeometry3D? meshGeometry;

        [ObservableProperty]
        TransformGroup? transform;

    


        [RelayCommand]
        void Select()
        {
            IsSelected = !isSelected;
            if (isSelected)
                CurrentMaterial = PhongMaterials.Green;
                





            else
                CurrentMaterial = originalMaterial;
        }
    }
}
