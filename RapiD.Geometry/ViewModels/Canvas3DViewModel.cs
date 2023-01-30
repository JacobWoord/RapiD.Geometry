using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using RapiD.Geometry.Models;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.ViewModels
{
   public partial class Canvas3DViewModel : ObservableObject
    {
        public Canvas3DViewModel()
        {

            camera = new OrthographicCamera()
            {
                LookDirection = new System.Windows.Media.Media3D.Vector3D(1, 1, -1),
                UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 0, -1),
                Position = new System.Windows.Media.Media3D.Point3D(-500, -500, 500),
                FarPlaneDistance = 10000,
                NearPlaneDistance = -10000,
                Width = 1000
            };
           
            geometry3DCollection = new ObservableCollection<GeometryBase3D>();
            geometry3DCollection.Add(new Sphere3D());

            testMaterial = PhongMaterials.Red;

            var mesh = new MeshBuilder();
            mesh.AddSphere(Vector3.Zero, 100);
            testGeometry = mesh.ToMeshGeometry3D();
        }

        [ObservableProperty]
        Camera camera;

        [ObservableProperty]
        MeshGeometry3D testGeometry;

        [ObservableProperty]
        Material testMaterial;

        [ObservableProperty]
        ObservableCollection<GeometryBase3D> geometry3DCollection;


        [RelayCommand]
        void GoHome()
        {
            MainViewModel.Navigate(Ioc.Default.GetService<HomeViewModel>());
        }


    }


    

}
