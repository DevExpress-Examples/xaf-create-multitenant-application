using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Office.Win;
using DevExpress.ExpressApp.ReportsV2.Win;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraMap;
using DevExpress.XtraPdfViewer;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit;
using XAF.Testing.RX;
using ListView = DevExpress.ExpressApp.ListView;
using TabbedGroup = DevExpress.XtraLayout.TabbedGroup;
using View = DevExpress.ExpressApp.View;

namespace XAF.Testing.XAF{
    public static class AssertExtensions{
        public static IObservable<Frame> AssertFrame(this XafApplication application, params ViewType[] viewTypes) 
            => application.AssertFrame(typeof(object),viewTypes);
        public static IObservable<Frame> AssertFrame(this XafApplication application,Type objectType, params ViewType[] viewTypes) 
            => application.WhenFrame(objectType,viewTypes).Assert($"{nameof(AssertFrame)} {string.Join(", ",viewTypes)}");
        
        public static IObservable<Frame> AssertChangeViewVariant(this IObservable<Frame> source,string variantId) 
            => source.If(_ => variantId!=null,frame => frame.Observe().ChangeViewVariant(variantId),frame => frame.Observe()).Assert($"{variantId}")
                .ReplayFirstTake();
        
        public static IObservable<(Frame listViewFrame, Frame detailViewFrame)> AssertProcessSelectedObject(this IObservable<Frame> source)
            => source.SelectMany(window => window.AssertProcessSelectedObject()).TakeAndReplay(1).RefCount();
        public static IObservable<(Frame listViewFrame, Frame detailViewFrame)> AssertProcessSelectedObject(this Frame frame) 
            => frame.Observe().SelectMany(frame1 => frame1.ProcessSelectedObject().Assert($"{frame.View.Id}"));
        
        public static IObservable<Frame> AssertCreateNewObject(this Frame window,bool inLine=false)
            => window.CreateNewObject(inLine).Select(frame => (frame.View.Id,t: frame)).Assert(t => t.Id).ToSecond();

        private static IObservable<(ITypeInfo typeInfo, object keyValue, bool needsDelete, Frame source)> DeleteWhenNeeded(
                this IObservable<(ITypeInfo typeInfo, object keyValue, bool needsDelete, Frame source)> source, XafApplication application) 
            => source.If(t => t.needsDelete, t => application.CreateObjectSpace(t.typeInfo.Type)
                .Use(space => {
                    space.Delete(space.GetObjectByKey(t.typeInfo.Type, t.keyValue));
                    space.CommitChanges();
                    return t.Observe();
                }),t => t.Observe());

        public static IObservable<T> DelayOnContext<T>(this IObservable<T> source,int seconds=1,bool delayOnEmpty=false) 
            => source.DelayOnContext(seconds.Seconds(),delayOnEmpty);
        public static IObservable<T> DelayOnContext<T>(this IObservable<T> source,TimeSpan? timeSpan,bool delayOnEmpty=false) 
            => source.If(_ => timeSpan.HasValue,arg => arg.DelayOnContext( (TimeSpan)timeSpan!),arg => arg.Observe())
                .SwitchIfEmpty(timeSpan.Observe().Where(_ => delayOnEmpty).WhenNotDefault().SelectMany(span => span.DelayOnContext((TimeSpan)span!)
                    .Select(_ => default(T)).IgnoreElements()));

        private static IObservable<T> DelayOnContext<T>(this T arg,TimeSpan timeSpan) 
            => arg.Observe()
            //     .SelectManySequential( arg1 => {
            //     return Observable.Return(arg1).Delay(timeSpan).ObserveOnContext();
            // })
                .Delay(timeSpan, new SynchronizationContextScheduler(SynchronizationContext.Current!))
            ;

        public static IObservable<T> DelayOnContext<T>(this IObservable<T> source,TimeSpan timeSpan) 
            => source.DelayOnContext((TimeSpan?)timeSpan);

        private static IObservable<(ITypeInfo typeInfo, object keyValue, bool needsDelete, Frame frame, Frame parent, bool isAggregated)> AssertSaveNewObject(
                this IObservable<(Frame frame, Frame parent, bool isAggregated)> source)
            => source.SelectMany(t => t.Observe().WhenSaveObject().DeleteWhenNeeded(t.frame.Application).Select(t1 => (t.frame.View.Id, t: t1))
                .Assert(t1 => t1.Id).Select(t1 => (t1.t.typeInfo,t1.t.keyValue,t1.t.needsDelete,t.frame,t.parent,t.isAggregated)));
        
        public static IObservable<(ITypeInfo typeInfo, object keyValue, bool needsDelete, Frame frame, Frame parent,bool isAggregated)> AssertSaveNewObject(this IObservable<Frame> source)
            => source.Select(frame => (frame,default(Frame),false)).AssertSaveNewObject();

        static IObservable<(Frame frame, Frame parent,bool isAggregated)> AssertDeleteObject(this IObservable<(Frame frame, Frame parent,bool isAggregated)> source,[CallerMemberName]string caller="")
            => source.WhenDeleteObject().SelectMany(t => t.application.CreateObjectSpace(t.type)
                    .Use(space => space.GetObjectByKey(t.type,t.keyValue).Observe().WhenDefault()))
                .Assert(caller).To<(Frame frame, Frame parent,bool)>();
        
