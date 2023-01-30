using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.ViewModels
{
   public partial class HomeViewModel : ObservableObject    
    {

        public HomeViewModel()
        {
                
        }

        [RelayCommand]
        void Canvas3D()
        {
            MainViewModel.Navigate(Ioc.Default.GetService<Canvas3DViewModel>());
        }

        [RelayCommand]
        void Canvas2D()
        {
            MainViewModel.Navigate(Ioc.Default.GetService<Canvas2DViewModel>());
        }






    }
}
