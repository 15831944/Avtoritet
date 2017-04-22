﻿using System.Collections.Generic;

namespace CatalogApi.Settings
{
    public class BrandProvider
    {
        public string Uri { get; set; }

        /* KhryapinAN 22.04.2017 */
        public int ProviderId { get; set; }

        public string IconPath { get; set; }

        public string Title { get; set; }
        public string Commands { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public List<CommandFile> CommandFiles { get; set; }
        public List<ProviderFile> ProviderFiles { get; set; }
    }

    public class CommandFile
    {
        public string FileName { get; set; }
        public string FileContent { get; set; }
    }

    public class ProviderFile
    {
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public long FileSize { get; set; }
        public byte[] FileContent { get; set; }
    }
}