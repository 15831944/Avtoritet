namespace NewLauncher.Helper
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class GridHelpers
    {
        public static readonly DependencyProperty ColumnCountProperty = DependencyProperty.RegisterAttached("ColumnCount", typeof(int), typeof(GridHelpers), new PropertyMetadata(-1, new PropertyChangedCallback(GridHelpers.ColumnCountChanged)));
        public static readonly DependencyProperty RowCountProperty = DependencyProperty.RegisterAttached("RowCount", typeof(int), typeof(GridHelpers), new PropertyMetadata(-1, new PropertyChangedCallback(GridHelpers.RowCountChanged)));

        public static void ColumnCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if ((obj is Grid) && (((int) e.NewValue) >= 0))
            {
                Grid grid = (Grid) obj;
                grid.ColumnDefinitions.Clear();
                for (int i = 0; i < ((int) e.NewValue); i++)
                {
                    ColumnDefinition definition = new ColumnDefinition {
                        Width = GridLength.Auto
                    };
                    grid.ColumnDefinitions.Add(definition);
                }
            }
        }

        public static int GetColumnCount(DependencyObject obj)
        {
            return (int) obj.GetValue(ColumnCountProperty);
        }

        public static int GetRowCount(DependencyObject obj)
        {
            return (int) obj.GetValue(RowCountProperty);
        }

        public static void RowCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if ((obj is Grid) && (((int) e.NewValue) >= 0))
            {
                Grid grid = (Grid) obj;
                grid.RowDefinitions.Clear();
                for (int i = 0; i < ((int) e.NewValue); i++)
                {
                    RowDefinition definition = new RowDefinition {
                        Height = GridLength.Auto
                    };
                    grid.RowDefinitions.Add(definition);
                }
                int num2 = 0;
                foreach (FrameworkElement element in grid.Children)
                {
                    Grid.SetRow(element, num2);
                    num2++;
                }
            }
        }

        public static void SetColumnCount(DependencyObject obj, int value)
        {
            obj.SetValue(ColumnCountProperty, value);
        }

        public static void SetRowCount(DependencyObject obj, int value)
        {
            obj.SetValue(RowCountProperty, value);
        }
    }
}

