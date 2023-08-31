using System.Linq.Expressions;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Services{
    internal static class MapExtensions{
        public static IEnumerable<CustomerStore> Stores(this ISalesMapsMarker salesMapsMarker,Period period,DateTime dateTime=default)
            => salesMapsMarker.Orders.Where(period,dateTime)
                .GroupBy(order => order.Store).Select(orders => orders.Key);

        static IEnumerable<Order> Where(this IEnumerable<Order> source,Period period,DateTime dateTime=default) 
            => source.Where(order => period == Period.ThisYear ? order.OrderDate.Year == DateTime.Now.Year : period == Period.ThisMonth
                ? order.OrderDate.Month == DateTime.Now.Month && order.OrderDate.Year == DateTime.Now.Year
                : period != Period.FixedDate || order.OrderDate.Month == dateTime.Month &&
                order.OrderDate.Year == dateTime.Year && order.OrderDate.Day == dateTime.Day);

        public static MapItem[] SaleMapItems(this ISalesMapsMarker salesMapsMarker,Period period,DateTime dateTime=default) 
            => salesMapsMarker.Orders.Where(period,dateTime).SelectMany(order => order.OrderItems)
                .Select(orderItem => new MapItem {
                    Customer = orderItem.Order.Customer,
                    Product = orderItem.Product,
                    Total = orderItem.Total,
                    Latitude = orderItem.Order.Store.Latitude,
                    Longitude = orderItem.Order.Store.Longitude,
                    City = orderItem.Order.Store.City
                }).ToArray();

        public static IEnumerable<QuoteMapItem> Opportunities(this IQueryable<Quote> quotes)
            => Enum.GetValues<Stage>().Where(stage => stage!=Stage.Summary)
                .Select(stage => new QuoteMapItem{ Stage = stage, Value = quotes.GetQuotes( stage).CustomSum(q => q.Total) });

        static decimal CustomSum<T>(this IEnumerable<T> query, Expression<Func<T, decimal>> selector){
            var source = query.AsQueryable().Select(selector);
            return !source.Any() ? 0M : source.AsEnumerable().Sum();
        }

        static IQueryable<Quote> GetQuotes(this IQueryable<Quote> quotes, Stage stage){
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

            return quotes.Where((Expression<Func<Quote, bool>>)(q => q.Opportunity > min && q.Opportunity < max));
        }

    }
}