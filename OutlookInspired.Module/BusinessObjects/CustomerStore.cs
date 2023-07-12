using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using DevExpress.XtraEditors.Controls;

namespace OutlookInspired.Module.BusinessObjects{
	public class CustomerStore :MyBaseObject{
		public CustomerStore() {
			_address = new Address();
			// _address.PropertyChanged += (s, e) => SetPropertyValue(e.PropertyName, "Address_", (Address)s);
		}

		readonly Address _address;
		[NotMapped]
		public Address Address {
			get => _address.UpdateAddress(AddressLine, AddressCity, AddressState, AddressZipCode, AddressLatitude, AddressLongitude);
			set => _address.UpdateAddress(value.Line, value.City, value.State, value.ZipCode, value.Latitude, value.Longitude);
		}
		[Column("Address_Line")]
		public string AddressLine { get; set; }
		[Column("Address_City")]
		public string AddressCity { get; set; }
		[Column("Address_State")]
		public StateEnum AddressState { get; set; }
		[Column("Address_ZipCode")]
		public string AddressZipCode { get; set; }
		[Column("Address_Latitude")]
		public double AddressLatitude { get; set; }
		[Column("Address_Longitude")]
		public double AddressLongitude { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual long? CustomerId { get; set; }
		public virtual string Phone { get; set; }
		public virtual string Fax { get; set; }
		public virtual int TotalEmployees { get; set; }
		public virtual int SquereFootage { get; set; }
		[DataType(DataType.Currency)]
		public virtual decimal AnnualSales { get; set; }
		public virtual Crest Crest { get; set; }
		public virtual long? CrestId { get; set; }
		public virtual string Location { get; set; }
		public virtual string City => Address == null ? "" : Address.City;
		public virtual StateEnum State => Address?.State ?? StateEnum.CA;
		public virtual ICollection<CustomerEmployee> CustomerEmployees { get; set; }
		public virtual ICollection<Order> Orders { get; set; }
		public virtual ICollection<Quote> Quotes { get; set; }
		public string CustomerName => Customer?.Name;

		public string AddressLines => (Address != null) ? $"{Address.Line}\r\n{Address.State} {Address.ZipCode}" : null;

		public string CrestCity => Crest?.CityName;
		Image _smallImg;
		public Image CrestSmallImage => _smallImg != null || Crest == null ? _smallImg : _smallImg = CreateImage(Crest.SmallImage);
		Image _largeImg;
		public Image CrestLargeImage => _largeImg != null || Crest == null ? _largeImg : _largeImg = CreateImage(Crest.LargeImage);

		Image CreateImage(byte[] data){
			if(data == null)
				throw new NotImplementedException();
				// return ResourceImageHelper.CreateImageFromResourcesEx("DevExpress.DevAV.Resources.Unknown-user.png", typeof(Employee).Assembly);
			return ByteImageConverter.FromByteArray(data);
		}
	}
}