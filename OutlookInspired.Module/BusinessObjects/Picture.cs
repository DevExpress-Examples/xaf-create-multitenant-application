

using System.Collections.ObjectModel;
using DevExpress.Persistent.Base;

namespace OutlookInspired.Module.BusinessObjects;
public class Picture :MigrationBaseObject{
    [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit,
        DetailViewImageEditorMode = ImageEditorMode.PictureEdit,ImageSizeMode = ImageSizeMode.AutoSize)]
    public  virtual byte[] Data { get; set; }

    public virtual ObservableCollection<Employee> Employees{ get; set; } = new();
    public virtual ObservableCollection<CustomerEmployee> CustomerEmployees{ get; set; } = new();
    public virtual ObservableCollection<Product> Products{ get; set; } = new();
    public virtual ObservableCollection<ProductImage> ProductImages{ get; set; } = new();
}

