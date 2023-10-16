using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using XAF.Testing.RX;

namespace XAF.Testing.XAF{
    public static class AssertExtensions{
        public static IObservable<ITabControlProvider> AssertTabbedGroup(this XafApplication application,
            Type objectType = null, int tabPagesCount = 0,Func<DetailView,bool> match=null,[CallerMemberName]string caller="")
            => application.AssertTabControl<ITabControlProvider>(objectType,match)
                .If(group => tabPagesCount > 0 && group.TabPages != tabPagesCount,group => group.Observe().DelayOnContext()
                        .SelectMany(_ => new AssertException(
                            $"{nameof(AssertTabbedGroup)} {objectType?.Name} expected {tabPagesCount} but was {group.TabPages}").Throw<ITabControlProvider>(caller)),
                    group => group.Observe());
        
        public static IObservable<TTabbedControl> AssertTabControl<TTabbedControl>(this XafApplication application,Type objectType=null,Func<DetailView,bool> match=null) 
            => application.WhenDetailViewCreated(objectType).ToDetailView()
                .Where(view => match?.Invoke(view)??true)
                .SelectMany(detailView => detailView.WhenTabControl()).Cast<TTabbedControl>()
                .Assert(objectType?.Name);
        
        public static IObservable<Frame> AssertNestedListView(this IObservable<ITabControlProvider> source, Frame frame, Type objectType, int selectedTabPageIndex,
            Func<Frame, IObservable<Unit>> existingObjectDetailview = null, Func<Frame,AssertAction> assert=null,bool inlineEdit=false,[CallerMemberName]string caller="")
            => source.AssertNestedListView(frame, objectType, group => group.SelectTab(selectedTabPageIndex),existingObjectDetailview,assert,inlineEdit,caller);
        
        public static IObservable<Frame> AssertNestedListView(this IObservable<ITabControlProvider> source, Frame frame, Type objectType, Action<ITabControlProvider> tabGroupAction,
            Func<Frame, IObservable<Unit>> existingObjectDetailview = null, Func<Frame,AssertAction> assert = null,bool inlineEdit=false,[CallerMemberName]string caller=""){
            return frame.AssertNestedListView(objectType, existingObjectDetailview, assert, inlineEdit, caller)
                .Merge(source.DelayOnContext().Do(tabGroupAction).DelayOnContext().IgnoreElements().To<Frame>());
        }

        public static void ClearFilter(this Frame frame){
            if (frame.View is not ListView listView) return;
            frame.Application.GetRequiredService<IFilterClearer>().Clear(listView);
        }
        
        public static IObservable<Unit> AssertPdfViewer(this IObservable<DashboardViewItem> source){
            // return source.SelectMany(item =>
            //         item.InnerView.ToDetailView().WhenViewItemWinControl<PropertyEditor>(typeof(PdfViewer)))
            //     .SelectMany(t => t.item.WhenEvent(nameof(PropertyEditor.ValueRead))
            //         .WhenNotDefault(_ => ((PdfViewer)t.control).PageCount))
            //     .ToUnit().Assert();
            throw new NotImplementedException();
        }
        public static IObservable<Unit> AssertPdfViewer(this DetailView detailView) 
            => detailView.ObjectSpace.GetRequiredService<IPdfViewerAsserter>().Assert(detailView);

        public static IObservable<Unit> AssertRichEditControl(this DetailView detailView, bool assertMailMerge = false) 
            => detailView.ObjectSpace.GetRequiredService<IRichEditControlAsserter>().Assert(detailView, assertMailMerge);

        public static IObservable<object> AssertObjectsCount(this View view, int objectsCount) 
            => view.ObjectSpace.GetRequiredService<IObjectCountAsserter>().AssertObjectsCount(view, objectsCount);


