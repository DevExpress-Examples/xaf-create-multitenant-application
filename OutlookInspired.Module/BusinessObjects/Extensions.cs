namespace OutlookInspired.Module.BusinessObjects{
    public static class Extensions {
        public static Address UpdateAddress(this Address address, string line, string city, StateEnum state, string zipCode, double latitude, double longtitude) {
            address.Line = line;
            address.City = city;
            address.State = state;
            address.ZipCode = zipCode;
            address.Latitude = latitude;
            address.Longitude = longtitude;
            return address;
        }
    }
}