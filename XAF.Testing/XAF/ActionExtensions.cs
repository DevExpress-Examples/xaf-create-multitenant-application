using System.Collections;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using XAF.Testing.RX;
using View = DevExpress.ExpressApp.View;

namespace XAF.Testing.XAF{
    public static class ActionExtensions{
        public static T View<T>(this ActionBase actionBase) where T : View => actionBase.Controller.Frame?.View as T;
        public static View View(this ActionBase actionBase) => actionBase.View<View>();
        public static IObservable<ItemsChangedEventArgs> WhenItemsChanged(this SingleChoiceAction action) 
            => action.WhenEvent<ItemsChangedEventArgs>(nameof(SingleChoiceAction.ItemsChanged)).TakeUntil(action.WhenDisposed());

        public static Frame Frame(this ActionBase action) => action.Controller?.Frame;
        public static IEnumerable<ChoiceActionItem> Items<T>(this SingleChoiceAction action)
            => action.Items.Where(item => item.Data is T);
        public static IObservable<SimpleActionExecuteEventArgs> WhenExecuteCompleted(this SimpleAction action) 
            => action.WhenEvent<ActionBaseEventArgs>(nameof(ActionBase.ExecuteCompleted)).Cast<SimpleActionExecuteEventArgs>().TakeUntilDisposed(action);
        public static IObservable<SimpleActionExecuteEventArgs> WhenExecuted(this SimpleAction action) 
            => action.WhenEvent<ActionBaseEventArgs>(nameof(SimpleAction.Executed)).Cast<SimpleActionExecuteEventArgs>().TakeUntilDisposed(action);
        public static IObservable<T> Trigger<T>(this SimpleAction action, IObservable<T> afterExecuted,params object[] selection)
            => afterExecuted.Trigger(() => action.DoExecute(selection));
        public static IObservable<Unit> Trigger(this SimpleAction action, params object[] selection)
            => action.Trigger(action.WhenExecuteCompleted().ToUnit(),selection);
        
        public static void DoExecute(this SimpleAction action, params object[] selection) 
            => action.DoExecute(() => action.DoExecute(),selection);
        
        public static IObservable<T> Trigger<T>(this SingleChoiceAction action, IObservable<T> afterExecuted,params object[] selection)
            => action.Trigger(afterExecuted,() => action.SelectedItem,selection);
        
        public static IObservable<T> Trigger<T>(this SingleChoiceAction action, IObservable<T> afterExecuted,Func<ChoiceActionItem> selectedItem,params object[] selection)
            => afterExecuted.Trigger(() => action.DoExecute(selectedItem(), selection));
        
        public static void DoExecute(this SingleChoiceAction action,ChoiceActionItem selectedItem, params object[] objectSelection) 
            => action.DoExecute( () => action.DoExecute(selectedItem), objectSelection);
        public static bool Available(this ActionBase actionBase) 
            => actionBase.Active && actionBase.Enabled;
        public static void DoExecute(this ActionBase action, Action execute, object[] objectSelection){
            if (objectSelection.Any()) {
                var context = action.SelectionContext;
                action.SelectionContext = new SelectionContext(objectSelection.Single());
                execute();
                action.SelectionContext = context;
            }
            else {
                execute();
            }
        }
        
        private static IObservable<T> Trigger<T>(this IObservable<T> afterExecuted, Action action)
            => afterExecuted.Merge(Observable.Defer(() => {
                action();
                return Observable.Empty<T>();
            }),new SynchronizationContextScheduler(SynchronizationContext.Current!));
        
    }
    
    sealed class SelectionContext:ISelectionContext {
        public SelectionContext(object currentObject) {
            CurrentObject = currentObject;
            SelectedObjects = new List<object>(){currentObject};
            OnCurrentObjectChanged();
            OnSelectionChanged();
        }
        public object CurrentObject { get; set; }
        public IList SelectedObjects { get; set; }
        public SelectionType SelectionType => SelectionType.MultipleSelection;
        public string Name => null;
        public bool IsRoot => false;
        public event EventHandler CurrentObjectChanged;
        public event EventHandler SelectionChanged;
        public event EventHandler SelectionTypeChanged;
            
        private void OnSelectionChanged() => SelectionChanged?.Invoke(this, EventArgs.Empty);
        private void OnCurrentObjectChanged() => CurrentObjectChanged?.Invoke(this, EventArgs.Empty);

        public void OnSelectionTypeChanged() => SelectionTypeChanged?.Invoke(this, EventArgs.Empty);
    }

}