

using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.Attributes;

namespace OutlookInspired.Module.BusinessObjects{
    [Appearance(nameof(CreatedOn),AppearanceItemType.ViewItem, "1=1",TargetItems = nameof(CreatedOn),FontStyle = FontStyle.Bold,Context = "Employee_Evaluations_ListView")]
    [Appearance(nameof(Manager),AppearanceItemType.ViewItem, "1=1",TargetItems = nameof(Manager),FontStyle = FontStyle.Bold,Context = "Employee_Evaluations_ListView_Child")]
    [Appearance(nameof(CreatedOn)+"_Employee_Evaluations_ListView_Child",AppearanceItemType.ViewItem, "1=1",TargetItems = nameof(CreatedOn),FontColor = "Blue",Context = "Employee_Evaluations_ListView_Child")]
    [Appearance(nameof(Rating),AppearanceItemType.ViewItem, nameof(Rating)+"='"+nameof(EvaluationRating.Good)+"'",TargetItems = "*",FontColor = "Green",Context = "Employee_Evaluations_ListView")]
    public class Evaluation :MigrationBaseObject{
        public virtual Employee Manager{ get; set; }
        [Browsable(false)]
        public virtual Guid? ManagerId{ get; set; }
        public virtual DateTime CreatedOn{ get; set; }
        public virtual Employee Employee{ get; set; }
        [FontSizeDelta(8)]
        public virtual string Subject{ get; set; }
        public virtual string Details{ get; set; }
        public virtual EvaluationRating Rating{ get; set; }
        [VisibleInListView(false)]
        public virtual Raise Raise=>Details.Contains("Raise: Yes")?Raise.Yes : Raise.No;
        [VisibleInListView(false)]
        public virtual Bonus Bonus=>Details.Contains("Bonus: Yes")?Bonus.Yes : Bonus.No;
        
        
    }

    public enum Raise{
        [XafDisplayName("RAISE")]
        [ImageName("EvaluationNo")]
        No,
        [XafDisplayName("RAISE")]
        [ImageName("EvaluationYes")]
        Yes
    }
    public enum Bonus{
        [XafDisplayName("BONUS")]
        [ImageName("EvaluationNo")]
        No,
        [XafDisplayName("BONUS")]
        [ImageName("EvaluationYes")]
        Yes
    }

    public enum EvaluationRating {
        Unset, Good, Average, Poor
    }

}