﻿using CommunityToolkit.Mvvm.Messaging;
using RapiD.Geometry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace RapiD.Geometry.ViewModels
{
    public partial class ChainControlViewModel : ObservableObject
    {


        public ChainControlViewModel(IModel selectedChain)
        {
            this.chain = selectedChain as ChainLink3D;
            this.SelectedDiameter = chain.Diameter;
            this.TotalLength = chain.ChainLength;
            this.SelectedWidth = chain.Width;
            this.SelectedLength = chain.Elementlength;
            this.SelectedNumOfLinks = chain.NumberOfCopies;
            this.ChainType= chain.ChainType;
            this.currentLength = totalLength;
            FillNumOfLinksListCalc(numberOfLinks);
            FillNumOfLinksListCalc(length);

        }


        [ObservableProperty]
        ChainLink3D chain;

        [ObservableProperty]
        float totalLength;

        [ObservableProperty]
        float currentLength ;

        [ObservableProperty]
        List<float> diameters = new() {16,20,22,24,26,60,30};

        [ObservableProperty]
        float selectedDiameter;

        [ObservableProperty]
        List<float> numberOfLinks = new();
        [ObservableProperty]
        float selectedNumOfLinks;

        [ObservableProperty]
        List<float>width = new() { 20,30,40,50,60,70,80,90};
        [ObservableProperty]
        float selectedWidth;

        [ObservableProperty]
        List<float> length = new();
        [ObservableProperty]
        float selectedLength;

      


        [ObservableProperty]
        ChainType chainType;


        void FillListCalc()
        {
            var memory = SelectedLength;

            for (int i = 10; i < 20; i++)
            {
                memory += 2;
                length.Add(memory);
            }       
        }

        void FillNumOfLinksListCalc(List<float> list)
        {
            var memory = 5;

            for (int i = 0; i < 100; i++)
            {
                memory += 1;
                list.Add(memory);
            }
        }


        [RelayCommand]
        void Update()
        {
            if (TotalLength != currentLength)
            {
                WeakReferenceMessenger.Default.Send(new patentChangedMessage(TotalLength, chain.PatentId,chain.Name));
                CurrentLength = totalLength;
            }

            chain.Diameter = selectedDiameter;
            chain.Elementlength = selectedLength;
            chain.Width = selectedWidth;

            chain.Draw();
        }



    }
}