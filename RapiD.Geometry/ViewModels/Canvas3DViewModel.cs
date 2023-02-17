using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using Microsoft.Win32;
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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Material = HelixToolkit.Wpf.SharpDX.Material;


namespace RapiD.Geometry.ViewModels
{

    public partial class Canvas3DViewModel : ObservableObject
    {

        public string Doorfile;

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

            modelCollection = new ObservableCollection<IModel>();
            string basefolder = Utils2D.GetAppDataFolder();
            Doorfile = basefolder + @"3DModels\FISHINGBOARD_BB.obj";

            this.myDoor = new Door(Doorfile);
            modelCollection.Add(myDoor);

            this.myDoor = new Door(Doorfile);
            modelCollection.Add(myDoor);


            myDoor.UpdatePositionDoor(myDoor);
            myDoor.RotateTranform(myDoor);
            myDoor.Mirror(MirrorAxis.Z);
         
        }  




        public ElementType SelectedElementType { get; set; }
        public IEnumerable<ElementType> ElementTypes => Enum.GetValues(typeof(ElementType)).Cast<ElementType>();



        [ObservableProperty]
        IModel selectedModel;

        [ObservableProperty]
        Door myDoor;


        [ObservableProperty]
        Material material = PhongMaterials.Red;



        [ObservableProperty]
        HelixToolkit.Wpf.SharpDX.Camera camera;

        [ObservableProperty]
        ObservableCollection<IModel> modelCollection;


        [ObservableProperty]
        ObservableCollection<ChainLink3D> chainsCollection = new();

        //[ObservableProperty]
        //ObservableCollection<BatchedModel> batchedModelCollection;

        //[ObservableProperty]
        //ObservableCollection<GeometryBase3D> geometry3DCollection;

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
        float numberOfChainCopies;

        [ObservableProperty]
        bool isOpenMenu = false;






        public void UpdateChainStartPoint(IModel Node)
        {


            if (Node is not null)
            {
                var pos = (Node as InfoButton3D).Position2;

                if (selectedModel is ChainLink3D chain)
                {
                    chain.SetNewStartPosition(pos);
                    chain.Draw();
                }
            }


        }


 

  




        [RelayCommand]
        async Task OpenFileExplorer()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {

            }
        }




        public void DeselectAll()
        {
            if (modelCollection != null)
            {
                // Removes all infobuttons
                modelCollection
                  .OfType<InfoButton3D>()
                  .ToList()
                  .ForEach(x => modelCollection
                  .Remove(x));

                // Deselects all models
                modelCollection.ToList().ForEach(x => x.Deselect());
            }

            return;
        }

       
        [RelayCommand]
        void SelectFromGrid()
        {
            selectedModel.Select();

        }


        public void Select(IModel geometry)
        {
            geometry.Select();
            bool menuState = geometry.GetMenuState();
            IsOpenMenu = menuState;

            modelCollection
                .OfType<ChainLink3D>()
                .ToList()
                .ForEach(x => chainsCollection.Add(x));

            if (geometry.IsSelected == false)
            {

                modelCollection
                    .OfType<InfoButton3D>()
                    .ToList()
                    .ForEach(x => modelCollection.Remove(x));


            }
            ShowButtonIfSelected(geometry);
        }


        void ShowButtonIfSelected(IModel geometry)
        {
            int count = 0;
            var nodeList = myDoor.GetNodeList();
            int indexOfNodeList = 0;
            if (geometry is ChainLink3D)
                foreach (var node in nodeList)
                {
                    modelCollection.Add(new InfoButton3D(node, count));
                    count++;
                }


        }


         [RelayCommand]
        void CreateChain()
        {

            var nodeList = myDoor.GetNodeList();
            var indexOfList = nodeList;




            modelCollection.Add(new ChainLink3D(15f, 50, 40f, indexOfList[4], new SharpDX.Vector3(-8000, -800, -200)));
            modelCollection.Add(new ChainLink3D(15f, 50, 40f, indexOfList[0], new SharpDX.Vector3(-8000, -800, -200)));



        }




        [RelayCommand]
        void Remove()
        {
            modelCollection.Remove(selectedModel);
        }



        [RelayCommand]
        void UpdatePositionX()
        {
            if (selectedModel != null)
            {
                Matrix3D matrix = new Matrix3D();
                matrix.Translate(new Vector3D(xAxis, 0d, 0d));
                MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);

                // Add the MatrixTransform3D to the TransformGroup
                if (selectedModel is GeometryBase3D g)
                {
                    g.Transform.Children.Add(matrixTransform);
                    g.Draw();
                }
            }
        }

      

        



              

            
             



            [RelayCommand]
            void Replace()
            {
                if (selectedModel != null)
                {
                    Matrix3D matrix = new Matrix3D();
                    matrix.Translate(new Vector3D(0d, 0d, -1000));
                    MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);

                    // Add the MatrixTransform3D to the TransformGroup
                    if (selectedModel is GeometryBase3D g)
                    {
                        g.Transform.Children.Add(matrixTransform);
                        g.Draw();
                    }
                }
            }

            [RelayCommand]
            void UpdateWidth()
            {
                float width = this.width;

                if (selectedModel == null)
                {
                    return;
                }
                else if (selectedModel is ChainLink3D chain)
                {
                    chain.Width = width;
                    chain.Draw();

                }

            }



            [RelayCommand]
            void UpdateLength()
            {
                float length = this.length;
                if (selectedModel == null)
                {
                    return;
                }
                else if (selectedModel is ChainLink3D chain)
                {
                    chain.Length = length;
                    chain.Draw();
                }
            }





            [RelayCommand]
            void DrawSingleChainLink()
            {
                //geometry3DCollection.Add(new ChainLink3D(10f, 40f, 65f, 1));



            }

            [RelayCommand]
            void UpdateDiameter()
            {

                float diam = diameter;

                if (selectedModel == null)
                {

                    return;

                }
                else if (selectedModel is Cillinder3D c)
                {
                    c.Diameter = diam;

                    c.Draw();
                }
                else if (selectedModel is Sphere3D sphere)
                {
                    sphere.Radius = diam;
                    sphere.Draw();
                }
                else if (selectedModel is ChainLink3D chain)
                {
                    chain.Diameter = diam;
                    chain.Draw();
                }


            }





            [RelayCommand]
            void DrawTorrus()
            {

                Random random = new Random();
                double diameter = random.NextDouble(10, 300);
                double TubeDiameter = random.NextDouble(10, 300);

                modelCollection.Add(new Torus3D(diameter, TubeDiameter));

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


                modelCollection.Add(new Cillinder3D(new Vector3(p11, p12, p13), new Vector3(p21, p23, p22)));

            }


            [RelayCommand]
            void DrawStructure()
            {
                modelCollection.Add(new Structure3D());
            }

            [RelayCommand]
            void DrawTube()
            {
                modelCollection.Add(new Tube3D());
            }




            [RelayCommand]
            void DrawSphere()
            {

                Random random = new Random();
                double diameter = random.NextDouble(10, 300);
                double TubeDiameter = random.NextDouble(10, 300);

                modelCollection.Add(new Sphere3D());

            }




            [RelayCommand]
            void GoHome()
            {
                MainViewModel.Navigate(Ioc.Default.GetService<HomeViewModel>());
            }


        }
    }

















































