using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RapiD.Geometry.Models
{
    public partial class GeometryBase : ObservableObject
    {

        [ObservableProperty]
        Guid id = Guid.NewGuid();

        [ObservableProperty]
        Point position;

        [ObservableProperty]
        Color color = Color.FromRgb(222,45,64);

        [ObservableProperty]
        bool isSelected = false;

       
        

        
        
        
        [RelayCommand]
        void Select()
        {
            IsSelected = !isSelected;
        }
    }

   
}
