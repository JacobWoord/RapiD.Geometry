using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RapiD.Geometry.Views
{
    public class ResourcesHelper
    {
        public static readonly DependencyProperty ChainTemplateProperty = DependencyProperty.RegisterAttached(
            "ChainTemplate",
            typeof(DataTemplate),
            typeof(ResourcesHelper),
            new PropertyMetadata(null));

        public static void SetChainTemplate(DependencyObject element, DataTemplate value)
        {
            element.SetValue(ChainTemplateProperty, value);
        }

        public static DataTemplate GetChainTemplate(DependencyObject element)
        {
            return (DataTemplate)element.GetValue(ChainTemplateProperty);
        }
    }

    }
