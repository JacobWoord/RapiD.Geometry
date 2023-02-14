using System.Windows;


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
