namespace NewLauncher.Helper
{
    using System;
    using System.Windows;

    public class ButtonHelpers
    {
        public static readonly DependencyProperty ColGridProperty = DependencyProperty.RegisterAttached("ColGrid", typeof(double), typeof(ButtonHelpers), new PropertyMetadata(0.0));
        public static readonly DependencyProperty RowGridProperty = DependencyProperty.RegisterAttached("RowGrid", typeof(double), typeof(ButtonHelpers), new PropertyMetadata(0.0));

        public static double GetColGrid(UIElement element)
        {
            return (double) element.GetValue(RowGridProperty);
        }

        public static double GetRowGrid(UIElement element)
        {
            return (double) element.GetValue(RowGridProperty);
        }

        public static void SetColGrid(UIElement element, double value)
        {
            element.SetValue(RowGridProperty, value);
        }

        public static void SetRowGrid(UIElement element, double value)
        {
            element.SetValue(RowGridProperty, value);
        }
    }
}

