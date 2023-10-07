using OutlookInspired.Blazor.Server.Components.DevExtreme.PivotGrid;
using OutlookInspired.Blazor.Server.Components.Models;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Features.Quotes.Pivot{
    public class Model:RootListViewComponentModel<Quote,Model,PivotGrid> {
        public override void Refresh(){
            base.Refresh();
            PivotModel.Options.DataSource.Store = Objects.Select(quote => new{
                State = quote.CustomerStore.State.ToString(), quote.CustomerStore.City, quote.Total, quote.Opportunity
            }).ToArray();
            SelectedObjects = Objects.Take(1).ToArray();
        }

        public Components.DevExtreme.PivotGrid.Model PivotModel{ get; } = new(){
            Options ={
                Scrolling={Mode="virtual"},
                DataSource = { Fields ={
                        new PivotGridField{
                            DataField = nameof(CustomerStore.State).FirstCharacterToLower(), Area = "row",
                            SortOrder="desc",SummaryType = "count"
                        },
                        new PivotGridField{
                            DataField = nameof(CustomerStore.City).FirstCharacterToLower(), Area = "row",
                            SortOrder="desc",SummaryType = "count"
                        },
                        new PivotGridField{
                            DataField = nameof(Quote.Total).FirstCharacterToLower(), Area = "data",
                            SummaryType = "sum", DataType = "number", Format = "currency",SortOrder="desc"
                        },
                        new PivotGridField{
                            DataField = nameof(Quote.Opportunity).FirstCharacterToLower(),
                            Area = "data", SummaryType = "avg", DataType = "fixedPoint",
                            SortOrder="desc",IsProgressBar = true
                        },
                        
                }
                }

            }
        };
    }
}