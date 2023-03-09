global using CommunityToolkit.Mvvm.Collections;
global using CommunityToolkit.Mvvm.Input;
global using CommunityToolkit.Mvvm.ComponentModel;
global using CommunityToolkit.Mvvm.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RapiD.Geometry.ViewModels;
using RapiD.Geometry.Views;
using RapiD.Geometry.Models;

namespace RapiD.Geometry
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection serviceDescriptors = new ServiceCollection();

            serviceDescriptors.AddSingleton<MainWindow>();
            serviceDescriptors.AddSingleton<MainViewModel>();
            serviceDescriptors.AddSingleton<HomeViewModel>();
            serviceDescriptors.AddSingleton<Canvas2DViewModel>();
            serviceDescriptors.AddSingleton<Canvas3DViewModel>();
            serviceDescriptors.AddSingleton<ChainPropertiesViewModel>();


            serviceDescriptors.AddTransient<HomeVIew>();
            serviceDescriptors.AddTransient<ConnectionControlViewModel>();
            serviceDescriptors.AddTransient<RopePropertiesViewModel>();
            serviceDescriptors.AddTransient<CircleViewModel>();
            //serviceDescriptors.AddTransient<Circle2D>();
            //serviceDescriptors.AddTransient<Rectangle2D>();
            //serviceDescriptors.AddTransient<GeometryBase>();
            serviceDescriptors.AddSingleton<Canvas2DView>();
            serviceDescriptors.AddSingleton<Canvas3DView>();


            serviceProvider = serviceDescriptors.BuildServiceProvider();

            Ioc.Default.ConfigureServices(serviceProvider);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
           
            MainWindow = serviceProvider.GetService<MainWindow>();
            MainWindow.DataContext = serviceProvider.GetService<MainViewModel>();

            MainWindow.Show();

            base.OnStartup(e);
        }



    }
}
