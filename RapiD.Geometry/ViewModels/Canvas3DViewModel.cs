﻿using Assimp;
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



        }


        [ObservableProperty]
        float patentLength = 2000;

        [ObservableProperty]
        ObservableObject sideViewModel;




        [RelayCommand]
        async Task Initialize()
        {
            //await BbDoor.OpenFile();
            await SbDoor.OpenFile();

            // Putting the  SB door in the correct postion. important to note that we Update the node list after all transformations are done!
           // await SbDoor.UpdatePositionDoor(xaxis: 10000f);
            //SbDoor.UpdateNodeList();

           // float spread = Vector3.Distance(bbDoor.Position, sbDoor.Position);
            
            //NET
            var net = new Squared3D(Vector3.Zero, new Vector3(10000, 1000, 8000));
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


            
           // var BbPlane = CreatePlaneFromTwoPoints(center, middleBb);
           SharpDX.Plane SbPlane = CreatePlaneFromTwoPoints(center, middle);
           DefinePlane3D newPlaneSb = new DefinePlane3D(SbPlane, 200000, 100000);

            //BbPlane = new SharpDX.Plane(center, middleBb , middleBb+ new Vector3(0,0,1000));
            //SbPlane = new SharpDX.Plane(center, middle, middle + new Vector3(0, 0, 1000));

            //BbPlane.D *= -1;
            //SbPlane.D *= -1;


            //  DefinePlane3D newPlaneBb = new DefinePlane3D(BbPlane,200000,100000);
            //  modelCollection.Add(newPlaneBb);
            
            ModelCollection.Add(newPlaneSb);

            //sbDoor.UpdateNodeList();
            Vector3 replacementLength = new Vector3(0, 100000, 0);
            //  SbDoor.UpdatePositionDoor(new Vector3(0,50000,0));
           
            SbDoor.UpdatePositionDoor(replacementLength);




            //SbDoor.UpdatePositionDoor(replacementLength);

            //SbDoor.UpdateNodeList();
            
            var nodeTest = sbDoor.GetNodeList();

            // await sbDoor.UpdatePositionDoor(SbPlane, nodeTest[7], Vector3.Zero);
            SbDoor.UpdatePositionDoor(SbPlane, nodeTest[5], Vector3.Zero);

            nodeTest =  SbDoor.GetNodeList();
            // nodeTest = sbDoor.GetNodeList();
            modelCollection.Add(new Sphere3D(nodeTest[5]));


          
            


            //spheres
            //  var centerpointsphere = new Sphere3D(center) { Name = "centersphere"};
            // var middlePointSPhere = new Sphere3D(middleBb) { Name = "middlesphere"};
            //  modelCollection.Add(centerpointsphere);
            // modelCollection.Add(middlePointSPhere);
            // var oorsprong = new Sphere3D(Vector3.Zero);
            //modelCollection.Add(oorsprong);


            /* BABOORD BORD */
            // BbDoor.Mirror(MirrorAxis.X);


            /* BABOORD BORD TO PLANE*/
            // var bbDoorBottomNode = BbDoor.GetBottomNode();



            // BbDoor.UpdateNodeList();
            // SbDoor.UpdateNodeList();
            //var bbDoorTopNode = BbDoor.GetTopNode();

            //SbDoor.UpdatePositionDoor(replacementLength);
            //SbBottomNode = SbDoor.GetMiddleNode();

            // Patent3D bbDoorPatent = CreateDoorPatent(BbDoor, BbPlane, 8000, 8000, center);
            Patent3D sbDoorPatent = CreateDoorPatent(SbDoor, SbPlane, 8000, 8000, center);

            //Patents.Add(bbDoorPatent);
            Patents.Add(sbDoorPatent);


            /*AWAIT*/
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                ModelCollection.Add(SbDoor);
                //ModelCollection.Add(BbDoor);

            });









        }















        public SharpDX.Plane CreatePlaneFromTwoPoints(Vector3 p1, Vector3 p2)
        {
            // Calculate the plane normal by taking the cross product of the two positions
            Vector3 planeNormal = Vector3.Cross(p2 - p1, new Vector3(0, 0, 1));

            // Normalize the plane normal
            planeNormal.Normalize();

            // Calculate the distance from the origin to the plane
            float planeDistance = Vector3.Dot(planeNormal, p1);

            return new SharpDX.Plane(planeNormal, planeDistance);
        }

        public static float DistancePointToPlane(Vector3 point, SharpDX.Plane plane)
        {
            // Calculate the distance from the point to the plane using the plane equation:
            // ax + by + cz + d = 0
            // Where a, b, and c are the components of the plane's normal vector, and d is the distance from the origin to the plane.
            float distance = Vector3.Dot(plane.Normal, point) + plane.D;

            // Return the absolute value of the distance, since the sign indicates which side of the plane the point is on.
            return Math.Abs(distance) / plane.Normal.Length();
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
        IModel lastSelectedModel;


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
                if (SelectedModel.ConnectionId != null)
                {
                    var viewmodel = new ConnectionControlViewModel(SelectedModel);
                    SideViewModel = viewmodel;
                    viewmodel.PropertiesViewModel = new ChainPropertiesViewModel();
                    viewmodel.connectionID = selectedModel.ConnectionId;
                    viewmodel.SelectedModel = SelectedModel;
                    if (SelectedModel.PatentId != null)
                    {
                        LastSelectedModel = selectedModel;
                    }
                }
                else
                {
                    SideViewModel = null;
                }
            }
            else
            {
                SideViewModel = null;
            }
            return;
        }


        public Patent3D CreateDoorPatent(Door door, SharpDX.Plane plane, float lengthbot, float lengthtop,Vector3 center)
        {
            SharpDX.Plane planeform = plane;
            var doormodel = door;
            var centerpoint = center;

            var DoorTopNode = doormodel.GetTopNode();
            var DoorMiddleNode = doormodel.GetMiddleNode();
            var DoorBottomNode = doormodel.GetBottomNode();
            var nodelist = doormodel.GetNodeList();

            Patent3D Doorpatent = new Patent3D(DoorBottomNode, DoorTopNode, DoorMiddleNode, lengthbot, lengthtop, planeform,nodelist,center);
            var connections = Doorpatent.Connections;
            DrawConnections(connections);


            return Doorpatent;

        }
































                    








        public void UpdateChainStartPoint(IModel Node)
        {
            if (Node is not null)
            {
                var pos = (Node as InfoButton3D).Position;

                if(LastSelectedModel.PatentId != null)
                {
                    UpdateDoorPatent(LastSelectedModel.PatentId, LastSelectedModel.ConnectionId, startposition:pos);

                }


            }
        }


        void UpdateDoorPatent(string patentId, string connectionId, ConnectionType conType = ConnectionType.Chain, Vector3 startposition = new Vector3())
        {
            var connections = new List<ConnectionClass>();
            Vector3 newStartPosition = startposition;

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

                    if (newStartPosition != Vector3.Zero)
                    {
                       patent.UpdateTargetCalculation(connectionId, newstartposition:newStartPosition);

                    }
                }


            }

            DrawConnections(connections);
        }




        public void DrawConnections(List<ConnectionClass> connections)
        {
            foreach (var connection in connections)
            {
                MeshBuilder meshBuilder = new MeshBuilder();
                float connectionLength = Vector3.Distance(connection.startVector, connection.endVector);

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

                var connectiongeometry = new GeometryBase3D(connectionLength, connection.Id, connection.PatentId )
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
            SideViewModel = null;


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

            float lengthofConnection;
            string connectionId;
            string patentId;
            ConnectionType currentType;
            Patent3D TargetPatent = null;
            ConnectionType connectionType = message.connectionType;

            if (selectedModel != null)
            {
                if (SelectedModel.PatentId != null)
                {
                    connectionId = SelectedModel.ConnectionId;
                    patentId = selectedModel.PatentId;
                    Patent3D patent = Patents.FirstOrDefault(m => m.Id == patentId);
                    ConnectionClass connection = patent.Connections.FirstOrDefault(c => c.Id == connectionId);

                    if (message.connectionType != connection.Type)
                    {
                        UpdateDoorPatent(patentId, connectionId, connectionType);
                    }
                    if (message.ConnectionLength > 1000)
                    {
                        lengthofConnection = message.ConnectionLength;
                        bool check = patent.UpdateTargetCalculation(connectionId, lengthofConnection);

                        if (check)
                        {
                            UpdateDoorPatent(patentId, connectionId, connectionType);
                            var updatedConnection = modelCollection.FirstOrDefault(c => MathF.Round(c.ConnectionLength) == lengthofConnection);
                            SelectedModel = updatedConnection;

                        }
                        else
                        {
                            MessageBox.Show($"{check}");
                            return;
                        }

                    }
                }

















            }
        }





    }









}























































