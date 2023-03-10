using SharpDX;

using Assimp;
using CommunityToolkit.Mvvm.Messaging;
using RapiD.Geometry.Messages;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RapiD.Geometry.Models
{
    public partial class ConnectionClass : ObservableObject,IModel
    {
       


        public Vector3 startVector;
        public Vector3 endVector;

        [ObservableProperty]
        ConnectionType type;


        public PatentSide patentSide = PatentSide.None;
        public float Lenght;

        //data. ToDO: make a data class that stores this data
        public float segmentLengh=100;
        public float dia = 10;
        public float width = 40;

        
        public string Id { get; set; }
        public string ConnectionId { get; set; }
        public string PatentId { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public float ConnectionLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ConnectionClass connectionMessage;


        public List<Element3D> Elements = new();


        public ConnectionClass(Vector3 startVector, Vector3 endVector,ConnectionType contype, string id = "")
        {
            this.PatentId = id;       
            this.type= contype;
            this.startVector = startVector;
            this.endVector = endVector;
            this.Lenght=Vector3.Distance(startVector, endVector);
            int numOfSegments = (int)MathF.Round(this.Lenght / this.segmentLengh);
            this.segmentLengh = Lenght / numOfSegments;

            Id = Guid.NewGuid().ToString();

            CreateConnection();
            
            
            connectionMessage = this;



        }


        
        partial void OnTypeChanged(ConnectionType value)
        {
            Update();
        }






        public void Update()
        {
            this.Lenght = Vector3.Distance(startVector, endVector);
            Elements.Clear();
            CreateConnection();
        }


        public void CreateConnection()
        {

            if (type  == ConnectionType.Chain)
            {
                
                int numOfSegments = (int)MathF.Round(this.Lenght / this.segmentLengh);
                Vector3 direction = Vector3.Normalize(endVector - startVector);

                Vector3 start = startVector;
                for (int i = 0; i < numOfSegments; i++)
                {
                    Vector3 end = start + direction * segmentLengh;
                    bool rotate = false;
                    if (type == ConnectionType.Chain && i % 2 == 1)
                        rotate = true;

                    Element3D el = new Element3D(start, end, type, dia, width, rotate);
                    Elements.Add(el);
                    start = end;
                }
            }
            else if (type == ConnectionType.Rope) 
            {
                Element3D element = new Element3D(startVector, endVector, type);
                Elements.Add(element);
            }

        }










        public void CableConnection()
        {

        }

        public void SteelCableConnection()
        {

        }

        public void RopeConnection()
        {

        }

        public void Deselect()
        {
            throw new System.NotImplementedException();
        }

        public void Select()
        {
            throw new System.NotImplementedException();
        }

        
    }
}
