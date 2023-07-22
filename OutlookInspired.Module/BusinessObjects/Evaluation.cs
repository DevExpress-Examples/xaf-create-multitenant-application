

using System.Drawing;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.Attributes;

namespace OutlookInspired.Module.BusinessObjects{
    using DevExpress.ExpressApp;
    using DevExpress.ExpressApp.ConditionalAppearance;
    using System;
//...
    public abstract class ConditionalAppearanceController : ViewController {
        protected ConditionalAppearanceController() {
            TargetObjectType = typeof(Evaluation);
        }
        private AppearanceController _appearanceController;
        protected override void OnActivated() {
            base.OnActivated();
            _appearanceController = Frame.GetController<AppearanceController>();
            if (_appearanceController != null) {
                _appearanceController.CustomApplyAppearance += appearanceController_CustomApplyAppearance;
            }
        }
        void appearanceController_CustomApplyAppearance(object sender, ApplyAppearanceEventArgs e) => CustomizeDisabledEditorsAppearance(e);

        protected abstract void CustomizeDisabledEditorsAppearance(ApplyAppearanceEventArgs e);
        protected override void OnDeactivated() {
            if (_appearanceController != null) {
                _appearanceController.CustomApplyAppearance -=
                    appearanceController_CustomApplyAppearance;
            }
            base.OnDeactivated();
        }
    }
    [Appearance(nameof(CreatedOn),AppearanceItemType.ViewItem, "1=1",TargetItems = nameof(CreatedOn),FontStyle = FontStyle.Bold,Context = "Employee_Evaluations_ListView")]
    [Appearance(nameof(Manager),AppearanceItemType.ViewItem, "1=1",TargetItems = nameof(Manager),FontStyle = FontStyle.Bold,Context = "Employee_Evaluations_ListView_Child")]
    [Appearance(nameof(CreatedOn)+"_Employee_Evaluations_ListView_Child",AppearanceItemType.ViewItem, "1=1",TargetItems = nameof(CreatedOn),FontColor = "Blue",Context = "Employee_Evaluations_ListView_Child")]
    [Appearance(nameof(Rating),AppearanceItemType.ViewItem, nameof(Rating)+"='"+nameof(EvaluationRating.Good)+"'",TargetItems = "*",FontColor = "Green",Context = "Employee_Evaluations_ListView")]
    public class Evaluation :MigrationBaseObject{
        public virtual Employee Manager{ get; set; }
        public virtual DateTime CreatedOn{ get; set; }
        public virtual Employee Employee{ get; set; }
        // [EditorAlias(EditorAliases.CustomStringPropertyEditor)]
        [Appearance(nameof(Subject), Criteria = "1=1")]
        [FontSizeDelta(8)]
        public virtual string Subject{ get; set; }
        public virtual string Details{ get; set; }
        public virtual EvaluationRating Rating{ get; set; }
        
        public Image Bonus => ImageLoader.Instance.GetImageInfo("Delete").Image;
        public Image Raise => ImageLoader.Instance.GetImageInfo("Delete").Image;
    }
    
    public enum EvaluationRating {
        Unset, Good, Average, Poor
    }

}