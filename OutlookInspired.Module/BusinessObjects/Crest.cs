using System.Drawing;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;


namespace OutlookInspired.Module.BusinessObjects{
    [DefaultClassOptions]
    public class Crest:MigrationBaseObject {
        public virtual string CityName { get; set; }
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit,
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit)]
        public virtual byte[] SmallImage { get; set; }
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit,
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit)]
        [XafDisplayName("Large")][VisibleInListView(false)][VisibleInLookupListView(false)]
        public virtual byte[] LargeImage { get; set; }
        public virtual ICollection<CustomerStore> CustomerStores { get; set; }
        
    }
}