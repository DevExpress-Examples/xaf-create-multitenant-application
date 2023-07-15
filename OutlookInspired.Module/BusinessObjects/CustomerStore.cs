using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;


namespace OutlookInspired.Module.BusinessObjects{
	public class CustomerStore :MigrationBaseObject{
		[Column("Address_Line")]
		public virtual string AddressLine { get; set; }
		[Column("Address_City")]
		public virtual string AddressCity { get; set; }
		[Column("Address_State")]
		public virtual StateEnum AddressState { get; set; }
		[Column("Address_ZipCode")]
		public virtual string AddressZipCode { get; set; }
		[Column("Address_Latitude")]
		public virtual double AddressLatitude { get; set; }
		[Column("Address_Longitude")]
		public virtual double AddressLongitude { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual string Phone { get; set; }
		public virtual string Fax { get; set; }
		public virtual int TotalEmployees { get; set; }
		public virtual int SquereFootage { get; set; }
		[DataType(DataType.Currency)]
		public virtual decimal AnnualSales { get; set; }
		public virtual Crest Crest { get; set; }
		public virtual string Location { get; set; }
		public virtual ICollection<CustomerEmployee> CustomerEmployees { get; set; }
		public virtual ICollection<Order> Orders { get; set; }
		public virtual ICollection<Quote> Quotes { get; set; }
		public string CustomerName => Customer?.Name;
		public string CrestCity => Crest?.CityName;
		Image _smallImg;
		public Image CrestSmallImage 
			=> _smallImg != null || Crest == null ? _smallImg : _smallImg = Crest.SmallImage.CreateImage();
		
		Image _largeImg;
		public Image CrestLargeImage 
			=> _largeImg != null || Crest == null ? _largeImg : _largeImg = Crest.LargeImage.CreateImage();

	}
}