        public static IObservable<Frame> AssertDeleteObject(this IObservable<Frame> source)
            => source.Select(frame => (frame,default(Frame),false)).AssertDeleteObject().ToFirst();

        public static IObservable<Frame> AssertCreateNewObject(this IObservable<Frame> source,bool inLineEdit=false)
            => source.SelectMany(window => window.AssertCreateNewObject(inLineEdit));
        public static IObservable<(Frame frame, Frame parent,bool isAggregated)> AssertCreateNewObject(this IObservable<(Frame frame, Frame parent,bool isAggregated)> source,bool inLineEdit=false)
            => source.SelectMany(t => t.frame.AssertCreateNewObject(inLineEdit).Select(frame => (frame,t.parent,t.isAggregated)));
        
        public static IObservable<Frame> AssertExistingObjectDetailView(this XafApplication application,Func<Frame,IObservable<Unit>> assertDetailview,Type objectType=null)
            => application.WhenExistingObjectRootDetailViewFrame(objectType)
                .Assert(viewId => $"{nameof(AssertExistingObjectDetailView)} {objectType?.Name} {viewId}")
                .ConcatIgnored(assertDetailview);

        static IObservable<SingleChoiceAction> AssertSingleChoiceActionItems(
            this SingleChoiceAction action, ChoiceActionItem[] source, int itemsCount,Func<ChoiceActionItem,int> nestedItemsCountSelector=null,[CallerMemberName]string caller="") 
            => source.Active().ToArray().Observe().Where(items => items.Length==itemsCount||itemsCount==-1)
                .Assert($"{action.Id} has {source.Active().ToArray().Length} items but should have {itemsCount}",caller:caller)
                .IgnoreElements().SelectMany()
                .Concat(source.Active().ToArray().ToNowObservable())
                .SelectMany(item => {
                    var count = nestedItemsCountSelector?.Invoke(item) ?? -1;
                    return count > -1 ? action.AssertSingleChoiceActionItems(item.Items.Active().ToArray(), count) : Observable.Empty<SingleChoiceAction>();
                })
                .IgnoreElements();

        public static IObservable<SimpleAction> AssertSimpleAction(this IObservable<Frame> source, string actionId,Func<SimpleAction,bool> completeWhenNotAvailable=null,[CallerMemberName]string caller="")
            => source.SelectMany(frame => frame.AssertSimpleAction(actionId,completeWhenNotAvailable, caller));

        public static IObservable<SimpleAction> AssertSimpleAction(this Frame frame,string actionId,Func<SimpleAction,bool> completeWhenNotAvailable=null,[CallerMemberName]string caller="") 
            => frame.Actions<SimpleAction>(actionId).ToNowObservable()
                .SelectMany(action => !action.Available() && (completeWhenNotAvailable?.Invoke(action)??false) ? Observable.Empty<SimpleAction>()
                    : action.Observe().Assert($"{nameof(AssertSimpleAction)} {frame.View} {actionId}", caller: caller));

        public static IObservable<SimpleAction> AssertSimpleAction(this IObservable<DashboardView> source,string actionId,Func<SimpleAction,bool> completeWhenNotAvailable=null)
            => source.SelectMany(view => view.DashboardViewItems().Where(item => item.MasterViewItem()).ToNowObservable()
                    .SelectMany(item => item.Frame.AssertSimpleAction(actionId,completeWhenNotAvailable,item.ToString())))
                .ReplayFirstTake();

        public static IObservable<SingleChoiceAction> AssertNavigationItems(this XafApplication application,
            Func<SingleChoiceAction, ChoiceActionItem, int> itemsCount)
            => application.WhenWindowCreated(true).AssertSingleChoiceAction("ShowNavigationItem",itemsCount);
        
        public static IObservable<SingleChoiceAction> AssertSingleChoiceAction(this IObservable<Frame> source,
            string actionId, Func<SingleChoiceAction,int> itemsCount = null) 
            => source.AssertSingleChoiceAction(actionId,(action, item) => item==null? itemsCount?.Invoke(action) ?? -1:-1);
        
        public static IObservable<SingleChoiceAction> AssertSingleChoiceAction(this IObservable<Frame> source,
            string actionId, Func<SingleChoiceAction,ChoiceActionItem, int> itemsCount = null) 
            => source.SelectMany(frame => frame.Actions<SingleChoiceAction>(actionId)
                    .Where(action => action.Available() || itemsCount != null && itemsCount(action, null) == -1).ToNowObservable()
                    .Assert($"{nameof(AssertSingleChoiceAction)} {actionId}")
                .SelectMany(action => action.AssertSingleChoiceActionItems(action.Items.ToArray(),itemsCount?.Invoke(action,null)??-1
                        ,item => itemsCount?.Invoke(action, item)??-1).IgnoreElements().Concat(action.Observe())))
                .ReplayFirstTake();

