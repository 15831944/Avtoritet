namespace NewLauncher.Entities
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.CompilerServices;

    public class TabGroupModel
    {
        public int Height { get; set; }

        public int Left { get; set; }

        public ObservableCollection<TabItemModel> TabItemCollection { get; set; }

        public string Title { get; set; }

        public int Top { get; set; }

        public int VisualBorder { get; set; }

        public int Width { get; set; }
    }
}

