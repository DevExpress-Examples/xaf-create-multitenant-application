using System.Linq.Expressions;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Services{
    internal static class OpportunitiesExtensions{
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