        public static IObservable<Frame> AssertDashboardViewShowInDocumentAction(this IObservable<Frame> source,Func<SingleChoiceAction,int> itemsCount) 
            => source.DashboardViewItem(item => item.MasterViewItem()).ToFrame()
                .AssertSingleChoiceAction("ShowInDocument",itemsCount)
                .SelectMany(action => action.Items.ToNowObservable()
                    .SelectManySequential(item => action.Trigger(action.Frame().GetController<RichTextServiceController>()
                        .WhenEvent<CustomRichTextRibbonFormEventArgs>(nameof(RichTextServiceController.CustomCreateRichTextRibbonForm)).Take(1)
                        .Do(e => e.RichTextRibbonForm.WindowState=FormWindowState.Maximized)
                        .SelectMany(e => e.RichTextRibbonForm.RichEditControl.Observe().AssertRichEditControl(caller:$"{nameof(AssertDashboardViewShowInDocumentAction)} {item}")
                            .Buffer(e.RichTextRibbonForm.WhenEvent(nameof(e.RichTextRibbonForm.Activated)).DelayOnContext()).SelectMany().Take(1)
                            .Do(richEditControl => richEditControl.FindForm()?.Close())
                            .DelayOnContext()).Take(1).To<Frame>(),() => item)))
                .IgnoreElements().Concat(source).ReplayFirstTake();

        public static IObservable<Frame> AssertDashboardViewReportsAction(this IObservable<Frame> source, string actionId,
            Func<SingleChoiceAction,ChoiceActionItem, bool> itemSelector = null,
            Func<SingleChoiceAction,ChoiceActionItem, int> reportsCount = null)
            => source.DashboardViewItem(item => item.MasterViewItem()).ToFrame()
                .AssertSingleChoiceAction(actionId, reportsCount)
                .AssertReports(itemSelector)
                .IgnoreElements().To<Frame>().Concat(source).ReplayFirstTake();
        
        public static IObservable<Frame> AssertDashboardViewReportsAction(this IObservable<Frame> source, string actionId,
            Func<SingleChoiceAction,ChoiceActionItem, bool> itemSelector = null, Func<SingleChoiceAction,int> reportsCount = null)
            => source.AssertDashboardViewReportsAction(actionId,itemSelector,(action, item) => item!=null?-1:reportsCount?.Invoke(action)??-1);

        public static IObservable<SingleChoiceAction> AssertReports(this IObservable<SingleChoiceAction> source,Func<SingleChoiceAction,ChoiceActionItem,bool> itemSelector=null)
            => source.Where(action => action.Available()).SelectMany(action => action.Items.Available()
                    .SelectManyRecursive(item => item.Items.Available()).ToNowObservable().Where(item => itemSelector?.Invoke(action, item)??true)
                    .SelectManySequential(item => action.Trigger(action.Controller.Frame.AssertReport( item.ToString()), () => item)))
                .IgnoreElements().To<SingleChoiceAction>().Concat(source);

        public static IObservable<Frame> AssertReports(this XafApplication application, string navigationView, string viewVariant,int reportsCount)
            => application.AssertListView(navigationView, viewVariant, assert: _ => AssertAction.HasObject)
                .SelectMany(frame => frame.View.ToListView().WhenObjects().Cast<ReportDataV2>()
                    .Buffer(reportsCount).Assert($"{nameof(AssertReports)} {reportsCount}").SelectMany()
                    .SelectManySequential(reportDataV2 => frame.Observe().AssertSelectListViewObject(reportDataV2)
                        .SelectMany(_ => frame.Action<SimpleAction>("ExecuteReportV2").Trigger(frame.AssertReport(reportDataV2.ToString())))));
        
        public static IObservable<Frame> AssertReport(this Frame frame, [CallerMemberName]string caller="") 
            => frame.GetController<WinReportServiceController>()
                .WhenEvent<WinReportServiceController.CustomizePrintToolEventArgs>(
                    nameof(WinReportServiceController.CustomizePrintTool)).Take(1)
                .SelectMany(e => e.PrintTool.PrintingSystem
                    .WhenEvent(nameof(e.PrintTool.PrintingSystem.CreateDocumentException))
                    .Select(pattern => ((ExceptionEventArgs)pattern.EventArgs).Exception)
                    .Buffer(((XtraReport)e.PrintTool.Report).WhenEvent(nameof(XtraReport.AfterPrint))).Take(1)
                    .Select(exceptions => (exceptions: exceptions.Count, pages: ((XtraReport)e.PrintTool.Report).Pages.Count))
                    .DelayOnContext()
                    .Do(_ => e.PrintTool.PreviewRibbonForm.Close())
                    .WhenDefault(t => t.exceptions)
                    .WhenNotDefault(t => t.pages).Select(_ => default(Frame))
                    .DelayOnContext()).Assert($"{nameof(AssertReport)}",caller:caller);

        public static IObservable<Frame> AssertExistingObjectDetailView(this XafApplication application,Type objectType=null) 
            => application.AssertExistingObjectDetailView(_ => Observable.Empty<Unit>(),objectType);

        public static IObservable<DashboardViewItem> AssertDashboardViewItems(this Frame frame,ViewType viewType,params Type[] objectTypes) 
            => frame.AssertDashboardViewItems(viewType,_ => true,objectTypes);
        public static IObservable<DashboardViewItem> AssertDashboardViewItems(this Frame frame,ViewType viewType,Func<DashboardViewItem,bool> match,params Type[] objectTypes) 
            => frame.DashboardViewItems(viewType,objectTypes).Where(match).ToNowObservable()
                .Assert(item => $"{viewType} {item?.View} {item?.InnerView}");

