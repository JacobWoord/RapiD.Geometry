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
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
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
        public string Shipfile;

        public Canvas3DViewModel()
        {

            camera = new HelixToolkit.Wpf.SharpDX.OrthographicCamera()
            {
                LookDirection = new System.Windows.Media.Media3D.Vector3D(213, -79, 57), //0.12, -1.1, -11
                UpDirection = new System.Windows.Media.Media3D.Vector3D(0.39, 0.91,0.08), /* (0, 0, -1)*/
                Position = new System.Windows.Media.Media3D.Point3D(4356, -2075, -1086), /*(-500, -500, 500)*/
                FarPlaneDistance = 1000000,
                NearPlaneDistance = -1000000,
                Width = 70876
            };

            camera.CreateViewMatrix();
            modelCollection = new ObservableCollection<IModel>();
            string basefolder = Utils2D.GetAppDataFolder();
            Doorfile = basefolder + @"3DModels\FISHINGBOARD_SB.obj";
            //Shipfile = basefolder + @"3DModels\UK151.obj";

            this.BbDoor = new Door(Doorfile, "bakboord");
            this.SbDoor = new Door(Doorfile, "stuurboord");
           // this.Trawler = new Door(Shipfile, "ship");


            myDoor2.UpdatePositionDoor(myDoor2);
            myDoor2.RotateTranform(myDoor2);
            myDoor2.Mirror(MirrorAxis.Z);





        }
        [RelayCommand]
        async Task Initialize()
        {
            await BbDoor.OpenFile();
            await SbDoor.OpenFile();
            //await Trawler.OpenFile();
           

            // Putting the  SB door in the correct postion. important to note that we Update the node list after all transformations are done!
          await  SbDoor.UpdatePositionDoor(xaxis:10000f);
            SbDoor.RotateTransform(xaxis: 1d, degrees: 90d);
            SbDoor.RotateTransform(yaxis: 1d, degrees: -80d);
            SbDoor.RotateTransform(zaxis: 1d, degrees: 0d);
            SbDoor.Mirror(MirrorAxis.X);
            SbDoor.UpdateNodeList();
            ShowNode(SbDoor);
            /* BABOORD BORD */

           await BbDoor.UpdatePositionDoor(xaxis:10000f);
            BbDoor.RotateTransform(xaxis: 1d, degrees: 90d);
            BbDoor.RotateTransform(yaxis: 1d, degrees: 70d);
            BbDoor.RotateTransform(zaxis: 1d, degrees: 0d);
            BbDoor.UpdateNodeList();

            //create RingConnections for net
            /*SB*/ CreateConnections(-4000, -1400, 10000, degrees: 0);
            /*BB*/ CreateConnections(3000, -1400f, -11400f,degrees:120);


            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                ModelCollection.Add(SbDoor);
                ModelCollection.Add(BbDoor);
            });

            CreateWarp();
            CreateDoorPatent();
        }





      [RelayCommand]
      public void ShowNode(IModel? door)
        {
            int count = 0;
            var parsedDoor = (door as BatchedModel);
            var nodelist = parsedDoor.GetNodeList();

            foreach (var node in nodelist)
            {

                ModelCollection.Add(new InfoButton3D(node, count.ToString()));
                count++;
            }

        }


        
        public void UpdatePositionDoorAndNodes(IModel door)
        {
            BatchedModel? parsedDoor = (door as BatchedModel);

            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(-10000f, 0f, 0f));

            MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);
            parsedDoor.Transform.Children.Add(matrixTransform);
            
        }





        public ElementType SelectedElementType { get; set; }
        public IEnumerable<ElementType> ElementTypes => Enum.GetValues(typeof(ElementType)).Cast<ElementType>();



        [ObservableProperty]
        IModel selectedModel;

        [ObservableProperty]
        Door bbDoor;

        [ObservableProperty]
        Door sbDoor;

        [ObservableProperty]
        Door trawler;

        [ObservableProperty]
        int spread;

        partial void OnSpreadChanged(int value)
        {
         //  UpdatePositionDoorAndNodes(sbdoor);
        }



        [ObservableProperty]
        Material material = PhongMaterials.Red;



        [ObservableProperty]
        HelixToolkit.Wpf.SharpDX.Camera camera;

        [ObservableProperty]
        ObservableCollection<IModel> modelCollection;

        ObservableCollection<Vector3> nodePositions = new();


        [ObservableProperty]
        ObservableCollection<ChainLink3D> chainsCollection = new();

        [ObservableProperty]
        Vector3 capturedPos;

        [ObservableProperty]
        float diameter;

        [ObservableProperty]
        float xAxis;

        [ObservableProperty]
        float yAxis;

        [ObservableProperty]
        float zAxis;

        [ObservableProperty]
        float width;

        [ObservableProperty]
        float length;

        [ObservableProperty]
        float numberOfChainCopies;

        [ObservableProperty]
        ChainLink3D chainModel;

     
        public ChainSide selectedSide;

        
        public void CreateConnections(float x=0, float y=0 , float z=0, double degrees=0)
        {
            ModelCollection.Add(new Torus3D(500, 100, new Vector3(x, y, z), rotateYDegrees:degrees));

        }

    


        [RelayCommand]
        public void RotateModel(object param)
        {
            var model = (SelectedModel as GeometryBase3D);

          
   

            if (model != null)
            {
                switch (param)
                {
                    case "+X":
                        model.RotateAroundModelCenter(xaxis: 1, degrees: 10);
                        break;
                    case "-X":
                        model.RotateAroundModelCenter(xaxis: 1, degrees: -10);
                        break;
                    case "+Y":
                        model.RotateAroundModelCenter(yaxis: 1, degrees: 10);
                        break;
                    case "-Y":
                        model.RotateAroundModelCenter(yaxis: 1, degrees: -10);
                        break;
                    case "+Z":
                        model.RotateAroundModelCenter(zaxis: 1, degrees: 10);
                        break;
                    case "-Z":
                        model.RotateAroundModelCenter(zaxis: 1, degrees: -10);
                        break;

                    default:
                        break;
                }


            }


        }

     
       


        [RelayCommand]
        public void MoveConnection(object parameter)
        {

            GeometryBase3D model = (SelectedModel as GeometryBase3D);

            
          



            if (model != null)
                {
                    string parameterString = parameter.ToString();
                Vector3 NewPosition = model.Transform.ToVector3();
                XAxis = NewPosition.X;
                YAxis = NewPosition.Y;
                ZAxis = NewPosition.Z;



                switch (parameterString)
                {
                    case "+Y":
                        model.Translate(y:200);
                            break;
                    case "-Y":
                        model.Translate(y: -200);
                        break;
                    case "+X":
                        model.Translate(x: 200);
                        break;
                    case "-X":
                        model.Translate(x: -200);
                        break;
                    case "+Z":
                        model.Translate(z: 200);
                        break;
                    case "-Z":
                        model.Translate(z: -200);
                        break;

                    default:
                        break;

                        



                }
            }
                return;
            }
















        void CreateDoorPatent()
        {
            var nodeList1 = SbDoor.GetNodeList();
            var nodeList2 = BbDoor.GetNodeList();
            /*SB*/
            ModelCollection.Add(new ChainLink3D(60f, 160f, 120f, nodeList1[7], new Vector3(-3600, -1400, 10000)));
            ModelCollection.Add(new ChainLink3D(60f, 160f, 120f, nodeList1[5], new Vector3(-3600,-1400,10000)));
            /*BB*/
            ModelCollection.Add(new ChainLink3D(60f, 160f, 120f, nodeList2[7], new Vector3(3000,-1400,-11200)));
            ModelCollection.Add(new ChainLink3D(60f, 160f, 120f, nodeList2[5], new Vector3(3000,-1400,-11200)));
        }


        void CreateWarp()
        {
            var nodeList1 = SbDoor.GetNodeList();
            var nodeList2 = BbDoor.GetNodeList();
        /*SB*/ ModelCollection.Add(new ChainLink3D(60f, 160f, 120f, nodeList1[3], new Vector3(nodeList1[3].X + 3000 * 5, nodeList1[4].Y + 9000, nodeList1[4].Z + -3000)));
        /*BB*/ ModelCollection.Add(new ChainLink3D(60f, 160f, 120f, nodeList2[4], new Vector3(nodeList2[4].X + 3000 * 5, nodeList2[4].Y + 8000, nodeList2[4].Z + 9000)));
        }


        [RelayCommand]
        public void UpdateWarpEndPosition()
        {
            if (selectedModel is ChainLink3D c)
            {
                if (SelectedModel is ChainLink3D chain)
                {
                    chain.SetNewEndPosition(new Vector3(XAxis,YAxis,ZAxis));
                    chain.Draw();
                }
            }
        }
               








        void ShowButtonIfSelected(IModel geometry, ChainSide chainside)
        {
            int count = 0;
            var nodeList1 = BbDoor.GetNodeList();
            var nodeList2 = SbDoor.GetNodeList();
           
            if (geometry is ChainLink3D c)
            {

                if (chainside == ChainSide.Right)
                {
                    selectedSide= ChainSide.Right;
                    foreach (var node in nodeList1)
                    {
                        ModelCollection.Add(new InfoButton3D(node, count.ToString()));
                        count++;
                    }
                }
                else if (chainside == ChainSide.Left)
                {
                   selectedSide= ChainSide.Left;
                    foreach (var node in nodeList2)
                    {
                        
                        ModelCollection.Add(new InfoButton3D(node, count.ToString()));
                        count++;
                    }

 


               
            }

        }



        public void UpdateChainStartPoint(IModel Node)
        {
            if (Node is not null)
            {
                var pos = (Node as InfoButton3D).Position;

                if (SelectedModel is ChainLink3D chain)
                {
                    chain.SetNewStartPosition(pos);
                    chain.Draw();
                }
            }
        }

        public void UpdateChainEndPoint(IModel Node)
        {
            if (Node is not null)
            {
                var pos = (Node as InfoButton3D).Position;

                if (SelectedModel is ChainLink3D chain)
                {
                    chain.SetNewEndPosition(pos);
                    chain.Draw();
                }
            }
        }




        public void DeselectAll()
        {
            if (ModelCollection != null)
            {
                // Removes all infobuttons
                ModelCollection
                  .OfType<InfoButton3D>()
                  .ToList()
                  .ForEach(x => ModelCollection
                  .Remove(x));

                // Deselects all models
                ModelCollection.ToList().ForEach(x => x.Deselect());
            }

            return;
        }

        public void Select(IModel geometry, ChainSide chainside)
        {
            geometry.Select();
            
            
            if (geometry.IsSelected == false)
            {
                ModelCollection
                    .OfType<InfoButton3D>()
                    .ToList()
                    .ForEach(x => ModelCollection.Remove(x));
            }
            ShowButtonIfSelected(geometry, chainside);
        }

        [RelayCommand]
        void CreateChain()
        {

            var nodeList = SbDoor.GetNodeList();
            var nodeList2 = BbDoor.GetNodeList();

            ModelCollection.Add(new ChainLink3D(15f, 50, 40f, nodeList[5], nodeList[5] + nodeList[5].X -1000 ));
           ModelCollection.Add(new ChainLink3D(15f, 50, 40f, nodeList[7], nodeList[7] + nodeList[7].X -1000 + nodeList[7].Y + 1000));
        }






















        [RelayCommand]
        void Remove()
        {
            ModelCollection.Remove(SelectedModel);
        }



     


        [RelayCommand]
        void Replace()
        {
            if (SelectedModel != null)
            {
                Matrix3D matrix = new Matrix3D();
                matrix.Translate(new Vector3D(0d, 0d, -1000));
                MatrixTransform3D matrixTransform = new MatrixTransform3D(matrix);

                // Add the MatrixTransform3D to the TransformGroup
                if (SelectedModel is GeometryBase3D g)
                {
                    g.Transform.Children.Add(matrixTransform);
                    g.Draw();
                }
            }
        }

        [RelayCommand]
        void UpdateWidth()
        {
            float width = this.Width;

            if (SelectedModel == null)
            {
                return;
            }
            else if (SelectedModel is ChainLink3D chain)
            {
                chain.Width = width;
                chain.Draw();

            }

        }



        [RelayCommand]
        void UpdateLength()
        {
            float length = this.Length;
            if (SelectedModel == null)
            {
                return;
            }
            else if (SelectedModel is ChainLink3D chain)
            {
                chain.Length = length;
                chain.Draw();
            }
        }



        [RelayCommand]
        void UpdateDiameter()
        {

            float diam = Diameter;

            if (SelectedModel == null)
            {

                return;

            }
            else if (SelectedModel is Cillinder3D c)
            {
                c.Diameter = diam;

                c.Draw();
            }
            else if (SelectedModel is Sphere3D sphere)
            {
                sphere.Radius = diam;
                sphere.Draw();
            }
            else if (SelectedModel is ChainLink3D chain)
            {
                chain.Diameter = diam;
                chain.Draw();
            }


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


            ModelCollection.Add(new Cillinder3D(new Vector3(p11, p12, p13), new Vector3(p21, p23, p22)));

        }


        [RelayCommand]
        void DrawStructure()
        {
            ModelCollection.Add(new Structure3D());
        }

        [RelayCommand]
        void DrawTube()
        {
            ModelCollection.Add(new Tube3D());
        }




        [RelayCommand]
        void DrawSphere()
        {

            Random random = new Random();
            double diameter = random.NextDouble(10, 300);
            double TubeDiameter = random.NextDouble(10, 300);

            ModelCollection.Add(new Sphere3D());

        }

        [RelayCommand]
        void GoHome()
        {
            MainViewModel.Navigate(Ioc.Default.GetService<HomeViewModel>());
        }


    }
}




















































