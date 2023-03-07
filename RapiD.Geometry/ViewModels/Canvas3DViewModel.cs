using Assimp;
using CommunityToolkit.Mvvm.Messaging;
using HelixToolkit.SharpDX.Core;
using HelixToolkit.SharpDX.Core.Model.Scene;
using HelixToolkit.Wpf.SharpDX;
using Microsoft.Win32;
using RapiD.Geometry;
using RapiD.Geometry.Messages;
using RapiD.Geometry.Models;
using RapiD.Geometry.Views;
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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

    public partial class Canvas3DViewModel : ObservableObject, IRecipient<patentChangedMessage>, IRecipient<ConnectionChangedMessage>,IRecipient<ConnectionEndPointVectorChangedMessage>
    {


        public string Doorfile;
        public string Shipfile;

        public Canvas3DViewModel()
        {

            camera = new HelixToolkit.Wpf.SharpDX.OrthographicCamera()
            {
                LookDirection = new System.Windows.Media.Media3D.Vector3D(-74, -134,-61), //0.12, -1.1, -11
                UpDirection = new System.Windows.Media.Media3D.Vector3D(-0.21, 0.26, 0.96), /* (0, 0, -1)*/
                Position = new System.Windows.Media.Media3D.Point3D(17437, -17368, 17250),                       /*(4356, -2075, -1086),*/ 
                FarPlaneDistance = 1000000,
                NearPlaneDistance = -1000000,
                Width = 71351
            };

            camera.CreateViewMatrix();
            modelCollection = new ObservableCollection<IModel>();
            string basefolder = Utils2D.GetAppDataFolder();
            Doorfile = basefolder + @"3DModels\FISHINGBOARD_SB.obj";

            this.BbDoor = new Door(Doorfile, "bakboord") { doorSide = Side.PortSide};
            this.SbDoor = new Door(Doorfile, "stuurboord") { doorSide = Side.StarBoard};

            modelCollection.Add(new Floor3D(new Vector3(0, 0, -50), new Vector3(750000, 750000, 100)));
            effectsManager = new EffectsManager();
            diameters = new ObservableCollection<string>();
            diameters.Add("gedag");

            WeakReferenceMessenger.Default.Register<patentChangedMessage>(this);
            WeakReferenceMessenger.Default.Register<ConnectionChangedMessage>(this);
            WeakReferenceMessenger.Default.Register<ConnectionEndPointVectorChangedMessage>(this);
           


        }

        [ObservableProperty]
        ObservableCollection<DoorPatent3D> doorPatents = new();

        [ObservableProperty]
        float patentLength = 2000;

        [ObservableProperty]
        ObservableObject sideViewModel;


        [ObservableProperty]
        ObservableCollection<string> diameters;

        [RelayCommand]
        async Task Initialize()
        {
            await BbDoor.OpenFile();
            await SbDoor.OpenFile();

            // Putting the  SB door in the correct postion. important to note that we Update the node list after all transformations are done!
            await SbDoor.UpdatePositionDoor(xaxis: 4000f);
            SbDoor.UpdateNodeList();

            float spread = Vector3.Distance(bbDoor.Position, sbDoor.Position);

            //NET
            var net = new Squared3D(new Vector3(spread / 2, -40000, 4300), new Vector3(10000, 1000, 8000));
            ModelCollection.Add(net);


            // Line - Helper
            float lineLength = 100000;
            ////sb
            Vector3 center = net.CenterPoint;
            Vector3 middle = net.SbMiddlePoint;
            Vector3 direction = middle - center;
            Vector3 directionNormalized = Vector3.Normalize(direction);
            Vector3 lineEnd = center + directionNormalized * lineLength;
            ////bb
            Vector3 middleBb = net.BbMiddlePoint;
            Vector3 directionBb = middleBb - center;
            Vector3 directionNormalizedBb = Vector3.Normalize(directionBb);
            Vector3 lineEnd1 = center + directionNormalizedBb * lineLength;

            ModelCollection.Add(new Tube3D(center, lineEnd1));
            ModelCollection.Add(new Tube3D(center, lineEnd));

            SharpDX.Plane bbPlane = new SharpDX.Plane(center, middle, middle + new Vector3(0, 0, 1000));


          


            /* BABOORD BORD */
            BbDoor.Mirror(MirrorAxis.X);
            BbDoor.UpdateNodeList();

            var bbDoorTopNode = BbDoor.GetTopNode();
            var bbDoorMiddleNode = BbDoor.GetMiddleNode();
            var bbDoorBottomNode = BbDoor.GetBottomNode();



            
            Patent3D BbDoorpatent = new Patent3D(bbDoorBottomNode, bbDoorTopNode, bbDoorMiddleNode, 4000, 4000,  bbPlane);

            BbDoorpatent.lengthbottom = 6000;
            BbDoorpatent.UpdatePatent();
            BbDoorpatent.UpdateConnections();

           


        
       
        
            /* STUURBOORD BORD */

            //var SbDoorPatent = new DoorPatent3D(Side.StarBoard);
            //SbDoorPatent.InitializeModels(modelCollection);
            //SbDoorPatent.Update(sbDoor.GetNodeList(), modelCollection);
            //DoorPatents.Add(SbDoorPatent);  









            // var startplane = -p.Normal * p.D;
            //var endplane = -p.Normal * p.D * 1.01f;
            //float dist = Vector3.Distance(startplane, endplane);




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




        partial void OnSelectedModelChanged(IModel value)
        {
            switch (selectedModel)
            {
                case ChainLink3D:
                    if ((value as ChainLink3D).PatentId != null)
                    {
                        SideViewModel = new ChainControlViewModel(selectedModel);

                    }
                    // DoorPatent3D linkedPatent = DoorPatents.Where(x => x.id == (value as ChainLink3D).PatentId).First();
                    SideViewModel = new ChainControlViewModel(selectedModel);
                    // SideViewModel = new ChainControlViewModel(selectedModel);
                    break;
                case Squared3D:

                       return;

                    
                      
                    



            }



        }



        void CreatePath()
        {
            List<Vector3> TubePath = new();
            TubePath.Add(new Vector3(0, 0, 0));
            for (int i = 0; i < 50; i++)
            {
                TubePath.Add(new Vector3(TubePath[i].X + 150, 0, 0));
            }

            ModelCollection.Add(new CablePatent(TubePath, 300));
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
                SideViewModel = null;
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

        public void Receive(patentChangedMessage message)
        {

            var id = message.patentId;
            var length = message.totalLength;
            var name = message.name;
            DoorPatent3D Doorpatent  =doorPatents.Where(doorPatents=> doorPatents.id == id).FirstOrDefault();
            if (name == "UpperChain")
            {
                Doorpatent.upperChainLength = length;
                Doorpatent.Update();
            }
            else if (name == "BottomChain")
            {
                Doorpatent.BottomChainLength = length;
                Doorpatent.Update();
            }
            return;


            

        }

        public void Receive(ConnectionChangedMessage message)
        {
            var ConnectionModel = message.chain;
            ModelCollection.Add(ConnectionModel);
        }

        public void Receive(ConnectionEndPointVectorChangedMessage message)
        {
           var connection = modelCollection.Where(modelCollection => modelCollection.ConnectionID == message.connectionId).FirstOrDefault();
            (connection as ChainLink3D).EndPointVector = message.endpoint;
        }
    }


    
    }



















