        public static IObservable<Frame> AssertListView(this XafApplication application,string navigation, string viewVariant=null,Func<Frame, IObservable<Unit>> assertExistingObjectDetailview = null,Func<Frame,AssertAction> assert=null) 
            => application.AssertNavigation(navigation).AssertChangeViewVariant(viewVariant)
                .AssertListView(assertExistingObjectDetailview,assert:assert);

        public static IObservable<Frame> AssertListView(this DashboardViewItem[] items, Type objectType,
            Func<Frame, IObservable<Unit>> assertExistingObjectDetailview = null, Func<Frame,AssertAction> assert=null) 
            => items.ToNowObservable().SelectMany(item => item.InnerView.ToDetailView().NestedListViews(objectType))
                .Select(editor => editor.Frame).ReplayFirstTake()
                .AssertListView(assertExistingObjectDetailview, assert);

        public static IObservable<Frame> AssertListView(this Frame frame, Func<Frame, IObservable<Unit>> assertExistingObjectDetailview = null, Func<Frame,AssertAction> assert=null) 
            => frame.Observe().AssertListView(assertExistingObjectDetailview, assert);

        public static IObservable<Frame> AssertItemsAdded(this IObservable<SingleChoiceAction> source, IObservable<object> when)
            => source.AssertItemsAdded().Merge(when.IgnoreElements().To<Frame>()).Assert();
        
        public static IObservable<Frame> AssertItemsAdded(this IObservable<SingleChoiceAction> source) 
            => source.SelectMany(action => action.WhenItemsChanged().Where(e => e.ChangedItemsInfo.Any(pair => pair.Value==ChoiceActionItemChangesType.ItemsAdd)).To(action.Frame()));

        public static IObservable<Frame> AssertListView(this IObservable<Frame> source, Func<Frame, IObservable<Unit>> assertExistingObjectDetailview = null, Func<Frame,AssertAction> assert=null,bool inlineEdit=false,[CallerMemberName]string caller="") 
            => source.Select(frame => (frame, default(Frame), false)).AssertListView(assertExistingObjectDetailview,assert,inlineEdit,caller);

        private static IObservable<Frame> AssertListView(this IObservable<(Frame frame, Frame parent,bool aggregated)> source,
            Func<Frame, IObservable<Unit>> assertExistingObjectDetailview = null, Func<Frame,AssertAction> assert = null, bool inlineEdit = false,[CallerMemberName]string caller="") 
            => source.AssertListViewHasObjects( assert,caller).SwitchIfEmpty(source.AssertListViewNotHasObjects(assert).Select(frame => (frame,new object())))
                .AssertProcessSelectedObject(assertExistingObjectDetailview, assert)
                .AssertCreateSaveAndDeleteObject( source,assert, inlineEdit)
                .IgnoreElements().To<Frame>()
                .Concat(source.ToFirst())
                .ReplayFirstTake();

        private static IObservable<(Frame frame, object o)> AssertListViewHasObjects(
            this IObservable<(Frame frame, Frame parent, bool aggregated)> source, Func<Frame, AssertAction> assert,
            [CallerMemberName] string caller = "") 
            => source.ToFirst().If(frame => frame.Assert(assert).HasFlag(AssertAction.HasObject), frame => frame.Observe().AssertListViewHasObjects(caller)).ReplayFirstTake();
        static AssertAction Assert(this Frame frame,Func<Frame, AssertAction> assert,AssertAction assertAction=AssertAction.All) 
            => assert?.Invoke(frame)??assertAction;

        private static IObservable<Frame> AssertProcessSelectedObject(this IObservable<(Frame frame, object o)> listViewHasObjects,
            Func<Frame, IObservable<Unit>> assertExistingObjectDetailview, Func<Frame,AssertAction> assert) 
            => listViewHasObjects.ToFirst().If(frame => frame.Assert(assert).HasFlag(AssertAction.Process),
                    frame => frame.AssertProcessSelectedObject().ToSecond()
                        .ConcatIgnored(assertExistingObjectDetailview??(_ =>Observable.Empty<Unit>()) ))
                .CloseWindow()
                .ReplayFirstTake();
        private static IObservable<Frame> AssertListViewNotHasObjects(this IObservable<(Frame frame, Frame parent, bool aggregated)> source, Func<Frame,AssertAction> assert) 
            => source.ToFirst().If(frame => frame.Assert(assert,AssertAction.NotHasObject).HasFlag(AssertAction.NotHasObject),
                    frame => frame.WhenObjects().Assert(timeout: 5.Seconds()).SelectMany(t => new AssertException($"{t.frame.View} has objects").Throw<Frame>())
                        .Catch<Frame, TimeoutException>(_ => frame.Observe()).ObserveOnContext())
                .ReplayFirstTake();

