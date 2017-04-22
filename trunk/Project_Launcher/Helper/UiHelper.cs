namespace NewLauncher.Helper
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public static class UiHelper
    {
        public static BitmapImage CreateBitmapImage(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("Image file not found at: {0}", imagePath);
            }
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(imagePath, UriKind.Relative);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            return image;
        }

        public static DockPanel CreateButtonContent(string imagePath, string text)
        {
            DockPanel panel = new DockPanel {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 150.0,
                Height = 42.0
            };
            panel.Children.Add(CreateButtonImage(imagePath));
            panel.Children.Add(CreateButtonText(text));
            return panel;
        }

        private static Image CreateButtonImage(string imagePath)
        {
            return new Image { HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(2.0, 2.0, 5.0, 2.0), Source = CreateBitmapImage(imagePath), SnapsToDevicePixels = true, Stretch = Stretch.Uniform };
        }

        private static TextBlock CreateButtonText(string text)
        {
            TextBlock element = new TextBlock {
                Text = text,
                Margin = new Thickness(4.0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontWeight = FontWeights.Bold
            };
            DockPanel.SetDock(element, Dock.Right);
            return element;
        }

        public static BitmapImage LoadImage(byte[] imageData)
        {
            if ((imageData == null) || (imageData.Length == 0))
            {
                return null;
            }
            BitmapImage image = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(imageData))
            {
                stream.Position = 0L;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = stream;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}

