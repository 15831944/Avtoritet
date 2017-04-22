using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using NewLauncher.Entities;

namespace NewLauncher.ViewModel
{
    public class TabViewModel
    {
        public TabViewModel()
        {
            
        }

        public int Height { get; set; }

        public string Name { get; set; }

        public ObservableCollection<TabGroupModel> TabGroupCollection { get; set; }

        public int Width { get; set; }
    }
}