        private static IObservable<Unit> AssertCreateSaveAndDeleteObject(this IObservable<Frame> processSelectedObject,
            IObservable<(Frame frame, Frame parent, bool aggregated)> source, Func<Frame,AssertAction> assert, bool inlineEdit) 
            => source.ConcatIgnored(_ => processSelectedObject)
                .AssertCreateNewObject(assert, inlineEdit).AssertSaveNewObject(assert,inlineEdit).AssertDeleteObject(assert);

        private static IObservable<Unit> AssertDeleteObject(this IObservable<(Frame frame, Frame parent, bool isAggregated)> source,Func<Frame,AssertAction> assert,[CallerMemberName]string caller="") 
            => source.If(t => {
                var assertAction = t.frame.Assert(assert);
                var hasFlag = assertAction.HasFlag(AssertAction.DetailViewDelete);
                return hasFlag;
            },t => t.Observe().AssertDeleteObject(caller).ToUnit(),t => t.frame.Observe().CloseWindow().ToUnit());

        private static IObservable<(Frame frame, Frame parent, bool isAggregated)> AssertSaveNewObject(
            this IObservable<(Frame frame, Frame parent, bool isAggregated)> source, Func<Frame,AssertAction> assert, bool inlineEdit) 
            => source.If(t => {
                    var assertAction = t.frame.Assert(assert);
                    var hasFlag = assertAction.HasFlag(AssertAction.DetailViewSave);
                    return hasFlag || inlineEdit;
                },
                t => t.Observe().AssertSaveNewObject().Select(t1 => (t1.frame, t1.parent, t1.isAggregated)));

        private static IObservable<(Frame frame, Frame parent, bool isAggregated)> AssertCreateNewObject(this IObservable<(Frame frame, Frame parent, bool aggregated)> source, 
            Func<Frame,AssertAction> assert, bool inlineEdit) 
            => source.If(t => {
                var assertAction = t.frame.Assert(assert);
                var hasFlag = assertAction.HasFlag(AssertAction.DetailViewNew);
                return hasFlag || inlineEdit;
            },t => t.Observe().AssertCreateNewObject(inlineEdit));
        
        public static IObservable<Frame> AssertDashboardMasterDetail(this XafApplication application, string navigationView, string viewVariant, 
            Func<Frame, IObservable<Frame>> detailViewFrameSelector = null, Func<DashboardViewItem, bool> listViewFrameSelector = null,
            Func<Frame, IObservable<Unit>> existingObjectDetailview = null,Func<Frame,AssertAction> assert=null) 
            => application.AssertNavigation(navigationView)
                .AssertDashboardMasterDetail(viewVariant, detailViewFrameSelector, listViewFrameSelector, existingObjectDetailview,assert);

        public static IObservable<Frame> AssertDashboardMasterDetail(this IObservable<Window> source,string viewVariant=null, Func<Frame, IObservable<Frame>> detailViewFrameSelector=null,
            Func<DashboardViewItem, bool> listViewFrameSelector=null, Func<Frame, IObservable<Unit>> existingObjectDetailview=null,Func<Frame,AssertAction> assert=null) 
            => source.AssertChangeViewVariant(viewVariant)
                .AssertDashboardDetailView(detailViewFrameSelector, listViewFrameSelector).IgnoreElements()
                .Concat(source.AssertDashboardListView(listViewFrameSelector, existingObjectDetailview,assert)).IgnoreElements()
                .Concat(source)
                .ReplayFirstTake();

        public static IObservable<(Frame frame, Frame source)> AssertDialogControllerListView(this IObservable<SingleChoiceAction> action,Type objectType,Func<Frame,AssertAction> assert=null,bool inlineEdit=false) 
            => action.SelectMany(choiceAction => choiceAction.Trigger(choiceAction.Application.AssertFrame(objectType,ViewType.ListView)
                    .Select(frame => (frame, source: choiceAction.Controller.Frame)), choiceAction.Items.First))
                .SelectMany(t => t.frame.Observe().AssertListView(assert:assert,inlineEdit:inlineEdit).To(t));

        
        private static IObservable<Frame> AssertDashboardListView(this IObservable<Frame> changeViewVariant, Func<DashboardViewItem, bool> listViewFrameSelector,
            Func<Frame, IObservable<Unit>> assertExistingObjectDetailview, Func<Frame,AssertAction> assert = null, [CallerMemberName] string caller = "") 
            => changeViewVariant.AssertMasterFrame(listViewFrameSelector).ToFrame()
                .AssertListView(assertExistingObjectDetailview, assert,caller:caller)
                .ReplayFirstTake();

        public static IObservable<Frame> AssertDashboardListView(this XafApplication application, string navigationId,
            string viewVariant, Func<Frame, IObservable<Unit>> existingObjectDetailview = null,Func<DashboardViewItem, bool> listViewFrameSelector=null, Func<Frame,AssertAction> assert = null)
            => application.AssertNavigation(navigationId).AssertDashboardListView(viewVariant, existingObjectDetailview,listViewFrameSelector,assert );
        
        public static IObservable<Frame> AssertDashboardListView(this IObservable<Frame> source, 
            string viewVariant, Func<Frame, IObservable<Unit>> existingObjectDetailview = null,Func<DashboardViewItem, bool> listViewFrameSelector=null,
            Func<Frame,AssertAction> assert = null)
            => source.AssertChangeViewVariant(viewVariant)
                .AssertDashboardListView(listViewFrameSelector, existingObjectDetailview, assert).IgnoreElements()
                .Concat(source).ReplayFirstTake();
        
