﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RapiD.Geometry.Models
{
   public partial class Circle2D : GeometryBase
    {

        public Circle2D(int radius, System.Windows.Point postion , int id)
        {
            this.radius = radius;
            this.Position = postion;
            this.Id = id;
           
        }

        [ObservableProperty]
        int radius;

    }
}