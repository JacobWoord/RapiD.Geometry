using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RapiD.Geometry.Models
{
    public partial class Element3D : GeometryBase3D
    {
        [ObservableProperty]
        Vector3 startPoint;

        [ObservableProperty]
        Vector3 endPoint;

        [ObservableProperty]
        Color4 color;

        public ElementType ElementType { get; set; }
        






        public Element3D(Vector3 startPoint , Vector3 endPoint)
        {
            ID = Guid.NewGuid().ToString();
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            
        }





    }
}