        public static IObservable<Frame> AssertDashboardDetailView(this XafApplication application,string navigationId,string viewVariant) 
            => application.AssertNavigation(navigationId).AssertChangeViewVariant(viewVariant).AssertDashboardDetailView(null);

        private static IObservable<Frame> AssertDashboardDetailView(this IObservable<Frame> source, Func<Frame, IObservable<Frame>> detailViewFrameSelector=null,
            Func<DashboardViewItem, bool> listViewFrameSelector=null){
            var detailViewFrame = source.SelectMany(detailViewFrameSelector??(frame =>frame.DashboardDetailViewFrame()) ).TakeAndReplay(1).AutoConnect();
            return detailViewFrame.IsEmpty()
                .If(isEmpty => isEmpty, _ => source, _ => source.Cast<Window>()
                    .AssertSelectDashboardListViewObject(listViewFrameSelector ?? (item => item.MasterViewItem()))
                    .SelectMany(_ => detailViewFrame).AssertDetailViewHasObject().IgnoreElements().To<Frame>()
                    .Concat(source)).ReplayFirstTake();
        }
        private static IObservable<Frame> AssertDashboardDetailView(this IObservable<Frame> source,Func<DashboardViewItem,bool> masterItem){
            var detailViewItem = source.SelectMany(frame => frame.AssertDashboardViewItems(ViewType.DetailView, item => !item.MasterViewItem(masterItem)))
                .ReplayFirstTake();
            var detailViewDoesNotDisplayData = detailViewItem.AssertDetailViewNotHaveObject();
            return source.Cast<Window>().ConcatIgnored(_ => detailViewDoesNotDisplayData)
                .AssertSelectDashboardListViewObject(item => item.MasterViewItem(masterItem))
                .SelectMany(_ => detailViewItem).AssertDetailViewHasObject().IgnoreElements().To<Frame>()
                .Concat(source);
        }

        public static IObservable<object> AssertObjectsCount(this View view, int objectsCount) 
            => view.WhenDataSourceChanged().FirstAsync(o => o is CollectionSourceBase collectionSourceBase
                    ? collectionSourceBase.GetCount() == objectsCount
                    : ((GridControl)o).MainView.DataRowCount == objectsCount)
                .Assert($"{nameof(AssertObjectsCount)} {view.Id}");

        public static IObservable<object> WhenDataSourceChanged(this View view) 
            => view is ListView listView ? listView.CollectionSource.WhenCriteriaApplied()
                : view.ToDetailView().WhenControlViewItemGridControl().SelectMany(control => control.WhenDataSourceChanged().To(control));

        public static IObservable<DashboardViewItem> AssertMasterFrame(this IObservable<Frame> source,Func<DashboardViewItem, bool> masterItem=null) 
            => source.DashboardViewItem( masterItem).Select(item => item)
                .MergeIgnored(item => item.Frame.WhenDisposedFrame().Select(unit => unit))
                .Assert(item => $"{item?.Id}")
                .Select(item => item);

        public static IObservable<Window> AssertNavigation(this XafApplication application, string viewId)
            => application.Navigate(viewId).Assert($"{viewId}");

        public static IObservable<Frame> AssertDashboardListViewEditViewHasObject(this IObservable<Frame> source,Func<Frame,IObservable<Frame>> detailView=null)
            =>source.SelectMany(frame => frame.DashboardViewItems<ListView>().ToNowObservable()
                .SelectMany(view => view.EditView.AssertDetailViewHasObject().IgnoreElements().To<Frame>()
                    .Concat(detailView?.Invoke(view.EditFrame)?? Observable.Empty<Frame>()))).IgnoreElements().To<Frame>().Concat(source)
                .ReplayFirstTake();

        public static IObservable<PdfViewer> AssertPdfViewer(this DetailView detailView) 
            => detailView.WhenPropertyEditorControl().OfType<PdfViewer>()
                .WhenNotDefault(viewer => viewer.PageCount)
                .Assert();


        public static IObservable<RichEditControl> AssertRichEditControl(this IObservable<RichEditControl> source, bool assertMailMerge = false,[CallerMemberName]string caller="")
            => source.SelectMany(control => control.WhenEvent(nameof(control.DocumentLoaded)).To(control))
                .WhenNotDefault(control => control.DocumentLayout.GetPageCount())
                .Assert($"{caller} {nameof(RichEditControl.DocumentLoaded)}")
                .If(_ => assertMailMerge, control => control.WhenEvent(nameof(control.MailMergeFinished)).To(control)
                    .Assert($"{caller} {nameof(RichEditControl.MailMergeFinished)}"),control => control.Observe());
        
