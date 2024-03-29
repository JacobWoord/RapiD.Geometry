﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RapiD.Geometry.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Color c)
            {
                return new SolidColorBrush(c);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
