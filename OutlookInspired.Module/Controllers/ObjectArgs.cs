namespace OutlookInspired.Module.Controllers{
    public interface IMasterDetailService{
        event EventHandler<ObjectArgs> SelectedObjectChanged;
        event EventHandler<ObjectArgs> ProcessObject;
    }
    public class ObjectArgs:EventArgs{
        public object Instance{ get; }

        public ObjectArgs(object instance) => Instance = instance;
    }
}