        public static IObservable<MapControl> AssertMapsControl(this DetailView detailView)
            => detailView.WhenControlViewItemControl().OfType<MapControl>().WhenNotDefault(control => control.Layers.Count)
                .SelectMany(control => control.Layers.OfType<ImageLayer>().ToNowObservable()
                    .SelectMany(layer => Observable.FromEventPattern(layer, nameof(layer.Error),EventExtensions.ImmediateScheduler)
                        .SelectMany(pattern => ((MapErrorEventArgs)pattern.EventArgs).Exception.Throw<MapControl>())
                        .Merge(Observable.FromEventPattern(layer, nameof(layer.DataLoaded),EventExtensions.ImmediateScheduler).To(control)).Take(1)))
                .Assert();
        
        public static IObservable<RichEditControl> AssertRichEditControl(this DetailView detailView, bool assertMailMerge = false)
            => detailView.WhenPropertyEditorControl().OfType<Control>().SelectControlRecursive().OfType<RichEditControl>()
                .AssertRichEditControl(assertMailMerge);

        public static IObservable<Frame> AssertDashboardListViewEditView(this IObservable<Frame> source, Func<Frame,IObservable<Frame>> detailView=null,Func<DashboardViewItem, bool> itemSelector = null)
            => source.AssertSelectDashboardListViewObject(itemSelector).AssertDashboardListViewEditViewHasObject(detailView).IgnoreElements()
                .Concat(source).ReplayFirstTake();
        
        
        public static IObservable<Frame> AssertSelectDashboardListViewObject(this IObservable<Frame> source, Func<DashboardViewItem, bool> itemSelector = null)
            => source.SelectMany(frame => frame.Observe().SelectDashboardListViewObject(itemSelector).Assert()).IgnoreElements().To<Frame>().Concat(source).ReplayFirstTake();
        public static IObservable<Frame> AssertSelectListViewObject(this IObservable<Frame> source,params object[] objects)
            => source.ToListView().SelectMany(listView => objects.FirstOrDefault() is{ } selectedObject ? listView.SelectObject(selectedObject)
                    : listView.Objects().Take(1).SelectMany(o => listView.SelectObject(o))).Assert().IgnoreElements().To<Frame>()
                .Concat(source);
        public static IObservable<TabbedGroup> WhenDashboardViewTabbedGroup(this XafApplication application, string viewVariant,Type objectType,int tabPagesCount = 0) 
            => application.WhenDashboardViewCreated().When(viewVariant)
                .Select(_ => application.AssertTabbedGroup(objectType,tabPagesCount)).Switch();
        
        public static IObservable<TabbedGroup> AssertTabbedGroup(this XafApplication application,
            Type objectType = null, int tabPagesCount = 0)
            => application.AssertTabControl<TabbedGroup>(objectType)
                .If(group => tabPagesCount > 0 && group.TabPages.Count != tabPagesCount,group => group.Observe().DelayOnContext()
                    .SelectMany(_ => new AssertException(
                        $"{nameof(AssertTabbedGroup)} expected {tabPagesCount} but was {group.TabPages.Count}").Throw<TabbedGroup>()),
                    group => group.Observe());
        
        public static IObservable<TTabbedControl> AssertTabControl<TTabbedControl>(this XafApplication application,Type objectType=null) 
            => application.WhenDetailViewCreated(objectType).ToDetailView()
                .SelectMany(detailView => detailView.WhenTabControl()).Cast<TTabbedControl>()
                .Assert(objectType?.Name);

        public static IObservable<object> AssertWindowHasObjects(this IObservable<Frame> source)
            => source.If(window => window.DashboardViewItems<DetailView>().Any(),window => window.Observe().WhenObjects().ToSecond()
                    .Assert($"{nameof(AssertWindowHasObjects)} {window.View.Id}"),
                window => window.DashboardViewItems<ListView>().ToNowObservable().BufferUntilCompleted()
                    .SelectMany(listViews => listViews.ToNowObservable().SelectMany(listView => listView.WhenObjects().Take(1))
                        .Take(listViews.Length))
                    .Assert($"{nameof(AssertWindowHasObjects)} {window.View.Id}"));

        public static IObservable<(ColumnView columnView, object value)> AssertGridControlDetailViewObjects(
            this IObservable<Frame> source, Func<ColumnView, int> count, [CallerMemberName] string caller = "")
            => source.WhenGridControlDetailViewObjects(count).Assert(o => $"{o} {caller}");
        
        public static IObservable<Frame> AssertDashboardViewGridControlDetailViewObjects(
            this IObservable<Frame> source, params string[] relationNames )
            => source.AssertDashboardViewGridControlDetailViewObjects(view => !relationNames.Contains(view.LevelName)
                ? throw new NotImplementedException(view.LevelName) : 1 );
        
        public static IObservable<Frame> AssertDashboardViewGridControlDetailViewObjects(
            this IObservable<Frame> source, Func<ColumnView, int> count, [CallerMemberName] string caller = "")
            => source.DashboardViewItem(item => item.MasterViewItem()).ToFrame()
                .SelectMany(frame => frame.Observe().AssertGridControlDetailViewObjects(count,caller).IgnoreElements().ToFirst().To<Frame>())
                .Concat(source);

        public static IObservable<DetailView> AssertDetailViewNotHaveObject(this IObservable<DashboardViewItem> source)
            =>  source.AsView<DetailView>().WhenDefault(detailView => detailView.CurrentObject)
                .Assert(view => $"{view}");
        public static IObservable<DetailView> AssertDetailViewNotHaveObject(this IObservable<Frame> source)
            =>  source.ToDetailView().WhenDefault(detailView => detailView.CurrentObject)
                .Assert(view => $"{view}");

