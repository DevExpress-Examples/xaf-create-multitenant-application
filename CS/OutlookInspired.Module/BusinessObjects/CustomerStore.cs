﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.Attributes.Validation;


namespace OutlookInspired.Module.BusinessObjects{
	[XafDefaultProperty(nameof(Crest))][ImageName("Shopping_Store")]
	public class CustomerStore :OutlookInspiredBaseObject,IMapsMarker{
		[MaxLength(255)]
		public virtual string Line { get; set; }
		[MaxLength(100)]
		public virtual string City { get; set; }
		public virtual StateEnum State { get; set; }
		[ZipCode][MaxLength(20)]
		public virtual string ZipCode { get; set; }
		public virtual double Latitude { get; set; }
		public virtual double Longitude { get; set; }
		public virtual Customer Customer { get; set; }
		[Attributes.Validation.Phone][MaxLength(20)]
		public virtual string Phone { get; set; }
		[Attributes.Validation.Phone][MaxLength(20)]
		public virtual string Fax { get; set; }
		public virtual int TotalEmployees { get; set; }
		public virtual int SquereFootage { get; set; }
		
		[Column(TypeName = CurrencyType)]
		public virtual decimal AnnualSales { get; set; }
		public virtual Crest Crest { get; set; }
		[MaxLength(255)]
		public virtual string Location { get; set; }
		[Aggregated]
		public virtual ObservableCollection<CustomerEmployee> CustomerEmployees{ get; set; } = new();
		[Aggregated]
		public virtual ObservableCollection<Order> Orders{ get; set; } = new();
		[Aggregated]
		public virtual ObservableCollection<Quote> Quotes{ get; set; } = new();
		string IBaseMapsMarker.Title => Location;
		
	}
}