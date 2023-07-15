

namespace OutlookInspired.Module.BusinessObjects{
    public class Evaluation :MigrationBaseObject{
        public virtual Employee CreatedBy{ get; set; }
        
        public virtual DateTime CreatedOn{ get; set; }
        public virtual Employee Employee{ get; set; }
        
        public virtual string Subject{ get; set; }
        public virtual string Details{ get; set; }
        public virtual EvaluationRating Rating{ get; set; }
    }
    
    public enum EvaluationRating {
        Unset, Good, Average, Poor
    }

}