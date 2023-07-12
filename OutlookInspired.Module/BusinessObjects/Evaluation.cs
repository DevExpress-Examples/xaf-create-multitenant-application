namespace OutlookInspired.Module.BusinessObjects{
    public class Evaluation :MyBaseObject{
        public virtual Employee CreatedBy{ get; set; }
        public virtual long? CreatedById{ get; set; }
        public virtual DateTime CreatedOn{ get; set; }
        public virtual Employee Employee{ get; set; }
        public virtual long? EmployeeId{ get; set; }
        public virtual string Subject{ get; set; }
        public virtual string Details{ get; set; }
        public virtual EvaluationRating Rating{ get; set; }
    }
    
    public enum EvaluationRating {
        Unset, Good, Average, Poor
    }

}