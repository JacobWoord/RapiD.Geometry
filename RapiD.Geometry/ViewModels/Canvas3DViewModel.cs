using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using RapiD.Geometry;
using RapiD.Geometry.Models;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Material = HelixToolkit.Wpf.SharpDX.Material;


namespace RapiD.Geometry.ViewModels
{
    public partial class Canvas3DViewModel : ObservableObject
    {
        public Canvas3DViewModel()
        {

            camera = new HelixToolkit.Wpf.SharpDX.OrthographicCamera()
            {
                LookDirection = new System.Windows.Media.Media3D.Vector3D(2.5, 4.5, -3), /*(1, 20, -1)*/
                UpDirection = new System.Windows.Media.Media3D.Vector3D(0.6, 0.6, 0.4), /* (0, 0, -1)*/
                Position = new System.Windows.Media.Media3D.Point3D(-130, -200, 200), /*(-500, -500, 500)*/
                FarPlaneDistance = 10000,
                NearPlaneDistance = -10000,
                Width = 400
            };

            camera.CreateViewMatrix();

            geometry3DCollection = new ObservableCollection<GeometryBase3D>();


        }



        public ElementType SelectedElementType { get; set; }
        public IEnumerable<ElementType> ElementTypes => Enum.GetValues(typeof(ElementType)).Cast<ElementType>();



        [ObservableProperty]
        GeometryBase3D selectedGeometry;

    

        [ObservableProperty]
        Material material = PhongMaterials.Red;



        [ObservableProperty]
        HelixToolkit.Wpf.SharpDX.Camera camera;



        [ObservableProperty]
        ObservableCollection<GeometryBase3D> geometry3DCollection;

        [ObservableProperty]
        float diameter;

        [ObservableProperty]
        double xAxis;

        [ObservableProperty]
        double yAxis;

        [ObservableProperty]
        double zAxis;

        [ObservableProperty]
        float width;

        [ObservableProperty]
        float length;

        [ObservableProperty]
        int numberOfChainCopies;


        public void ShowProperties()
        {
            
        }



        public void DeselectAll()
        {
            // Removes all infobuttons
            geometry3DCollection
              .OfType<InfoButton3D>()
              .ToList()
              .ForEach(x => geometry3DCollection
              .Remove(x));

            // Deselects all models
            geometry3DCollection.ToList().ForEach(x => x.Deselect());
        }

        public void Select(GeometryBase3D geometry)
        {
            geometry.Select();
            ShowButtonIfSelected(geometry);
        }
        void ShowButtonIfSelected(GeometryBase3D geometry)
        {
            if (geometry is not InfoButton3D)
            {
                Geometry3DCollection.Add(new InfoButton3D(geometry.Position));
            }
        }







        [RelayCommand]
        void CreateChain()
        {

            geometry3DCollection.Add(new ChainLink3D(15f, 50, 40f, 5));

        }




        [RelayCommand]
        void Remove()
        {
            geometry3DCollection.Remove(selectedGeometry);
        }



        [RelayCommand]
        void UpdatePositionX()
        {
            if (selectedGeometry != null)
            {
                Matrix3D matrix = new Matrix3D();
                matrix.Translate(new Vector3D(xAxis, 0d, 0d));
                MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);

                // Add the MatrixTransform3D to the TransformGroup
                SelectedGeometry.Transform.Children.Add(matrixTransform);
                SelectedGeometry.Draw(selectedGeometry);

            }
        }

        [RelayCommand]
        void UpdatePositionY()
        {
            if (selectedGeometry != null)
            {
                Matrix3D matrix = new Matrix3D();
                matrix.Translate(new Vector3D(0d, yAxis, 0d));
                MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);

                // Add the MatrixTransform3D to the TransformGroup
                SelectedGeometry.Transform.Children.Add(matrixTransform);
                SelectedGeometry.Draw(selectedGeometry);
            }
        }

        [RelayCommand]
        void UpdatePositionZ()
        {
            if (selectedGeometry != null)
            {
                Matrix3D matrix = new Matrix3D();
                matrix.Translate(new Vector3D(0d, 0d, zAxis));
                MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);

                // Add the MatrixTransform3D to the TransformGroup
                selectedGeometry.Transform.Children.Add(matrixTransform);
                SelectedGeometry.Draw(selectedGeometry);
            }
        }

        [RelayCommand]
        void UpdateWidth()
        {
            float width = this.width;

            if (selectedGeometry == null)
            {
                return;
            }
            else if (selectedGeometry is ChainLink3D chain)
            {
                chain.Width = width;
                chain.DrawChainLink();

            }

        }

        [RelayCommand]
        void UpdateNumberOfCopies()
        {
            int numberOfCopies = this.numberOfChainCopies;

            if (selectedGeometry == null)
            {
                return;
            }
            else if (selectedGeometry is ChainLink3D chain)
            {
                chain.Copies = numberOfCopies;
                chain.DrawChainLink();
            }
        }

        [RelayCommand]
        void UpdateLength()
        {
            float length = this.length;
            if (selectedGeometry == null)
            {
                return;
            }
            else if (selectedGeometry is ChainLink3D chain)
            {
                chain.Length = length;
                chain.DrawChainLink();
            }
        }





        [RelayCommand]
        void DrawSingleChainLink()
        {
            geometry3DCollection.Add(new ChainLink3D(10f, 40f, 65f, 1));



        }

        [RelayCommand]
        void UpdateDiameter()
        {

            float diam = diameter;

            if (selectedGeometry == null)
            {

                return;

            }
            else if (selectedGeometry is Cillinder3D c)
            {
                c.Diameter = diam;

                c.DrawCilinder();
            }
            else if (selectedGeometry is Sphere3D sphere)
            {
                sphere.Radius = diam;
                sphere.DrawSphere();
            }
            else if (selectedGeometry is ChainLink3D chain)
            {
                chain.Diameter = diam;
                chain.DrawChainLink();
            }


        }





        [RelayCommand]
        void DrawTorrus()
        {

            Random random = new Random();
            double diameter = random.NextDouble(10, 300);
            double TubeDiameter = random.NextDouble(10, 300);

            geometry3DCollection.Add(new Torus3D(diameter, TubeDiameter));

        }

        [RelayCommand]
        void DrawCillinder()
        {

            Random random = new Random();
            float p11 = random.Next(10, 30);
            float p12 = random.Next(10, 30);
            float p13 = random.Next(10, 30);

            float p21 = random.Next(10, 30);
            float p22 = random.Next(10, 30);
            float p23 = random.Next(10, 30);


            geometry3DCollection.Add(new Cillinder3D(new Vector3(p11, p12, p13), new Vector3(p21, p23, p22)));

        }


        [RelayCommand]
        void DrawStructure()
        {
            geometry3DCollection.Add(new Structure3D());
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

            geometry3DCollection.Add(new Sphere3D());

        }




        [RelayCommand]
        void GoHome()
        {
            MainViewModel.Navigate(Ioc.Default.GetService<HomeViewModel>());
        }

       
    }




}


















































