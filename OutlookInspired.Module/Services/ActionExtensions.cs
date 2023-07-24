using System.Collections;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using Humanizer;

namespace OutlookInspired.Module.Services{
    public static class ActionExtensions{
        public static IAsyncEnumerable<T> Trigger<T>(this SingleChoiceAction action, IAsyncEnumerable<T> afterNavigation,params object[] selection)
            => afterNavigation.Trigger(() => action.DoExecute(action.SelectedItem, selection));
        
        private static IAsyncEnumerable<T> Trigger<T>(this IAsyncEnumerable<T> afterNavigation,Action action)
            => throw new NotImplementedException();
            // => afterNavigation.Buffer(Unit.Default.ToAsyncEnumerable()
                // .Delay(1.Seconds()).ObserveOn(SynchronizationContext.Current)
                // .Do(_ => action())).Select(list => list).SelectMany();
        
        public static void DoExecute(this SingleChoiceAction action,ChoiceActionItem selectedItem, params object[] objectSelection) 
            => action.DoExecute( () => action.DoExecute(selectedItem), objectSelection);

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
        }


    }
}