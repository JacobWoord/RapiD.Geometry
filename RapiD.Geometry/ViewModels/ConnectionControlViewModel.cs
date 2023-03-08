using CommunityToolkit.Mvvm.Messaging;
using RapiD.Geometry.Messages;
using RapiD.Geometry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RapiD.Geometry.ViewModels
{
    public partial class ConnectionControlViewModel: ObservableObject
    {
        
        [ObservableProperty]
        ConnectionType connectionType;

        private string connectionID;
        public ConnectionControlViewModel(string connectionId)
        {

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
