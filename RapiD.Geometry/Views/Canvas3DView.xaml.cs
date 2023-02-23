using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using RapiD.Geometry.Models;
using RapiD.Geometry.ViewModels;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RapiD.Geometry.Views
{
    /// <summary>
    /// Interaction logic for Canvas3DView.xaml
    /// </summary>
    public partial class Canvas3DView : UserControl
    {

        //MeshGeometryModel3D? selectedGeometry;


        public Canvas3DView()
        {
            InitializeComponent();
        }

        private void Viewport3DX_Loaded(object sender, RoutedEventArgs e)
        {


            (sender as Viewport3DX).EffectsManager = new DefaultEffectsManager();

        }

        private void Viewport3DX_MouseDown3D(object sender, RoutedEventArgs e)
        {
            var args = e as MouseDown3DEventArgs;
            var vp = sender as Viewport3DX;
            var hits = vp.FindHits(args.Position);
            Canvas3DViewModel? viewModel = (this.DataContext as Canvas3DViewModel);


            if (hits.Count == 0)
            {
                viewModel.DeselectAll();
                return;
            }
            else if (hits.Count == 1) { }
                var hit = hits.First();
            



                BatchedMeshGeometryModel3D? batchedModel = hit.ModelHit as BatchedMeshGeometryModel3D;
                MeshGeometryModel3D? model = hit.ModelHit as MeshGeometryModel3D;


                if (batchedModel != null)
                {
                    var doorData = batchedModel.DataContext as IModel;


                    viewModel.DeselectAll();
                    viewModel.Select(doorData, ChainSide.middle);
                    viewModel.SelectedModel = doorData;
                }


            


            if (model != null)
            {
                var modeldata = model.DataContext as IModel;

                if (modeldata is InfoButton3D)
                {
                    ChainSide side = viewModel.selectedSide;
                    viewModel.UpdateChainStartPoint(modeldata);

               

                }
                else if(modeldata is ChainLink3D c)
                {
                    viewModel.XAxis = MathF.Round(c.EndPointVector.X,2);
                    viewModel.YAxis = MathF.Round(c.EndPointVector.Y, 2);
                    viewModel.ZAxis = MathF.Round(c.EndPointVector.Z, 2);
                    //Distance between start and end of a chain
                    var distance1 = SharpDX.Vector3.Distance(hit.PointHit, c.StartPointVector);
                    var distance2 = SharpDX.Vector3.Distance(hit.PointHit, c.EndPointVector);

                    //distance between startpoint chain and door to determine the door
                    var bbDoornodes = viewModel.BbDoor.GetNodeList();
                    var sbDoornodes = viewModel.SbDoor.GetNodeList();
                    var distanceToBb = Vector3.Distance(c.StartPointVector, bbDoornodes[3]);
                    var distanceToSb = Vector3.Distance(c.StartPointVector, sbDoornodes[3]);
                   
                        viewModel.Select(modeldata,ChainSide.Left);
                        viewModel.SelectedModel = modeldata;


                        if (distanceToBb < distanceToSb)
                        {
                            //BB Door
                            viewModel.ShowNode(viewModel.BbDoor);
                        }
                    else if (distanceToSb < distanceToBb)
                    {
                        //SB Door
                        viewModel.ShowNode(viewModel.SbDoor);

                    }





                }
                else
                {
                    viewModel.DeselectAll();

                    viewModel.Select(modeldata,ChainSide.middle);
                    viewModel.SelectedModel = modeldata;
                }               


            }

        }



        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            // Get the row that was clicked on
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            // If a row was clicked, raise an event
            if (row != null)
            {
                // Raise a custom event or do something else here
                // ...

            }
        }
    }
}
