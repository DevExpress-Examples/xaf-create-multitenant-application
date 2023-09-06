using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Office;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers.Maps;

namespace OutlookInspired.Module.Controllers.Customers{
    public class MailMergeController:ObjectViewController<ObjectView,Employee>{
        protected override void OnDeactivated(){
            base.OnDeactivated();
            
            Frame.GetController<RichTextShowInDocumentControllerBase>().ShowInDocumentAction.ItemsChanged-=ShowInDocumentActionOnItemsChanged;
        }

        protected override void OnActivated(){
            base.OnActivated();
            Active[nameof(MapsViewController)] = Frame.GetController<MapsViewController>().MapItAction.Active;
            if (Active){
                Frame.GetController<RichTextShowInDocumentControllerBase>().ShowInDocumentAction.ItemsChanged+=ShowInDocumentActionOnItemsChanged;
            }
        }

        private void ShowInDocumentActionOnItemsChanged(object sender, ItemsChangedEventArgs e){
            if (e.ChangedItemsInfo.All(pair => pair.Value == ChoiceActionItemChangesType.ItemsAdd)){
                ((SingleChoiceAction)sender).Items.ForEach(item => item.ImageName = ((MailMergeDataInfo)item.Data).DisplayName 
                    switch{
                        "Month Award" => "EmployeeQuickAward",
                        "Probation Notice" => "EmployeeQuickProbationNotice",
                        "Service Excellence" => "EmployeeQuickExellece",
                        "Thank You Note" => "ThankYouNote",
                        "Welcome to DevAV" => "EmployeeQuickWelcome",
                        _ => item.ImageName
                    });
            }
        }
    }
}