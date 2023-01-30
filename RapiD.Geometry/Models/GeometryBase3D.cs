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
        Material material = PhongMaterials.Red;

        [ObservableProperty]
        MeshGeometry3D meshGeometry;

        [ObservableProperty]
        TransformGroup transform;

        
        
        
        [RelayCommand]
        void Select()
        {
            IsSelected = !isSelected;
        }
    }
}
