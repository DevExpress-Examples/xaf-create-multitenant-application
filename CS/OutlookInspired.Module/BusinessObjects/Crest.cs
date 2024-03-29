﻿using System.Collections.ObjectModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;


namespace OutlookInspired.Module.BusinessObjects{
    [XafDefaultProperty(nameof(CityName))]
    [VisibleInReports(false)]
    public class Crest:OutlookInspiredBaseObject {
        public virtual string CityName { get; set; }
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit,
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit)]
        public virtual byte[] SmallImage { get; set; }
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit,
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit)]
        [XafDisplayName("Large")][VisibleInListView(false)][VisibleInLookupListView(false)]
        public virtual byte[] LargeImage { get; set; }

        public virtual ObservableCollection<CustomerStore> CustomerStores{ get; set; } = new();

    }
}