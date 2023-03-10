using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging.Abstractions;
using RapiD.Geometry.Messages;
using RapiD.Geometry.Models;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.ViewModels
{
    public partial class ConnectionControlViewModel: ObservableObject
    {
        
        public ObservableCollection<ConnectionClass> connections { get; set; }


        public string connectionID;

        [ObservableProperty]
        ObservableObject propertiesViewModel;

        public ConnectionType selectedType;

        [ObservableProperty]
        IModel selectedModel;

        partial void OnSelectedModelChanged(IModel value)
        {
            Trace.WriteLine("sdffd");
        }

        [ObservableProperty]
        ConnectionType connectionType;


        [ObservableProperty]
        float connectionLength;



        public ConnectionControlViewModel(IModel selectedModel)
        {
            this.selectedModel = selectedModel;

            if (SelectedModel != null)
            {
                
                var parsedImodel = selectedModel as GeometryBase3D;
                connectionLength = MathF.Round(SelectedModel.ConnectionLength);
            }
          
        } 




      
        



        [RelayCommand]
        void UpdateLength()
        {
            WeakReferenceMessenger.Default.Send(new ConnectionChangedMessage(connectionLength));

        }


        [RelayCommand]
        public void ChangeConnectionType(object parameter)
        {
            string param = parameter.ToString();
            ConnectionType connectionType;

            switch (param)
            {
                case "chain":
                    WeakReferenceMessenger.Default.Send(new ConnectionChangedMessage(ConnectionType.Chain));
                    break;
                case "cable":
                    WeakReferenceMessenger.Default.Send(new ConnectionChangedMessage(ConnectionType.SteelCable));

                    break;
                case "rope":
                    WeakReferenceMessenger.Default.Send(new ConnectionChangedMessage(ConnectionType.Rope));
                    break;
                default:
                    break;

            }








        }

    }
}
