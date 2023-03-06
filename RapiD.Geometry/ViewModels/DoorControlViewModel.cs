using RapiD.Geometry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.Core.Converters;

namespace RapiD.Geometry.ViewModels
{
    public partial class DoorControlViewModel : ObservableObject
    {

        private IModel door;
        private DoorPatent3D doorPatent;

        [ObservableProperty]
        Side doorside;

        [ObservableProperty]
        string sideNAme = "sidename";

        [ObservableProperty]
        ChainLink3D selectedChain;

        [ObservableProperty]
        float totalLength;

        [ObservableProperty]
        float linkLength;

        [ObservableProperty]
        float diameter;

        [ObservableProperty]
        float width;


        [ObservableProperty]
        List<float> numberOfLinks = new();
        [ObservableProperty]
        float selectedNumOfLinks;


        [ObservableProperty]
        float selectedWidth;

        [ObservableProperty]
        List<float> length = new();
        [ObservableProperty]
        float selectedLength;





        public DoorControlViewModel(IModel selectedChain, DoorPatent3D doorpatent)
        {
            this.selectedChain = (selectedChain as ChainLink3D);
            this.doorPatent = doorpatent;
            this.doorside = doorpatent.doorSide;
          


            totalLength = this.selectedChain.ChainLength;
            this.LinkLength = this.selectedChain.Elementlength;
            this.Diameter= this.selectedChain.Diameter;
            this.Width = this.selectedChain.Width;
        }


      //  [RelayCommand]
      //public void Update()
      //  {

      //      doorPatent.width = this.width;
      //      doorPatent.innerLength = this.linkLength;
      //      doorPatent.diameter = this.diameter;


      //      if (selectedChain.Name == "UpperChain")
      //      {
      //          doorPatent.innerLength = this.linkLength;
      //          doorPatent.width = this.width;
      //          doorPatent.diameter = this.diameter;

      //          doorPatent.UpdateProps();
      //      }
      //      else if (selectedChain.Name == "MiddleChain")
      //      {
      //          return;
      //      }
      //      else if (selectedChain.Name == "BottomChain")
      //      {
      //          doorPatent.BottomChainLength = this.totalLength;
      //          doorPatent.Update(doorPatent.vectors, doorPatent.modelCollection);
      //      }
      //      return;


        


   







    }
}
