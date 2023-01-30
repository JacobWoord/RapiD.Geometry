using RapiD.Geometry.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace RapiD.Geometry.ViewModels
{
    public partial class Canvas2DViewModel : ObservableObject
    {


        public Canvas2DViewModel()
        {
            geometryCollection = new ObservableCollection<GeometryBase>();

            availableColors = new ObservableCollection<Color>();
            availableColors.Add(Color.FromRgb(255, 0, 0));
            availableColors.Add(Color.FromRgb(37, 31, 220));
            availableColors.Add(Color.FromRgb(255, 196, 31));
            availableColors.Add(Color.FromRgb(0, 255, 213));

        }

        [ObservableProperty]
        ObservableCollection<Color> availableColors;

        
        
        [ObservableProperty]
        GeometryBase selectedGeometry;

       
        
        [ObservableProperty]
        ObservableCollection<GeometryBase> geometryCollection;
        
        [ObservableProperty]
        int radius;

        [ObservableProperty]
        Color color;


        [RelayCommand]
        void DrawCircle()
        {

          Random random = new Random();
          int randomRadius = random.Next(30,150);
          int randomPosX = random.Next(20, 600);
          int randomPosY = random.Next(20, 600);
          Point randomPos= new Point(randomPosX, randomPosY);
          int idCount = geometryCollection.Count;

          geometryCollection.Add(new Circle2D(randomRadius,new Point(randomPosX,randomPosY),idCount+1));

        }

        [RelayCommand]
        void DrawRectangle()
        {

            Random random = new Random();
            int randomDimension = random.Next(30, 150);
            int randomPosX = random.Next(20, 600);
            int randomPosY = random.Next(20, 600);
            Point randomPos = new Point(randomPosX, randomPosY);
            int idCount = geometryCollection.Count;

            geometryCollection.Add(new Rectangle2D(randomDimension, new Point(randomPosX, randomPosY), idCount + 1));

        }


        [RelayCommand]
        void GoHome()
        {
            MainViewModel.Navigate(Ioc.Default.GetService<HomeViewModel>());
        }

















    }
}
