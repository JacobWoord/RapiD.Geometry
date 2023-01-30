using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableObject currentViewModel;

        public MainViewModel()
        {
            CurrentViewModel = Ioc.Default.GetService<HomeViewModel>();
        }




        public static void Navigate(ObservableObject givenViewModel)
        {
            var main = Ioc.Default.GetService<MainViewModel>();
            main.CurrentViewModel = givenViewModel;

        }

    }

}
