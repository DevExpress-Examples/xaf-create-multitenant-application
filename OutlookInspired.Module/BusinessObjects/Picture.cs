using System.Drawing;
using DevExpress.XtraEditors.Controls;

namespace OutlookInspired.Module.BusinessObjects;
public class Picture :BaseObject{
    public  virtual byte[] Data { get; set; }
    public virtual ICollection<Employee> Employees { get; set; }
    public virtual ICollection<CustomerEmployee> CustomerEmployees { get; set; }
    public virtual ICollection<Product> Products { get; set; }
    public virtual ICollection<ProductImage> ProductImages { get; set; }
}

static class PictureExtension {
    public const string DefaultPic = DefaultUserPic;
    public const string DefaultUserPic = "DevExpress.DevAV.Resources.Unknown-user.png";
    internal static Image CreateImage(this Picture picture, string defaultImage = null){
        if(picture == null) {
            if(string.IsNullOrEmpty(defaultImage))
                defaultImage = DefaultPic;
            // return ResourceImageHelper.CreateImageFromResourcesEx(defaultImage, typeof(Picture).Assembly);
            throw new NotImplementedException();
        }

        return ByteImageConverter.FromByteArray(picture.Data);
    }
    internal static Picture FromImage(this Image image) 
        => image == null ? null : new Picture{
            Data = ByteImageConverter.ToByteArray(image, image.RawFormat)
        };
}
