using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatalogApi.Settings
{
    public class GroupBox
    {
        public GroupBox()
        {
            Brands = new List<Brand>();
        }

        public string Title { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public bool VisibleBorder { get; set; }
        public bool MenuWindow { get; set; }

        public IList<Brand> Brands { get; private set; }
    }
}
