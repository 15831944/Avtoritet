namespace NewLauncher.Extension
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    internal class StyleConverter1 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string str = values[0] as string;
            Style style = values[1] as Style;
            Style style2 = values[2] as Style;
            Style style3 = values[3] as Style;
            return (str.Equals("ButtonStyle1") ? style : (str.Equals("ButtonStyle2") ? style2 : style3));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

