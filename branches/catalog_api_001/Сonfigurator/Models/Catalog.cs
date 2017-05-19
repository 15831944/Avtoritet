using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Сonfigurator.Models
{
    public class Catalog
    {
        public int Rows { get; set; }
        public List<Group> Groups { get; set; }
    }

    public class Group
    {
        public string Name { get; set; }
        public List<GroupBox> GroupBoxs { get; set; }

    }
    public class GroupBox
    {
        public string Title { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public List<Brand> Brands { get; set; }

    }

    public class Brand
    {
        public string NameAndFolder { get; set; }
        public string IconPath { get; set; }
        public string IconPath2 { get; set; }

        public int Top { get; set; }
        public int Left { get; set; }

        public bool MenuWindow { get; set; }
        public List<Provider> Providers { get; set; }
    }

    public class Provider
    {
        public string Uri { get; set; }
        public string IconPath { get; set; }
        public string Title { get; set; }
    }
}
