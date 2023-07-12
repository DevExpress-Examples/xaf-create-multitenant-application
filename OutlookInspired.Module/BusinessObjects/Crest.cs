using System.Drawing;
using DevExpress.XtraEditors.Controls;

namespace OutlookInspired.Module.BusinessObjects{
    public class Crest:MyBaseObject {
        public virtual string CityName { get; set; }
        public virtual byte[] SmallImage { get; set; }
        public virtual byte[] LargeImage { get; set; }
        public virtual ICollection<CustomerStore> CustomerStores { get; set; }
        Image _img;
        public Image LargeImageEx {
            get {
                if(_img == null)
                    if(LargeImage == null)
                        throw new NotImplementedException();
                        // return ResourceImageHelper.CreateImageFromResourcesEx("DevExpress.DevAV.Resources.Unknown-user.png", typeof(Employee).Assembly);
                    else
                        _img = ByteImageConverter.FromByteArray(LargeImage);
                return _img;
            }
        }
    }
}