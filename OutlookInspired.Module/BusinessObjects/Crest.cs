using System.Drawing;
using DevExpress.Persistent.Base;


namespace OutlookInspired.Module.BusinessObjects{
    [DefaultClassOptions]
    public class Crest:MigrationBaseObject {
        public virtual string CityName { get; set; }
        public virtual byte[] SmallImage { get; set; }
        public virtual byte[] LargeImage { get; set; }
        public virtual ICollection<CustomerStore> CustomerStores { get; set; }
        Image _img;
        public Image LargeImageEx => _img ??= LargeImage.CreateImage();
    }
}