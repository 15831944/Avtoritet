using System;
using System.Collections.Generic;

namespace CatalogApi.Settings
{
    [Serializable]
    public class Brand
    {
        public Brand()
        {
            Providers = new List<BrandProvider>();
        }

        public int BrandId { get; set; }
        public string IconPath { get; set; }

        public string IconPath2 { get; set; }

        public string NameAndFolder { get; set; }

        public byte[] IconPathImg { get; set; }
        public byte[] IconPath2Img { get; set; }

        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ButtonStyle { get; set; }
        public bool  MenuWindow { get; set; }

        public IList<BrandProvider> Providers { get; set; }
    }
}