        public static IObservable<Unit> AssertDetailViewGridControlHasObjects(this IObservable<DashboardViewItem> source)
            => source.SelectMany(item => item.InnerView.ToDetailView().WhenControlViewItemGridControl().WhenObjects()).ToUnit().Assert();
        
        public static IObservable<Unit> AssertPdfViewer(this IObservable<DashboardViewItem> source)
            => source.SelectMany(item => item.InnerView.ToDetailView().WhenViewItemWinControl<PropertyEditor>(typeof(PdfViewer)))
                .SelectMany(t => t.item.WhenEvent(nameof(PropertyEditor.ValueRead))
                    .WhenNotDefault(_ => ((PdfViewer)t.control).PageCount))
                .ToUnit().Assert();
        
        public static IObservable<Unit> AssertDetailViewHasObject(this IObservable<DashboardViewItem> source) 
            => source.AsView<DetailView>().SelectMany(AssertDetailViewHasObject);

        public static IObservable<Unit> AssertDetailViewHasObject(this DetailView detailView) 
            => detailView.WhenSelectionChanged()
                .Merge(detailView.WhenCurrentObjectChanged()).To(detailView).StartWith(detailView)
                .WhenNotDefault(view => view.CurrentObject)
                .Assert(_ => $"{detailView.Id}").ToUnit();

        public static IObservable<Unit> AssertDetailViewHasObject(this IObservable<Frame> source) 
            => source.ToDetailView()
                .SelectMany(detailView => detailView.WhenSelectionChanged()
                    .Merge(detailView.WhenCurrentObjectChanged()).To(detailView).StartWith(detailView)
                    .WhenNotDefault(view => view.CurrentObject)
                    .Assert(_ => $"{detailView.Id}").ToUnit())
                ;

        public static IObservable<(Frame frame, object o)> AssertListViewHasObjects(this IObservable<Frame> source,[CallerMemberName]string caller="")
            => source.AssertObjectViewHasObjects(caller);
            
        public static IObservable<(Frame frame, object o)> AssertObjectViewHasObjects(this IObservable<Frame> source,[CallerMemberName]string caller="")
            => source.SelectMany(frame => frame.Observe().WhenObjects(1).Take(1).Select(t => (msg:$"{t.frame.View.Id} {t.o}", t)).Assert(t => t.msg,caller:caller)).ToSecond();

        public static IObservable<Unit> AssertNestedListView(this IObservable<TabbedGroup> source, Frame frame, Type objectType, int selectedTabPageIndex,
            Func<Frame, IObservable<Unit>> existingObjectDetailview = null, Func<Frame,AssertAction> assert=null,bool inlineEdit=false,[CallerMemberName]string caller="")
            => source.AssertNestedListView(frame, objectType, group => group.SelectedTabPageIndex=selectedTabPageIndex,existingObjectDetailview,assert,inlineEdit,caller);
        
        public static IObservable<Unit> AssertNestedListView(this IObservable<TabbedGroup> source, Frame frame, Type objectType, Action<TabbedGroup> tabGroupAction,
            Func<Frame, IObservable<Unit>> existingObjectDetailview = null, Func<Frame,AssertAction> assert = null,bool inlineEdit=false,[CallerMemberName]string caller="") 
            => frame.AssertNestedListView(objectType,existingObjectDetailview,assert,inlineEdit,caller)
                .MergeToUnit(source.DelayOnContext().Do(tabGroupAction)
                    .DelayOnContext().IgnoreElements());

        public static IObservable<Frame> AssertNestedListView(this Frame frame, Type objectType,
            Func<Frame, IObservable<Unit>> existingObjectDetailview = null, Func<Frame,AssertAction> assert = null,bool inlineEdit=false, [CallerMemberName] string caller = "") 
            => frame.NestedListViews(objectType)
                .Assert($"{nameof(AssertNestedListView)} {objectType.Name}",caller:caller)
                .Select(editor => (editor.Frame,frame,editor.MemberInfo.IsAggregated))
                .AssertListView(existingObjectDetailview, assert,inlineEdit,caller:caller);


        public static IObservable<(Frame frame, object o)> AssertListViewHasObjects(this XafApplication application,Type objectType) 
            => application.WhenFrame(objectType,ViewType.ListView).AssertListViewHasObjects()
                .Assert($"{nameof(AssertListViewHasObjects)} {objectType.Name}");

    }

    public class AssertException:Exception{
        public AssertException(string message) : base(message){
        }

        public AssertException(string message, Exception innerException) : base(message, innerException){
        }
    }

    [Flags]
    public enum AssertAction{
        None = 0,
        HasObject = 1 << 0,
        NotHasObject = 1 << 5,
        Process = (1 << 1) | HasObject,
        DetailViewNew = (1 << 2) | Process,
        DetailViewSave = (1 << 3) | DetailViewNew,
        DetailViewDelete = (1 << 4) | DetailViewSave,
        All = DetailViewDelete | DetailViewNew | DetailViewSave | Process  | HasObject
    }
}