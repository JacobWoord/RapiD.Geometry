using Assimp;
using Assimp.Configs;
using Assimp.Unmanaged;
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

    public partial class Canvas3DViewModel : ObservableObject, IRecipient<ConnectionChangedMessage>
    {


        public string Doorfile;
        public string Shipfile;
        List<Patent3D> Patents = new();

        SharpDX.Plane RefBBplane;
        SharpDX.Plane RefSBplane;

          

        public Canvas3DViewModel()
        {

            camera = new HelixToolkit.Wpf.SharpDX.OrthographicCamera()
            {
                LookDirection = new System.Windows.Media.Media3D.Vector3D(-74, -134, -61), //0.12, -1.1, -11
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

            this.BbDoor = new Door(Doorfile, "bakboord") { doorSide = Side.PortSide };
            this.SbDoor = new Door(Doorfile, "stuurboord") { doorSide = Side.StarBoard };

            modelCollection.Add(new Floor3D(new Vector3(0, 0, -50), new Vector3(750000, 750000, 100)));
            effectsManager = new EffectsManager();

            WeakReferenceMessenger.Default.Register<ConnectionChangedMessage>(this);

            ConnectionControlViewModel connectionControl = new ConnectionControlViewModel();


        }


        [ObservableProperty]
        float patentLength = 2000;

        [ObservableProperty]
        ObservableObject sideViewModel;


      

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

            RefBBplane = new SharpDX.Plane(center, middle, middle + new Vector3(0, 0, 1000));

            /* BABOORD BORD */
            BbDoor.Mirror(MirrorAxis.X);
            BbDoor.UpdateNodeList();

            var bbDoorTopNode = BbDoor.GetTopNode();
            var bbDoorMiddleNode = BbDoor.GetMiddleNode();
            var bbDoorBottomNode = BbDoor.GetBottomNode();

            Patents.Add(CreateDoorPatent(bbDoor, RefBBplane, 4000, 4000));

            //Cables SB
            Vector3 upperCablePositionSb = new Vector3(net.Size.X / 2, net.Size.Y / 2, net.Size.Z / 2);
            Vector3 bottomCablePositionSb = new Vector3(net.Size.X / 2, net.Size.Y / 2, -net.Size.Z / 2);

            Vector3 upperCablePositionBB = new Vector3(-net.Size.X / 2, net.Size.Y / 2, net.Size.Z / 2);
            Vector3 bottomCablePositionBb = new Vector3(-net.Size.X / 2, net.Size.Y / 2, -net.Size.Z / 2);
            var UpperPointBb = net.Position + upperCablePositionBB;
            var BottomPointBb = net.Position + upperCablePositionBB;

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
        Material material = PhongMaterials.Red;



        [ObservableProperty]
        HelixToolkit.Wpf.SharpDX.Camera camera;

        [ObservableProperty]
        ObservableCollection<IModel> modelCollection;



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








        partial void OnSelectedModelChanged(IModel value)
        {
            if (SelectedModel != null)
            {


                if (selectedModel.ConnectionId != "")
                {
                    SideViewModel = new ConnectionControlViewModel(selectedModel.ConnectionId);
                    var viewmodel = SideViewModel as ConnectionControlViewModel;
                    viewmodel.PropertiesViewModel = new ChainPropertiesViewModel();
                    viewmodel.connectionID = selectedModel.ConnectionId;
                    viewmodel.selectedModel = SelectedModel;
                    
                  
                }
                else
                {
                    SideViewModel = null;
                }

            }
            return;

        }


        public Patent3D CreateDoorPatent(Door door, SharpDX.Plane plane, float lengthbot, float lengthtop)
        {
            SharpDX.Plane planeform = plane;
            var doormodel = door;


            var bbDoorTopNode = BbDoor.GetTopNode();
            var bbDoorMiddleNode = BbDoor.GetMiddleNode();
            var bbDoorBottomNode = BbDoor.GetBottomNode();

            Patent3D BbDoorpatent = new Patent3D(bbDoorBottomNode, bbDoorTopNode, bbDoorMiddleNode, lengthbot, lengthtop, planeform);
            var connections = BbDoorpatent.Connections;
            DrawConnections(connections);


            return BbDoorpatent;

        }

        void UpdateDoorPatent(string patentId, string connectionId, ConnectionType conType)
        {
            var connections = new List<ConnectionClass>();

            foreach (var model in modelCollection.Where(m => m.PatentId == patentId).ToList())
            {
                modelCollection.Remove(model);
            }

            var patent = Patents.FirstOrDefault(p => p.Id == patentId);
            if (patent != null)
            {
                connections = patent.Connections;
                var connection = patent.Connections.FirstOrDefault(c => c.Id == connectionId);
                if (connection != null)
                {
                    connection.Type = conType;
                }


            }

            DrawConnections(connections);
        }




        public void DrawConnections(List<ConnectionClass> connections)
        {
            foreach (var connection in connections)
            {
                MeshBuilder meshBuilder = new MeshBuilder();


                foreach (var el in connection.Elements)
                {
                    var path = el.vectors;
                    if (el.ConnectionType != ConnectionType.Chain)
                    {
                        meshBuilder.AddTube(path, 20, 10, true);
                       // path = new List<SharpDX.Vector3> { el.StartPoint, el.EndPoint };
                    }
                    meshBuilder.AddTube(path, el.diameter, 10, true);
                }

                var connectiongeometry = new GeometryBase3D(connection.Id, connection.PatentId)
                {
                    Name = "TEST",
                    MeshGeometry = meshBuilder.ToMeshGeometry3D(),
                    OriginalMaterial = PhongMaterials.Green,
                    CurrentMaterial = PhongMaterials.Blue,

                };

                ModelCollection.Add(connectiongeometry);

            }
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
            model.RotateAroundModelCenter(yaxis: 1, degrees: 90);
            model.RotateAroundModelCenter(zaxis: 1, degrees: -20);
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

                string conid = selectedModel.ConnectionId;



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





        public void UpdateChainStartPoint(IModel Node)
        {
            if (Node is not null)
            {
                var pos = (Node as InfoButton3D).Position;

                //if (SelectedModel is ChainLink3D chain)
                //{
                //    chain.SetNewStartPosition(pos);
                //    chain.Draw();
                //}
            }
        }

        public void UpdateChainEndPoint(IModel Node)
        {
            if (Node is not null)
            {
                var pos = (Node as InfoButton3D).Position;

                //if (SelectedModel is ChainLink3D chain)
                //{
                //    chain.SetNewEndPosition(pos);
                //    chain.Draw();
                //}
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

        public void Select(IModel geometry)
        {


            if (geometry.IsSelected == false)
            {
                geometry.Select();
                SelectedModel = geometry;
            }
            geometry.Select();
            SelectedModel = null;


            //if (geometry.IsSelected == false)
            //{
            //    ModelCollection
            //        .OfType<InfoButton3D>()
            //        .ToList()
            //        .ForEach(x => ModelCollection.Remove(x));
            //}



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



        public void Receive(ConnectionChangedMessage message)
        {
            
            string connectionId;
            string patentId ;
            Patent3D TargetPatent = null;
            ConnectionType connectionType = message.connectionType;
            if (selectedModel != null)
            {
                if (SelectedModel.PatentId != null)
                {
                    connectionId = SelectedModel.ConnectionId;
                    patentId = selectedModel.PatentId;
                    UpdateDoorPatent(patentId, connectionId, connectionType);
                }
                return;
            }
          
            return;









        }





        //string connectionId;
        //string patentId= null;
        //Patent3D patent = null;

        //if (selectedModel.ConnectionID != null)
        //{
        //    connectionId = selectedModel.ConnectionID;

        //    foreach (var Patent in Patents)
        //    {
        //        foreach (var connection in Patent.Connections)
        //        {
        //            if (connectionId == connection.Id)
        //            {

        //                patentId = connection.PatentId;
        //                patent = Patents.FirstOrDefault(x => x.Id == patentId);
        //            }

        //        }


        //    }
        //    if (patentId != null)
        //    {

        //        foreach (var connection in modelCollection.ToList())
        //        {
        //            IModel connectionToDelete = null;
        //            connectionToDelete = ModelCollection.FirstOrDefault(x => x.PatentID == patentId);
        //            if (connectionToDelete != null)
        //                ModelCollection.Remove(connectionToDelete);
        //        }
        //    }




        //}






    }



}



















































