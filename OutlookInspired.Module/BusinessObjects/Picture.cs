

using DevExpress.Persistent.Base;

namespace OutlookInspired.Module.BusinessObjects;
public class Picture :MigrationBaseObject{
    [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit,
        DetailViewImageEditorMode = ImageEditorMode.PictureEdit)]
    public  virtual byte[] Data { get; set; }
    public virtual ICollection<Employee> Employees { get; set; }
    public virtual ICollection<CustomerEmployee> CustomerEmployees { get; set; }
    public virtual ICollection<Product> Products { get; set; }
    public virtual ICollection<ProductImage> ProductImages { get; set; }
}

