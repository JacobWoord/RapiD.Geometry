using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RapiD.Geometry.ViewModels
{
    public partial class ChainPropertiesViewModel : ObservableObject
    {


        [ObservableProperty]
        float diameter;

        [ObservableProperty]
        float width;

        [ObservableProperty]
        float length;



    }
}