        public static IObservable<Frame> AssertDashboardViewShowInDocumentAction(this IObservable<Frame> source,Func<SingleChoiceAction,int> itemsCount) 
            => source.DashboardViewItem(item => item.MasterViewItem()).ToFrame()
                .AssertSingleChoiceAction("ShowInDocument",itemsCount)
                .SelectMany(action => action.Items.ToNowObservable()
                    .SelectManySequential(item => {
                        // var assertDashboardViewShowInDocumentAction = action.Frame().GetController<RichTextServiceController>()
                        //     .WhenEvent<CustomRichTextRibbonFormEventArgs>(nameof(RichTextServiceController
                        //         .CustomCreateRichTextRibbonForm)).Take(1)
                        //     .Do(e => e.RichTextRibbonForm.WindowState = FormWindowState.Maximized)
                        //     .SelectMany(e => e.RichTextRibbonForm.RichEditControl.Observe()
                        //         .AssertRichEditControl(
                        //             caller: $"{nameof(AssertDashboardViewShowInDocumentAction)} {item}")
                        //         .Buffer(e.RichTextRibbonForm.WhenEvent(nameof(e.RichTextRibbonForm.Activated))
                        //             .DelayOnContext()).SelectMany().Take(1)
                        //         .Do(richEditControl => richEditControl.FindForm()?.Close())
                        //         .DelayOnContext()).Take(1).To<Frame>();
                        return action.Trigger(AssertDashboardViewShowInDocumentAction(action, item), () => item);
                    }))
                .IgnoreElements().Concat(source).ReplayFirstTake();

        public static IObservable<Frame> AssertDashboardViewShowInDocumentAction(this SingleChoiceAction action, ChoiceActionItem item) 
            => action.Application.GetRequiredService<IDashboardDocumentActionAsserter>().AssertDashboardViewShowInDocumentAction(action,item);

        public static IObservable<ITabControlProvider> WhenDashboardViewTabbedGroup(this XafApplication application, string viewVariant,Type objectType,int tabPagesCount = 0,[CallerMemberName]string caller="") 
            => application.WhenDashboardViewCreated().When(viewVariant)
                .Select(_ => application.AssertTabbedGroup(objectType,tabPagesCount,caller:caller)).Switch();

        public static IObservable<DashboardViewItem> AssertDashboardViewItems(this Frame frame,ViewType viewType,params Type[] objectTypes) 
            => frame.AssertDashboardViewItems(viewType,_ => true,objectTypes);
        public static IObservable<DashboardViewItem> AssertDashboardViewItems(this Frame frame,ViewType viewType,Func<DashboardViewItem,bool> match,params Type[] objectTypes) 
            => frame.DashboardViewItems(viewType,objectTypes).Where(match).ToNowObservable()
                .Assert(item => $"{viewType} {item?.View} {item?.InnerView}");
        
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
                .DelayOnContext()
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
        public static IObservable<Frame> AssertSelectDashboardListViewObject(this IObservable<Frame> source, Func<DashboardViewItem, bool> itemSelector = null)
            => source.SelectMany(frame => frame.Observe().SelectDashboardListViewObject(itemSelector).Assert()).IgnoreElements().To<Frame>().Concat(source).ReplayFirstTake();

        public static IObservable<Frame> AssertDashboardListViewEditView(this IObservable<Frame> source, Func<Frame,IObservable<Frame>> detailView=null,Func<DashboardViewItem, bool> itemSelector = null)
            => source.AssertSelectDashboardListViewObject(itemSelector).AssertDashboardListViewEditViewHasObject(detailView).IgnoreElements()
                .Concat(source).ReplayFirstTake();

        private static IObservable<Frame> AssertDashboardDetailView(this IObservable<Frame> source,Func<DashboardViewItem,bool> masterItem){
            var detailViewItem = source.SelectMany(frame => frame.AssertDashboardViewItems(ViewType.DetailView, item => !item.MasterViewItem(masterItem)))
                .ReplayFirstTake();
            var detailViewDoesNotDisplayData = detailViewItem.AssertDetailViewNotHaveObject();
            return source.Cast<Window>().ConcatIgnored(_ => detailViewDoesNotDisplayData)
                .AssertSelectDashboardListViewObject(item => item.MasterViewItem(masterItem))
                .SelectMany(_ => detailViewItem).AssertDetailViewHasObject().IgnoreElements().To<Frame>()
                .Concat(source);
        }
        
