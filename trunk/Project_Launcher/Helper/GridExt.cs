namespace NewLauncher.Helper
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class GridExt
    {
        public static readonly DependencyProperty ItemsPerRow = DependencyProperty.RegisterAttached("ItemsPerRow", typeof(int), typeof(GridExt), new PropertyMetadata(-1, new PropertyChangedCallback(GridExt.OnItemsPerRowPropertyChanged)));

        public static void OnItemsPerRowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Grid grid = d as Grid;
            int itemsPerRow = (int) e.NewValue;
            grid.LayoutUpdated += delegate (object s, EventArgs e2) {
                int count = grid.Children.Count;
                int num2 = (count - grid.RowDefinitions.Count) / itemsPerRow;
                for (int k = 0; k < num2; k++)
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                }
                for (int i = 0; i < count; i++)
                {
                    FrameworkElement element = grid.Children[i] as FrameworkElement;
                    Grid.SetRow(element, i / itemsPerRow);
                }
            };
        }
    }
}

