using System.Linq.Expressions;
using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Services{
    internal static class MapExtensions{
        public static IEnumerable<CustomerStore> Stores(this ISalesMapsMarker salesMapsMarker,Period period,DateTime dateTime=default)
            => salesMapsMarker.Orders.Where(period,dateTime:dateTime)
                .GroupBy(order => order.Store).Select(orders => orders.Key);

        static IEnumerable<Order> Where(this IEnumerable<Order> source, Period period, string city=null,DateTime dateTime = default) 
            => source.Where(order => period == Period.ThisYear ? order.OrderDate.Year == DateTime.Now.Year : period == Period.ThisMonth
                ? order.OrderDate.Month == DateTime.Now.Month && order.OrderDate.Year == DateTime.Now.Year
                : period != Period.FixedDate || order.OrderDate.Month == dateTime.Month &&
                order.OrderDate.Year == dateTime.Year && order.OrderDate.Day == dateTime.Day)
                .Where(order => city==null||order.Store.City==city);

        public static MapItem[] Sales(this ISalesMapsMarker salesMapsMarker,Period period,string city=null,DateTime dateTime=default){
            var orders = salesMapsMarker.Orders;
            return orders.Where(period,city, dateTime).SelectMany(order => order.OrderItems)
                .Select(orderItem => new MapItem{
                    Customer = orderItem.Order.Customer,
                    Product = orderItem.Product,
                    Total = orderItem.Total,
                    Latitude = orderItem.Order.Store.Latitude,
                    Longitude = orderItem.Order.Store.Longitude,
                    City = orderItem.Order.Store.City
                }).ToArray();
        }

        public static decimal Opportunity(this IObjectSpace objectSpace,Stage stage,string city)
            => objectSpace.Quotes(stage).Where(q => q.CustomerStore.City == city).TotalSum(q => q.Total);

        public static CustomerStore[] Stores(this IObjectSpace objectSpace, Stage stage){
            return objectSpace.Quotes(stage).Select(quote => quote.CustomerStore).Distinct().ToArray();
            // return from q in objectSpace.Quotes(stage)
            //     join s in stores on q.CustomerStoreId equals s.Id
            //     select s;
        }

        public static QuoteMapItem[] Opportunities(this IObjectSpace objectSpace, Stage stage)
            => objectSpace.Quotes(stage).Select(quote => new QuoteMapItem{
                Stage = stage,
                Value = quote.Total,
                Date = quote.Date,
                City = quote.CustomerStore.City,
                Latitude = quote.CustomerStore.Latitude,
                Longitude = quote.CustomerStore.Longitude
            }).ToArray();
                // .Join(objectSpace.GetObjectsQuery<Customer>(), q => q.Customer.ID, c => c.ID,
                //     (q, c) => new QuoteMapItem{
                //         Stage = stage,
                //         Value = q.Total,
                //         Date = q.Date,
                //         City = q.CustomerStore.City,
                //         Latitude = q.CustomerStore.Latitude,
                //         Longitude = q.CustomerStore.Longitude
                //     });

         
         
        public static IEnumerable<QuoteMapItem> Opportunities(this IObjectSpace objectSpace)
            => Enum.GetValues<Stage>().Where(stage => stage!=Stage.Summary)
                .Select(stage => new QuoteMapItem{ Stage = stage, Value = objectSpace.Quotes( stage).TotalSum(q => q.Total) });

        private static IQueryable<Quote> Quotes(this IObjectSpace objectSpace, Stage stage) 
            => objectSpace.GetObjectsQuery<Quote>().Where( stage);

        public static decimal TotalSum<T>(this IEnumerable<T> query, Expression<Func<T, decimal>> selector){
            var source = query.AsQueryable().Select(selector);
            return !source.Any() ? 0M : source.AsEnumerable().Sum();
        }

        static IQueryable<Quote> Where(this IQueryable<Quote> quotes, Stage stage){
            double min;
            double max;
            switch (stage){
                case Stage.High:
                    max = 1.0;
                    min = 0.6;
                    break;
                case Stage.Medium:
                    min = 0.3;
                    max = 0.6;
                    break;
                case Stage.Low:
                    min = 0.12;
                    max = 0.3;
                    break;
                case Stage.Summary:
                    min = 0.0;
                    max = 1.0;
                    break;
                default:
                    min = 0.0;
                    max = 0.12;
                    break;
            }
            return quotes.Where(quote =>  quote.Opportunity > min && quote.Opportunity < max);
        }

    }
}