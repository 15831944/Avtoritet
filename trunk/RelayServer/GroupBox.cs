//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

public partial class GroupBox
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public GroupBox()
    {
        this.Brand = new HashSet<Brand>();
    }

    public int GroupBoxId { get; set; }
    public string Title { get; set; }
    public Nullable<int> Left { get; set; }
    public Nullable<int> Top { get; set; }
    public int GroupId { get; set; }
    public bool Enable { get; set; }
    public Nullable<int> Width { get; set; }
    public Nullable<int> Height { get; set; }
    public Nullable<int> ButtonWidth { get; set; }
    public Nullable<int> ButtonHeight { get; set; }
    public Nullable<int> ButtonHorizontalPadding { get; set; }
    public Nullable<int> ButtonVerticalPadding { get; set; }
    public Nullable<int> ButtonBetweenVerticalPadding { get; set; }
    public Nullable<int> ButtonBetweenHorizontalPadding { get; set; }
    public Nullable<bool> VisibleBorder { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<Brand> Brand { get; set; }
    public virtual Group Group { get; set; }
}
