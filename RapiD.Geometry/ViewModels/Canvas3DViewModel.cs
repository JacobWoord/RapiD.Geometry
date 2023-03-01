using Assimp;
using HelixToolkit.SharpDX.Core;
using HelixToolkit.SharpDX.Core.Model.Scene;
using HelixToolkit.Wpf.SharpDX;
using Microsoft.Win32;
using RapiD.Geometry;
using RapiD.Geometry.Models;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
                LookDirection = new System.Windows.Media.Media3D.Vector3D(-786, 17, -194), //0.12, -1.1, -11
                UpDirection = new System.Windows.Media.Media3D.Vector3D(0.24, 0.05, 0.96), /* (0, 0, -1)*/
                Position = new System.Windows.Media.Media3D.Point3D(-2323, 5918, 12919),                       /*(4356, -2075, -1086),*/ 
                FarPlaneDistance = 1000000,
                NearPlaneDistance = -1000000,
                Width = 314782
            };

            camera.CreateViewMatrix();
            modelCollection = new ObservableCollection<IModel>();
            string basefolder = Utils2D.GetAppDataFolder();
            Doorfile = basefolder + @"3DModels\FISHINGBOARD_SB.obj";

            this.BbDoor = new Door(Doorfile, "bakboord");
            this.SbDoor = new Door(Doorfile, "stuurboord");

            modelCollection.Add(new Floor3D(new Vector3(0, 0, -50), new Vector3(750000, 750000, 100)));
            effectsManager = new EffectsManager();

           

           
   }
        [RelayCommand]
        async Task Initialize()
        {
            await BbDoor.OpenFile();
            await SbDoor.OpenFile();


            // Putting the  SB door in the correct postion. important to note that we Update the node list after all transformations are done!
            await SbDoor.UpdatePositionDoor(xaxis: 80000f);
            SbDoor.UpdateNodeList();


            /* BABOORD BORD */
            bbDoor.Mirror(MirrorAxis.X);
            BbDoor.UpdateNodeList();

            var BBpatent = new DoorPatent3D();
            List<ChainLink3D> BbChainPatent = BBpatent.CreateDoorPatent(BbDoor);
            Vector3 BbConnectionPostion = BbChainPatent[1].EndPointVector;

            var BbConnector = new Torus3D(500, 100, new Vector3(BbConnectionPostion.X, BbConnectionPostion.Y - 500 / 2, BbConnectionPostion.Z));
            ModelCollection.Add(BbConnector);
            foreach (var item in BbChainPatent)
            {
                modelCollection.Add(item);
            }



            /* STUURBOORD BORD */

            var SBpatent = new DoorPatent3D();
            List<ChainLink3D> SbChainPatent = SBpatent.CreateDoorPatent(SbDoor);
            Vector3 SbConnectionPosition = SbChainPatent[1].EndPointVector;
            IModel SbConnector = new Torus3D(500, 100, new Vector3(SbConnectionPosition.X, SbConnectionPosition.Y - 500 / 2, SbConnectionPosition.Z));
            modelCollection.Add(SbConnector);
            foreach (var item in SbChainPatent)
            {
                modelCollection.Add(item);
            }


            float spread = Vector3.Distance(SbConnectionPosition, BbConnectionPostion);


            //NET
            var net = new Squared3D(new Vector3(spread / 2, -30000, 4300), new Vector3(10000, 1000, 8000));
            modelCollection.Add(net);
            var netPoints = net.AddNetPoints();
            foreach (var item in netPoints)
            {
                modelCollection.Add(item);
            }
            List<ChainLink3D> netPatentBb = net.CreateNetPatent(netPoints[0].Position, netPoints[2].Position);
            List<ChainLink3D> netPatentSb = net.CreateNetPatent(netPoints[3].Position, netPoints[5].Position);
            foreach (var item in netPatentBb)
            {
                modelCollection.Add(item);
            }
            foreach (var item in netPatentSb)
            {
                modelCollection.Add(item);
            }
            
            // Line - Helper
            float lineLength = 100000;

            Vector3 center = netPoints[7].Position;
            Vector3 middle = netPoints[1].Position;
            Vector3 direction = middle - center;
            Vector3 directionNormalized = Vector3.Normalize(direction);
            Vector3 lineEnd = center + directionNormalized * lineLength;

            ModelCollection.Add(new Tube3D(new Vector3(center.X, center.Y, center.Z), lineEnd));

            var p = new SharpDX.Plane(center, middle, middle + new Vector3(0, 0, 1000));
            var startplane = -p.Normal * p.D;
            var endplane = -p.Normal * p.D * 1.01f;
            float dist = Vector3.Distance(startplane, endplane);


            ModelCollection.Add(new Tube3D(startplane, endplane, 5000) { Name = "PLane" });


           //Cables SB
            Vector3 upperCablePositionSb = new Vector3(net.Size.X / 2, net.Size.Y / 2, net.Size.Z / 2);
            Vector3 bottomCablePositionSb = new Vector3(net.Size.X / 2, net.Size.Y / 2, -net.Size.Z / 2);
            //var cableSb1 = new Tube3D(new Vector3(SbConnectionPosition.X, SbConnectionPosition.Y, SbConnectionPosition.Z), netPatentSb[1].EndPointVector);

            //ModelCollection.Add(cableSb1);


            //Cables BB

            Vector3 upperCablePositionBB = new Vector3(-net.Size.X / 2, net.Size.Y / 2, net.Size.Z / 2);
            Vector3 bottomCablePositionBb = new Vector3(-net.Size.X / 2, net.Size.Y / 2, -net.Size.Z / 2);
            //var cableBb1 = new Tube3D(new Vector3(BbConnectionPostion.X, BbConnectionPostion.Y, BbConnectionPostion.Z), netPatentBb[1].EndPointVector);
            var UpperPointBb = net.Position + upperCablePositionBB;
            var BottomPointBb = net.Position + upperCablePositionBB;

            //ModelCollection.Add(cableBb1);



            //Warp
            ModelCollection.Add(sbDoor.DrawWarp());
            ModelCollection.Add(BbDoor.DrawWarp());

            //PEES


            /*AWAIT*/
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                ModelCollection.Add(SbDoor);
                ModelCollection.Add(BbDoor);
            });


        }











        public ElementType SelectedElementType { get; set; }
        public IEnumerable<ElementType> ElementTypes => Enum.GetValues(typeof(ElementType)).Cast<ElementType>();

        [ObservableProperty]
        EffectsManager effectsManager = new EffectsManager();

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

        [ObservableProperty]
        AxisPlaneGridModel3D planeGrid;


        public ChainSide selectedSide;


        void CreatePath()
        {
            List<Vector3> TubePath = new();
            TubePath.Add(new Vector3(0, 0, 0));
            for (int i = 0; i < 50; i++)
            {
                TubePath.Add(new Vector3(TubePath[i].X + 150, 0, 0));
            }

            modelCollection.Add(new CablePatent(TubePath, 300));
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


        public void CreateBBConnection(Vector3 position)
        {
            var model = new Torus3D(500, 100, new Vector3(position.X, position.Y, position.Z));
            model.RotateAroundModelCenter(yaxis:1, degrees: 90);
            model.RotateAroundModelCenter(zaxis:1, degrees: -20);
            ModelCollection.Add(model);

        }

        public void CreateSBConnection(Vector3 position)
        {
            var model = new Torus3D(500, 100, new Vector3(position.X, position.Y, position.Z));
            model.RotateAroundModelCenter(yaxis: 1, degrees: 90);
            model.RotateAroundModelCenter(zaxis: 1, degrees: 20);
            ModelCollection.Add(model);

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
                        model.Translate(y: 200);
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






        void CreateWarp()
        {
            var nodeList1 = SbDoor.GetNodeList();
            var nodeList2 = BbDoor.GetNodeList();
            /*SB*/
            //ModelCollection.Add(new ChainLink3D(60f, 160f, 120f, nodeList1[3], new Vector3(nodeList1[3].X + 3000 * 5, nodeList1[4].Y + 9000, nodeList1[4].Z + -3000)));
            /*BB*/
           // ModelCollection.Add(new ChainLink3D(60f, 160f, 120f, nodeList2[4], new Vector3(nodeList2[4].X + 3000 * 5, nodeList2[4].Y + 8000, nodeList2[4].Z + 9000)));
        }


        [RelayCommand]
        public void UpdateWarpEndPosition()
        {
            if (selectedModel is ChainLink3D c)
            {
                if (SelectedModel is ChainLink3D chain)
                {
                    chain.SetNewEndPosition(new Vector3(XAxis, YAxis, ZAxis));
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
                    selectedSide = ChainSide.Right;
                    foreach (var node in nodeList1)
                    {
                        ModelCollection.Add(new InfoButton3D(node, count.ToString()));
                        count++;
                    }
                }
                else if (chainside == ChainSide.Left)
                {
                    selectedSide = ChainSide.Left;
                    foreach (var node in nodeList2)
                    {

                        ModelCollection.Add(new InfoButton3D(node, count.ToString()));
                        count++;
                    }





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
        void Remove()
        {
            ModelCollection.Remove(SelectedModel);
        }






        [RelayCommand]
        void GoHome()
        {
            MainViewModel.Navigate(Ioc.Default.GetService<HomeViewModel>());
        }


    }
}


















































