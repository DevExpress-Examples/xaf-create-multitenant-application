using System.Linq.Expressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EFCore;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;

namespace OutlookInspired.Module.Services.Internal{
    internal static class MapExtensions{
        public static string MapItemProperty(this Type salesMarkerType)
            => salesMarkerType switch{
                _ when typeof(Customer).IsAssignableFrom(salesMarkerType) => nameof(MapItem.ProductName),
                _ when typeof(Product).IsAssignableFrom(salesMarkerType) => nameof(MapItem.CustomerName),
                _ => throw new InvalidOperationException($"Invalid type provided. {salesMarkerType}")
            };

        public static void SetRoutePoints(this Employee employee,params RoutePoint[] routePoints){
            employee.RoutePoints.Clear();
            routePoints.Do(employee.RoutePoints.Add).Enumerate();
        }
        public static MapItem[] Sales(this ISalesMapsMarker salesMapsMarker, Period period, string city = null) 
            => salesMapsMarker.ObjectSpace.GetObjectsQuery<OrderItem>()
                .Where(salesMapsMarker.SalesExpression)
                .Where(period,city)
                .Select(item => new MapItem{
                    CustomerName = item.Order.Customer.Name,
                    ProductName = item.Product.Name,
                    ProductCategory = item.Product.Category,
                    Total = item.Total,
                    Latitude = item.Order.Store.Latitude,
                    Longitude = item.Order.Store.Longitude,
                    City = item.Order.Store.City
                }).ToArray().Do((item, i) => item.ID=i).ToArray();

        public static IQueryable<CustomerStore> Stores(this ISalesMapsMarker salesMapsMarker,Period period,DateTime dateTime=default) 
            => salesMapsMarker.ObjectSpace.GetObjectsQuery<Order>()
                .Where(period, dateTime: dateTime).GroupBy(order => order.Store)
                .Select(orders => orders.Key);

        static IQueryable<Order> Where(this IQueryable<Order> source, Period period, string city=null,DateTime dateTime = default) 
            => source.Where(order => period == Period.ThisYear ? order.OrderDate.Year == DateTime.Now.Year : period == Period.ThisMonth
                ? order.OrderDate.Month == DateTime.Now.Month && order.OrderDate.Year == DateTime.Now.Year
                : period != Period.FixedDate || order.OrderDate.Month == dateTime.Month &&
                order.OrderDate.Year == dateTime.Year && order.OrderDate.Day == dateTime.Day)
                .Where(order => city==null||order.Store.City==city);
        
        static IQueryable<OrderItem> Where(this IQueryable<OrderItem> source, Period period, string city=null) 
            => source.Where(item => (period == Period.ThisYear ? item.Order.OrderDate.Year == DateTime.Now.Year
                : period == Period.ThisMonth ? item.Order.OrderDate.Month == DateTime.Now.Month && item.Order.OrderDate.Year == DateTime.Now.Year
                : period != Period.FixedDate) &&(city==null||item.Order.Store.City==city));


        public static decimal Opportunity(this IObjectSpace objectSpace,Stage stage,string city)
            => objectSpace.Quotes(stage).Where(q => q.CustomerStore.City == city).TotalSum(q => q.Total);

        public static CustomerStore[] Stores(this IObjectSpace objectSpace, Stage stage) 
            => objectSpace.Quotes(stage).Select(quote => quote.CustomerStore).Distinct().ToArray();

        public static QuoteMapItem[] Opportunities(this IObjectSpace objectSpace, Stage stage)
            => objectSpace.Quotes(stage).Select(quote => new QuoteMapItem{
                Stage = stage,
                Value = quote.Total,
                Date = quote.Date,
                City = quote.CustomerStore.City,
                Latitude = quote.CustomerStore.Latitude,
                Longitude = quote.CustomerStore.Longitude
            }).ToArray().Do((item, i) => item.ID=i).ToArray();
         
        public static IEnumerable<QuoteMapItem> Opportunities(this IObjectSpace objectSpace,string criteria=null)
            => Enum.GetValues<Stage>().Where(stage => stage!=Stage.Summary)
                .Select(stage => new QuoteMapItem
                { Stage = stage, Value = ((IQueryable<Quote>)objectSpace.YieldAll().OfType<EFCoreObjectSpace>().First()
                        .Query(typeof(Quote), criteria))
                    .Where(stage).TotalSum(q => q.Total) });

        private static IQueryable<Quote> Quotes(this IObjectSpace objectSpace, Stage stage) 
            => objectSpace.GetObjectsQuery<Quote>().Where( stage);

        public static decimal TotalSum<T>(this IEnumerable<T> query, Expression<Func<T, decimal>> selector){
            var source = query.AsQueryable().Select(selector);
            return !source.Any() ? 0M : source.AsEnumerable().Sum();
        }

        static IQueryable<Quote> Where(this IQueryable<Quote> quotes, Stage stage){
            var (min, max) = new Dictionary<Stage, (double, double)>{
                [Stage.High] = (0.6, 1.0), [Stage.Medium] = (0.3, 0.6),
                [Stage.Low] = (0.12, 0.3), [Stage.Summary] = (0.0, 1.0),
            }.GetValueOrDefault(stage, (0.0, 0.12));
            return quotes.Where(quote => quote.Opportunity > min && quote.Opportunity < max);
        }

    }
}