//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewLauncher.DataContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class CommandFile
    {
        public int CommandFileId { get; set; }
        public Nullable<int> ProviderId { get; set; }
        public string FileName { get; set; }
        public string FileContent { get; set; }
    
        public virtual Provider Provider { get; set; }
    }
}