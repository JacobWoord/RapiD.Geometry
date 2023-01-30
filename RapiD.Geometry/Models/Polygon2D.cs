using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RapiD.Geometry.Models
{
    public partial class Polygon2D : GeometryBase   
    {

        public string? Name { get; set; }

        [ObservableProperty]
        List<Point> polygonPoints;

        public override string ToString() => $"Name: {Name}";
       
    }
}
