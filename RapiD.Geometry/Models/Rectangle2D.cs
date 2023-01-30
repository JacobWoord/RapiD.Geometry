using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Models
{
    public partial class Rectangle2D : GeometryBase
    {

        public Rectangle2D(int height, System.Windows.Point postion, int id)
        {
            this.Height = height;
            this.Position = postion;
            this.Id = id;   

        }


        [ObservableProperty]
        int height;

        public int width  => height;
        
    }
}
