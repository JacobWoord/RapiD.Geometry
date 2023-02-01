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
using System.Windows;

namespace RapiD.Geometry.ViewModels
{
   public partial class Canvas3DViewModel : ObservableObject
    {
        public Canvas3DViewModel()
        {

            camera = new OrthographicCamera()
            {
                LookDirection = new System.Windows.Media.Media3D.Vector3D(10, 10, 10), /*(1, 20, -1)*/
                UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 0, 60), /* (0, 0, -1)*/
                Position = new System.Windows.Media.Media3D.Point3D(-500, -500, 500), /*(-500, -500, 500)*/
                FarPlaneDistance = 10000,
                NearPlaneDistance = -10000,
                Width = 1000
            };

            camera.CreateViewMatrix();
           
            geometry3DCollection = new ObservableCollection<GeometryBase3D>();
            
           


        }

        [ObservableProperty]
        GeometryBase3D selectedGeometry;

        [ObservableProperty]
        Material material = PhongMaterials.Red;

        
        
        [ObservableProperty]
        Camera camera;

       

        [ObservableProperty]
        ObservableCollection<GeometryBase3D> geometry3DCollection;

        [ObservableProperty]
        double opacity;

        [RelayCommand]
        void OpenPropertyWindow()
        {

            var propWindow = new Window();
            propWindow.DataContext = this;
            propWindow.Show();

        }

        [RelayCommand]
        void DrawTorrus()
        {

            Random random = new Random();
            double diameter = random.NextDouble(10, 300);
            double TubeDiameter = random.NextDouble(10, 300);

            geometry3DCollection.Add(new Torus3D(diameter,TubeDiameter));

        }

        [RelayCommand]
        void DrawCillinder()
        {

            Random random = new Random();
            float p11 = random.Next(10, 300);
            float p12 = random.Next(10, 300);
            float p13 = random.Next(10, 300);
           
            float p21 = random.Next(10, 300);
            float p22 = random.Next(10, 300);
            float p23 = random.Next(10, 300);
            

            geometry3DCollection.Add(new Cillinder3D(new Vector3(p11,p12,p13), new Vector3(p21,p23,p22)));

        }

        [RelayCommand]
        void DrawTube()
        {

         
            geometry3DCollection.Add(new Tube3D());

        }

        [RelayCommand]
        void DrawSphere()
        {

            Random random = new Random();
            double diameter = random.NextDouble(10, 300);
            double TubeDiameter = random.NextDouble(10, 300);

            geometry3DCollection.Add(new Torus3D(diameter, TubeDiameter));

        }




        [RelayCommand]
        void GoHome()
        {
            MainViewModel.Navigate(Ioc.Default.GetService<HomeViewModel>());
        }


    }


    

}
