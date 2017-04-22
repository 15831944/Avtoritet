namespace NewLauncher.Extension
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class ButtonStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            return base.SelectStyle(item, container);
        }
    }
}

