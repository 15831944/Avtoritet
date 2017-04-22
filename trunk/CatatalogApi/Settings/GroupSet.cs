using System;
using System.Collections.Generic;

namespace CatalogApi.Settings
{
    [Serializable]
    public class GroupSet
    {
        public GroupSet()
        {
            GroupBoxs = new List<GroupBox>();
        }

        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // 21.04.2017 KhryapinAN rem private
        public IList<GroupBox> GroupBoxs { get; /*private*/ set; }
    }
}