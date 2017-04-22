using System;
using System.Collections.Generic;

namespace CatalogApi.Settings
{
    [Serializable]
    public class BrandProvider
    {
        public string Uri { get; set; }

        // 22.04.2017 KhryapinAN
        public Int64 ProviderId { get; set; }

        public string IconPath { get; set; }

        public string Title { get; set; }
        public string Commands { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public List<CommandFile> CommandFiles { get; set; }
        public List<ProviderFile> ProviderFiles { get; set; }
    }

    [Serializable]
    public class CommandFile
    {
        public string FileName { get; set; }
        public string FileContent { get; set; }
    }

    [Serializable]
    public class ProviderFile
    {
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public long FileSize { get; set; }
        public byte[] FileContent { get; set; }
    }
}