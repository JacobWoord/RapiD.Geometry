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
            
            serviceDescriptors.AddTransient<RectangleProps>();


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
