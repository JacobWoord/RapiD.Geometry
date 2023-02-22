using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using RapiD.Geometry.Models;
using RapiD.Geometry.ViewModels;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                
                //var position = new Vector3(Convert.ToSingle(args.Position.X), Convert.ToSingle(args.Position.Y), Convert.ToSingle(args.Position));
               // (this.DataContext as Canvas3DViewModel).CapturedPos = position;
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

                    if (side == ChainSide.Left)
                    {
                        viewModel.UpdateChainStartPoint(modeldata);
                        viewModel.DeselectAll();
                    }
                        viewModel.UpdateChainEndPoint(modeldata);
                        viewModel.DeselectAll();

                }
                else if(modeldata is ChainLink3D c)
                {
                   
                    var distance1 = SharpDX.Vector3.Distance(hit.PointHit, c.StartPointVector);
                    var distance2 = SharpDX.Vector3.Distance(hit.PointHit, c.EndPointVector);
                    if(distance1< distance2)
                    {
                       viewModel.DeselectAll();

                        viewModel.Select(modeldata,ChainSide.Left);
                        viewModel.SelectedModel = modeldata;
                    }
                    else
                    {
                        viewModel.DeselectAll();

                        viewModel.Select(modeldata, ChainSide.Right);
                        viewModel.SelectedModel = modeldata;
                    }


                  
                    viewModel.DeselectAll();

                    viewModel.Select(modeldata,ChainSide.middle);
                    viewModel.SelectedModel = modeldata;
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
