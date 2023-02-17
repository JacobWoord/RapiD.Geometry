﻿using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RapiD.Geometry.Models
{
    public partial class Door : BatchedModel
    {

        [ObservableProperty]
        Transform3DGroup transform3DGroup = new();
        
        public Door(string filename)
        {

            
            Name = "Bord";
            ID = Guid.NewGuid().ToString();

            FileName= filename;

            Task.Factory.StartNew(OpenFile);
        }


        public void ChangePosition()
        {


        }


        public void Draw()
        {


        }
       

    }
}
