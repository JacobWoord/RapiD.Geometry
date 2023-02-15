using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.Models
{
    public interface IModel
    {
        public string ID { get; set; }  
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public void Deselect();
        public void Select();
        
        public bool GetMenuState();

        public bool IsSelected { get; set; }    

    }
}
