using CommunityToolkit.Mvvm.Messaging;
using RapiD.Geometry.Messages;
using RapiD.Geometry.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.ViewModels
{
    public partial class ConnectionControlViewModel: ObservableObject,IRecipient<ConnectionListUpdateMessage>
    {
        
        public ObservableCollection<ConnectionClass> connections { get; set; }

      
        [ObservableProperty]
        ObservableObject propertiesViewModel;

        public ConnectionType selectedType;

        public IModel selectedModel;
        
        [ObservableProperty]
        ConnectionType connectionType;

        [ObservableProperty]
        float connectionLength;


        public string connectionID;
      
        
        
        public ConnectionControlViewModel(string connectionId = "")
        {


            
         WeakReferenceMessenger.Default.Register<ConnectionListUpdateMessage>(this);    

        }


        public void Receive(ConnectionListUpdateMessage message)
        {
            connections.Add(message.connection);
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
