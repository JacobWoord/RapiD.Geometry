using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media.Media3D;

namespace RapiD.Geometry.Models
{
    public partial class Door : BatchedModel
    {

        [ObservableProperty]
        Transform3DGroup transform3DGroup = new();
        
        public Door(string filename, string name = "bord")
        {

          
            Name = name + "Bord";
            ID = Guid.NewGuid().ToString();

            FileName= filename;

        }
     

        public void ChangePosition()
        {


        }


        public void Draw()
        {


        }
       

    }
}
