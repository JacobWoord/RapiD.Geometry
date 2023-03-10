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
        public string Id { get; set; }  
        public string ConnectionId { get; set; }  
        public string PatentId { get; set; }  
       
        public string Name { get; set; }
        public void Deselect();
        public void Select();

        public float ConnectionLength { get; set; }



        public bool IsSelected { get; set; }


    }
        
}
