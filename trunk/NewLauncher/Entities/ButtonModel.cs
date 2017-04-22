namespace NewLauncher.Entities
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;

    public class ButtonModel
    {
        private ICommand openBrowserCommand;

        public event RoutedEventHandler Click;

        private void OnClick()
        {
            this.Click(this, new RoutedEventArgs());
        }

        public string ButtonStyle { get; set; }

        public string Content { get; set; }

        public object DataContext { get; set; }

        public double Height { get; set; }

        public HorizontalAlignment HorizontalContentAlignment { get; set; }

        public string Login { get; set; }

        public Thickness Margin { get; set; }

        public ICommand OpenBrowserCommand
        {
            get
            {
                return (this.openBrowserCommand ?? (this.openBrowserCommand = new RelayCommand(new Action(this.OnClick))));
            }
        }

        public string Password { get; set; }

        public long ProviderId { get; set; }
    }
}

