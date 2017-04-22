using System.Collections.Generic;

namespace CatalogApi.Settings
{
    public class GroupSet
    {
        public GroupSet()
        {
            GroupBoxs = new List<GroupBox>();
        }

        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public IList<GroupBox> GroupBoxs { get; private set; }
    }
}