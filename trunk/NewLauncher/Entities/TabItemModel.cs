using System.Windows;
using GalaSoft.MvvmLight.Command;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CatalogApi.Settings;

namespace NewLauncher.Entities
{
    public class TabItemModel
    {
        public BitmapImage BrandIcon { get; set; }

        public Brand Brand { get; set; }

        public int BrandId { get; set; }

        public string BrandName { get; set; }

        public string ButtonStyle { get; set; }

        public ICommand ClickCommand { get; set; }

        public string Group { get; set; }

        public string GroupBox { get; set; }

        public int Height { get; set; }

        public int Left { get; set; }

        public int Top { get; set; }

        public int Width { get; set; }
    }
}