        public static IObservable<DetailView> AssertDetailViewNotHaveObject(this IObservable<DashboardViewItem> source)
            =>  source.AsView<DetailView>().WhenDefault(detailView => detailView.CurrentObject)
                .Assert(view => $"{view}");
        public static IObservable<DetailView> AssertDetailViewNotHaveObject(this IObservable<Frame> source)
            =>  source.ToDetailView().WhenDefault(detailView => detailView.CurrentObject)
                .Assert(view => $"{view}");
        
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
            => source.If(t => t.needsDelete, 
                t => application.CreateObjectSpace(t.typeInfo.Type).Use(space => {
                    space.Delete(space.GetObjectByKey(t.typeInfo.Type, t.keyValue));
                    space.CommitChanges();
                    return t.Observe();
                }),t => t.Observe());

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
                    .SelectManyRecursive(item => item.Items.Available()).ToNowObservable()
                    .Where(item => itemSelector?.Invoke(action, item) ?? true)
                    .SelectManySequential(item => action.Trigger(action.Controller.Frame.Application.AssertReport(action, item), () => item)))
                .IgnoreElements().To<SingleChoiceAction>().Concat(source);

        public static IObservable<Unit> AssertReport(this XafApplication application, SingleChoiceAction action, ChoiceActionItem item) 
            => application.GetRequiredService<IReportAsserter>().AssertReport(action.Controller.Frame, item.ToString());

        public static IObservable<Frame> AssertReports(this XafApplication application, string navigationView, string viewVariant,int reportsCount){
            // return application.AssertListView(navigationView, viewVariant, assert: _ => AssertAction.HasObject)
                // .SelectMany(frame => frame.View.ToListView().WhenObjects().Cast<ReportDataV2>()
                    // .Buffer(reportsCount).Assert($"{nameof(AssertReports)} {reportsCount}").SelectMany()
                    // .SelectManySequential(reportDataV2 => frame.Observe().AssertSelectListViewObject(reportDataV2)
                        // .SelectMany(_ =>
                            // frame.Action<SimpleAction>("ExecuteReportV2")
                                // .Trigger(frame.AssertReport(reportDataV2.ToString())))));
                                throw new NotImplementedException();
        }
        
        public static IObservable<DashboardViewItem> AssertMasterFrame(this IObservable<Frame> source,Func<DashboardViewItem, bool> masterItem=null) 
            => source.DashboardViewItem( masterItem).Select(item => item)
                .MergeIgnored(item => item.Frame.WhenDisposedFrame().Select(unit => unit))
                .Assert(item => $"{item?.Id}")
                .ReplayFirstTake();

        public static IObservable<Window> AssertNavigation(this XafApplication application, string viewId)
            => application.Navigate2(viewId).Assert($"{viewId}");

        public static IObservable<Frame> AssertDashboardListViewEditViewHasObject(this IObservable<Frame> source,Func<Frame,IObservable<Frame>> detailView=null)
            =>source.SelectMany(frame => frame.DashboardViewItems<ListView>().ToNowObservable()
                    .SelectMany(view => view.EditView.AssertDetailViewHasObject().IgnoreElements().To<Frame>()
                        .Concat(detailView?.Invoke(view.EditFrame)?? Observable.Empty<Frame>()))).IgnoreElements().To<Frame>().Concat(source)
                .ReplayFirstTake();
        
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

        public static IObservable<(Frame frame, object o)> AssertListViewHasObjects(this IObservable<Frame> source,[CallerMemberName]string caller="")
            => source.AssertObjectViewHasObjects(caller);
            
