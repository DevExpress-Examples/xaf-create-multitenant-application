using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Office;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using static OutlookInspired.Module.Services.MailMergeExtensions;

namespace OutlookInspired.Module.Features.Customers{
    public class MailMergeController:ObjectViewController<ObjectView,Employee>{
        protected override void OnDeactivated(){
            base.OnDeactivated();
            Frame.GetController<RichTextShowInDocumentControllerBase>().ShowInDocumentAction.ItemsChanged-=ShowInDocumentActionOnItemsChanged;
        }

        protected override void OnViewControllersActivated(){
            base.OnViewControllersActivated();
            Active[nameof(MapsViewController)] = Frame.GetController<MapsViewController>().MapItAction.Active;
            if (Active){
                Frame.GetController<RichTextShowInDocumentControllerBase>().ShowInDocumentAction.ItemsChanged+=ShowInDocumentActionOnItemsChanged;
            }
        }

        private void ShowInDocumentActionOnItemsChanged(object sender, ItemsChangedEventArgs e){
            if (e.ChangedItemsInfo.All(pair => pair.Value == ChoiceActionItemChangesType.ItemsAdd)){
                ((SingleChoiceAction)sender).Items.ForEach(item => item.ImageName = ((MailMergeDataInfo)item.Data).DisplayName 
                    switch{
                        MonthAward => "EmployeeQuickAward",
                        ProbationNotice => "EmployeeQuickProbationNotice",
                        ServiceExcellence => "EmployeeQuickExellece",
                        ThankYouNote => "ThankYouNote",
                        WelcomeToDevAV => "EmployeeQuickWelcome",
                        _ => item.ImageName
                    });
            }
        }
    }
}