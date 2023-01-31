using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using RapiD.Geometry.Models;
using System;
using System.Collections.Generic;
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

        MeshGeometryModel3D? selectedGeometry;
        

        public Canvas3DView()
        {
            InitializeComponent();
        }

        private void Viewport3DX_Loaded(object sender, RoutedEventArgs e)
        {


            (sender as Viewport3DX).EffectsManager = new DefaultEffectsManager();

        }


    }
}