        public static IObservable<(Frame frame, object o)> AssertObjectViewHasObjects(this IObservable<Frame> source,[CallerMemberName]string caller="")
            => source.SelectMany(frame => frame.Observe().WhenObjects(1).Take(1).Select(t => (msg:$"{t.frame.View.Id} {t.o}", t)).Assert(t => t.msg,caller:caller)).ToSecond();

        public static IObservable<Frame> AssertNestedListView(this Frame frame, Type objectType,
            Func<Frame, IObservable<Unit>> existingObjectDetailview = null, Func<Frame,AssertAction> assert = null,bool inlineEdit=false, [CallerMemberName] string caller = "") 
            => frame.NestedListViews(objectType)
                .Assert($"{nameof(AssertNestedListView)} {objectType.Name}",caller:caller)
                .Select(editor => (editor.Frame,frame,editor.MemberInfo.IsAggregated))
                .AssertListView(existingObjectDetailview, assert,inlineEdit,caller:caller);


        public static IObservable<(Frame frame, object o)> AssertListViewHasObjects(this XafApplication application,Type objectType) 
            => application.WhenFrame(objectType,ViewType.ListView).AssertListViewHasObjects()
                .Assert($"{nameof(AssertListViewHasObjects)} {objectType.Name}");

        private static IObservable<(Frame frame, object o)> AssertListViewHasObjects(
            this IObservable<(Frame frame, Frame parent, bool aggregated)> source, Func<Frame, AssertAction> assert, [CallerMemberName] string caller = "") 
            => source.ToFirst().If(frame => frame.Assert(assert).HasFlag(AssertAction.HasObject),
                frame => frame.Observe().AssertListViewHasObjects(caller)).ReplayFirstTake();
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
                .AssertCreateNewObject(assert, inlineEdit).AssertSaveNewObject(assert).AssertDeleteObject(assert,inlineEdit);

        private static IObservable<(Frame frame, Frame parent, bool isAggregated)> AssertCreateNewObject(this IObservable<(Frame frame, Frame parent, bool aggregated)> source, 
            Func<Frame,AssertAction> assert, bool inlineEdit) 
            => source.If(t => t.frame.Assert(assert).HasFlag(AssertAction.DetailViewNew) ,t => t.Observe().AssertCreateNewObject(inlineEdit));
        
        private static IObservable<Unit> AssertDeleteObject(this IObservable<(Frame frame, Frame parent, bool isAggregated)> source, Func<Frame, AssertAction> assert,
            bool inlineEdit, [CallerMemberName] string caller = "") 
            => source.If(t => t.frame.Assert(assert).HasFlag(AssertAction.DetailViewDelete),
                t => t.Observe().AssertDeleteObject(caller).ToUnit(),t => t.frame.Observe().WhenDefault(_ => inlineEdit).CloseWindow().ToUnit());

        private static IObservable<(Frame frame, Frame parent, bool isAggregated)> AssertSaveNewObject(
            this IObservable<(Frame frame, Frame parent, bool isAggregated)> source, Func<Frame,AssertAction> assert) 
            => source.If(t => t.frame.Assert(assert).HasFlag(AssertAction.DetailViewSave),
                t => t.Observe().AssertSaveNewObject().Select(t1 => (t1.frame, t1.parent, t1.isAggregated)),t => t.frame.Observe().CloseWindow().To(t));

        public static IObservable<Unit> AssertMapsControl(this DetailView detailView)
            => detailView.ObjectSpace.GetRequiredService<IMapControlAsserter>().AssertMapControl(detailView);
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
        DetailViewNew = (1 << 2) ,
        DetailViewSave = (1 << 3) | DetailViewNew,
        DetailViewDelete = (1 << 4) | DetailViewSave,
        All = DetailViewDelete | DetailViewNew | DetailViewSave | Process  | HasObject,
        AllButDelete=All^DetailViewDelete
    }
}