using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Models
{
    public partial class Door : BatchedModel
    {
       
        public Door(string filename)
        {
            Name = "Bord";
            ID = Guid.NewGuid().ToString();

            FileName= filename;

            Task.Factory.StartNew(OpenFile);
        }
       

    }
}
