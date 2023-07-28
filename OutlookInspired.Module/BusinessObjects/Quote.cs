using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using DevExpress.ExpressApp.DC;


namespace OutlookInspired.Module.BusinessObjects{
    public class Quote :MigrationBaseObject{
        public  virtual string Number { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual CustomerStore CustomerStore { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual DateTime Date { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal SubTotal { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal ShippingAmount { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal Total { get; set; }
        public virtual  double Opportunity { get; set; }
        [Aggregated]
        public virtual ObservableCollection<QuoteItem> QuoteItems{ get; set; } = new();
    }
}