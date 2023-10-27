using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using Microsoft.Extensions.DependencyInjection;
using XAF.Testing.RX;

namespace XAF.Testing.XAF{
    public static class XafApplicationExtensions{
        public static T GetRequiredService<T>(this XafApplication application) where T : notnull 
            => application.ServiceProvider.GetRequiredService<T>();

        public static IObservable<Frame> WhenFrameCreated(this XafApplication application,TemplateContext templateContext=default)
            => application.WhenEvent<FrameCreatedEventArgs>(nameof(XafApplication.FrameCreated)).Select(e => e.Frame)
                .Where(frame => frame.Application==application&& (templateContext==default ||frame.Context == templateContext));

        public static IObservable<Frame> WhenFrame(this XafApplication application)
            => application.WhenFrameViewChanged().Merge(application.MainWindow.Observe().WhenNotDefault().WhenViewChanged().WhenNotDefault(window => window.View));
        public static IObservable<Frame> WhenFrame(this XafApplication application, Type objectType , params ViewType[] viewTypes) 
            => application.WhenFrame(objectType).WhenFrame(viewTypes);

        public static IObservable<Frame> WhenFrame(this XafApplication application, Nesting nesting) 
            => application.WhenFrame().WhenFrame(nesting);
        
        public static IObservable<Frame> WhenFrame(this XafApplication application, params string[] viewIds) 
            => application.WhenFrame().WhenFrame(viewIds);
        
        public static IObservable<Frame> WhenFrame(this XafApplication application, Type objectType ,
            ViewType viewType = ViewType.Any, Nesting nesting = Nesting.Any) 
            => application.WhenFrame(_ => objectType,_ => viewType,nesting);
        
        public static IObservable<Frame> WhenFrame(this XafApplication application, params ViewType[] viewTypes) 
            => application.WhenFrame().WhenFrame(viewTypes);
        
        public static IObservable<ListPropertyEditor> WhenNestedFrame(this XafApplication application, Type parentObjectType,params Type[] objectTypes)
            => application.WhenFrame(parentObjectType,ViewType.DetailView).SelectUntilViewClosed(frame => frame.NestedListViews(objectTypes));
        
        public static IObservable<Frame> WhenFrame(this XafApplication application, Func<Frame,Type> objectType,
            Func<Frame,ViewType> viewType = null, Nesting nesting = Nesting.Any) 
            => application.WhenFrame().WhenFrame(objectType,viewType,nesting);

        public static IObservable<T> WhenFrame<T>(this IObservable<T> source, Func<Frame,Type> objectType = null,
            Func<Frame,ViewType> viewType = null, Nesting nesting = Nesting.Any) where T:Frame
            => source.Where(frame => frame.When(nesting))
                .SelectMany(frame => frame.WhenFrame(viewType?.Invoke(frame)??ViewType.Any, objectType?.Invoke(frame)));
        [Obsolete]
        public static IObservable<Window> Navigate(this XafApplication application,string viewId) 
            => application.Navigate(viewId,application.WhenFrame(viewId).Take(1)).Cast<Window>();
        public static IObservable<Window> Navigate2(this XafApplication application,string viewId,Func<Window,IObservable<Unit>> navigate=null) 
            => application.Navigate(viewId,frame =>frame.WhenFrame(viewId),navigate).Take(1).Cast<Window>();
        
        public static IObservable<Frame> WhenFrameViewChanged(this XafApplication application) 
            => application.WhenFrameCreated()
                // .Where(frame => frame.Context!=TemplateContext.ApplicationWindow).Select(frame => frame)
                .WhenViewChanged();

        public static IObservable<DetailView> WhenExistingObjectRootDetailView(this XafApplication application,Type objectType=null)
            => application.WhenExistingObjectRootDetailViewFrame(objectType).Select(frame => frame.View).Cast<DetailView>();
        public static IObservable<Frame> WhenExistingObjectRootDetailViewFrame(this XafApplication application,Type objectType=null)
            => application.WhenRootDetailViewFrame(objectType).Where(frame => !frame.View.ToDetailView().IsNewObject());

        public static IObservable<DetailView> WhenRootDetailView(this XafApplication application, Type objectType=null) 
            => application.WhenRootDetailViewFrame(objectType).Select(frame => frame.View).OfType<DetailView>();
        public static IObservable<Frame> WhenRootDetailViewFrame(this XafApplication application, Type objectType=null) 
            => application.WhenRootFrame(objectType,ViewType.DetailView);
        public static IObservable<Frame> WhenRootFrame(this XafApplication application, Type objectType=null) 
            => application.WhenRootFrame(objectType,ViewType.DetailView).WhenNotDefault(frame => frame.View.CurrentObject);

        public static IObservable<LayoutManager> WhenLayoutCreated(this LayoutManager layoutManager) 
            => layoutManager.WhenEvent(nameof(layoutManager.LayoutCreated)).To(layoutManager);
        
        public static IObservable<DetailView> NewObjectRootDetailView(this XafApplication application,Type objectType)
            => application.NewObjectRootFrame(objectType).Select(frame => frame.View.ToDetailView());
        public static IObservable<Frame> NewObjectRootFrame(this XafApplication application,Type objectType=null)
            => application.WhenRootFrame(objectType).Where(frame => frame.View.ToCompositeView().IsNewObject());
        
        public static IObservable<DashboardView> WhenDashboardViewCreated(this XafApplication application) 
            => application.WhenEvent<DashboardViewCreatedEventArgs>(nameof(XafApplication.DashboardViewCreated)).Select(e => e.View);
        public static IObservable<Frame> WhenDeleteObject(this IObservable<(ITypeInfo typeInfo, object keyValue, Frame frame,Frame parent)> source,Frame parentFrame)
            => source.SelectMany(t => t.frame.GetController<DeleteObjectsViewController>().DeleteAction
                .Trigger( t.frame.View.ObjectSpace.GetObjectByKey(t.frame.View.ObjectTypeInfo.Type, t.keyValue)).To<Frame>().IgnoreElements()
                .Concat(t.frame.WhenAggregatedSave(t.parent).Select(frame => frame).IgnoreElements()
                    .ConcatDefer(() => t.frame.WhenDialogAccept(parentFrame).Select(_ => t.frame)))
                );

        public static IObservable<(Type type, object keyValue, XafApplication application)> WhenDeleteObject(this IObservable<Frame> source)
            => source.SelectMany(frame => {
                    var keyValue = frame.View.ObjectSpace.GetKeyValue(frame.View.CurrentObject);
                    var type = frame.View.ObjectTypeInfo.Type;
                    var application = frame.Application;
                    return frame.GetController<DeleteObjectsViewController>().DeleteAction
                        .Trigger(frame.WhenDisposedFrame().Select(_ => (type,keyValue,application)));
            });
        public static IObservable<(Type type, object keyValue, XafApplication application, Frame parent,bool isAggregated)> WhenDeleteObject(this IObservable<(Frame frame, Frame parent,bool isAggregated)> source)
            => source.SelectMany(t => {
                    var keyValue = t.frame.View.ObjectSpace.GetKeyValue(t.frame.View.CurrentObject);
                    var type = t.frame.View.ObjectTypeInfo.Type;
                    var application = t.frame.Application;
                    return t.frame.GetController<DeleteObjectsViewController>().DeleteAction
                        .Trigger(!t.isAggregated ? t.frame.WhenDisposedFrame().Take(1) : t.parent.Observe().WhenNotDefault()
                                .SelectMany(frame => frame.View.ObjectSpace.WhenModifyChanged().Take(1)
                                    .Select(_ => frame.GetController<ModificationsController>().SaveAction)
                                    .SelectMany(simpleAction => simpleAction.Trigger())))
                        .Select(_ => (type, keyValue, application, t.parent, t.isAggregated)).Take(1);
            });
        
        [Obsolete]
        public static IObservable<Frame> Navigate(this XafApplication application,string viewId, IObservable<Frame> afterNavigation) 
            => afterNavigation.Publish(source => application.MainWindow == null ? application.WhenWindowCreated(true)
                    .SelectMany(window => window.Navigate(viewId, source))
                : application.MainWindow.Navigate(viewId, source));
        public static IObservable<Frame> Navigate(this XafApplication application,string viewId, Func<Frame,IObservable<Frame>> afterNavigation,Func<Window,IObservable<Unit>> navigate=null) 
            => application.Defer(() => application.MainWindow == null ? application.WhenWindowCreated(true)
                    .SelectMany(window => window.Navigate(viewId,afterNavigation(window),navigate))
                : application.MainWindow.Navigate(viewId, afterNavigation(application.MainWindow),navigate));
        
        public static bool DbExist(this XafApplication application,string connectionString=null) {
            var builder = new SqlConnectionStringBuilder(connectionString??application.ConnectionString);
            var initialCatalog = "Initial catalog";
            var databaseName = builder[initialCatalog].ToString();
            builder.Remove(initialCatalog);
            using var sqlConnection = new SqlConnection(builder.ConnectionString);
            return sqlConnection.DbExists(databaseName);
        }
        public static bool DbExists(this IDbConnection dbConnection, string databaseName=null){
            if (dbConnection.State != ConnectionState.Open) {
                dbConnection.Open();
            }
            using var dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = $"SELECT db_id('{databaseName??dbConnection.Database}')";
            return dbCommand.ExecuteScalar() != DBNull.Value;
        }

        public static IObservable<(ListView listView, XafApplication application)> WhenListViewCreated(this IObservable<XafApplication> source,Type objectType=null) 
            => source.SelectMany(application => application.WhenListViewCreated(objectType).Pair(application));

        public static IObservable<ListView> WhenListViewCreated(this XafApplication application,Type objectType=null) 
            => application.WhenEvent<ListViewCreatedEventArgs>(nameof(XafApplication.ListViewCreated))
                .Select(pattern => pattern.ListView)
                .Where(view => objectType==null||objectType.IsAssignableFrom(view.ObjectTypeInfo.Type));
        
        public static IObservable<DetailView> ToDetailView(this IObservable<(XafApplication application, DetailViewCreatedEventArgs e)> source) 
            => source.Select(t => t.e.View);


        public static IObservable<Unit> FilterListViews(this XafApplication application, Func<DetailView, System.Linq.Expressions.LambdaExpression, IObservable<object>> userControlSelector, 
            params System.Linq.Expressions.LambdaExpression[] expressions) 
            => application.FuseAny(expressions)
                .SelectMany(expression => application.WhenDetailViewCreated(expression.Parameters.First().Type).ToDetailView()
                    .SelectMany(view => view.WhenControlsCreated())
                    .SelectMany(view => userControlSelector(view, expression))
                    .MergeToUnit(application.WhenListViewCreating(expression.Parameters.First().Type)
                        .Select(t => t.e.CollectionSource)
                        .Do(collectionSourceBase => collectionSourceBase.SetCriteria(expression))));
        
        public static IObservable<(XafApplication application, DetailViewCreatedEventArgs e)> WhenDetailViewCreated(this XafApplication application,Type objectType) 
            => application.WhenDetailViewCreated().Where(t =>objectType?.IsAssignableFrom(t.e.View.ObjectTypeInfo.Type)??true);

        public static IObservable<(XafApplication application, DetailViewCreatedEventArgs e)> WhenDetailViewCreated(this XafApplication application) 
            => application.WhenEvent<DetailViewCreatedEventArgs>(nameof(XafApplication.DetailViewCreated)).InversePair(application);


        public static IObservable<Window> WhenMainWindowCreated(this XafApplication application,  bool emitIfMainExists = true) 
            => application.WhenWindowCreated(true, emitIfMainExists);

        public static IObservable<Window> WhenWindowCreated(this XafApplication application,bool isMain=false,bool emitIfMainExists=true) {
            var windowCreated = application.WhenFrameCreated().Select(frame => frame).OfType<Window>();
            return isMain ? emitIfMainExists && application.MainWindow != null ? application.MainWindow.Observe().ObserveOn(SynchronizationContext.Current!)
                : windowCreated.WhenMainWindowAvailable() : windowCreated;
        }

        private static IObservable<Window> WhenMainWindowAvailable(this IObservable<Window> windowCreated) 
            => windowCreated.When(TemplateContext.ApplicationWindow).Select(window => window).TemplateChanged().Cast<Window>().Take(1)
                .Select(window => window);

        public static IObservable<Frame> Navigate(this Window window,string viewId, IObservable<Frame> afterNavigation,Func<Window,IObservable<Unit>> navigate=null){
            navigate ??= _ => Unit.Default.Observe();
            return navigate(window).SelectMany(_ => {
                var controller = window.GetController<ShowNavigationItemController>();
                return controller.ShowNavigationItemAction.Trigger(afterNavigation,
                    () => controller.FindNavigationItemByViewShortcut(new ViewShortcut(viewId, null)));
            });
        }

        public static IObservable<string[]> NavigationViews(this XafApplication application) 
            => application.WhenWindowCreated(true)
                .ToController<ShowNavigationItemController>().Take(1)
                .Select(controller => controller.ShowNavigationItemAction.Items.SelectManyRecursive(item => item.Items)
                    .Where(item => item.Active).Select(item => item.Data).OfType<ViewShortcut>()
                    .Select(item => item.ViewId).ToArray());


        public static IObservable<Unit> WhenLoggedOn<TParams>(
            this XafApplication application, string userName, string pass=null) where TParams:IAuthenticationStandardLogonParameters
            => application.WhenFrame(typeof(TParams), ViewType.DetailView).Take(1)
                .Do(frame => {
                    var logonParameters = ((TParams)frame.View.CurrentObject);
                    logonParameters.UserName = userName;
                    logonParameters.Password = pass;
                })
                .ToController<DialogController>().WhenAcceptTriggered();
        public static IObservable<Unit> WhenLoggedOn(this XafApplication application, string userName, string pass=null) 
            => application.WhenLoggedOn<AuthenticationStandardLogonParameters>(userName,pass);

        public static IObservable<(XafApplication application, LogonEventArgs e)> WhenLoggedOn(this XafApplication application) 
            => application.WhenEvent<LogonEventArgs>(nameof(XafApplication.LoggedOn)).InversePair(application);
        
        
        public static IEnumerable<IObjectSpaceProvider> ObjectSpaceProviders(this XafApplication application, params Type[] objectTypes) 
            => objectTypes.Select(application.GetObjectSpaceProvider).Distinct();
        public static IObjectSpace CreateObjectSpace(this XafApplication application, bool useObjectSpaceProvider,Type type=null,bool nonSecuredObjectSpace=false,
            [CallerMemberName] string caller = "") {
            if (type != null) {
                if (type.IsArray) {
                    type = type.GetElementType();
                }
                if (!XafTypesInfo.Instance.FindTypeInfo(type).IsPersistent) {
                    throw new InvalidOperationException($"{caller} {type?.FullName} is not a persistent object");
                }
            }
            if (!useObjectSpaceProvider)
                return application.CreateObjectSpace(type ?? typeof(object));
            var applicationObjectSpaceProvider = application.ObjectSpaceProviders(type ?? typeof(object)).First();
            IObjectSpace objectSpace;
            if (!nonSecuredObjectSpace) {
                objectSpace = applicationObjectSpaceProvider.CreateObjectSpace();
            }
            else if (applicationObjectSpaceProvider is INonsecuredObjectSpaceProvider nonsecuredObjectSpaceProvider) {
                objectSpace= nonsecuredObjectSpaceProvider.CreateNonsecuredObjectSpace();
            }
            else {
                objectSpace= applicationObjectSpaceProvider.CreateUpdatingObjectSpace(false);    
            }

            if (objectSpace is CompositeObjectSpace compositeObjectSpace) {
                compositeObjectSpace.PopulateAdditionalObjectSpaces(application);
            }
            return objectSpace;
        }

        public static IObservable<T> UseObjectSpace<T>(this XafApplication application,Func<IObjectSpace,IObservable<T>> factory,bool useObjectSpaceProvider=false,[CallerMemberName]string caller="") 
            => Observable.Using(() => application.CreateObjectSpace(useObjectSpaceProvider, typeof(T), caller: caller), factory);
        public static IObservable<ListView> ToListView(this IObservable<(XafApplication application, ListViewCreatedEventArgs e)> source) 
            => source.Select(t => t.e.ListView);

        
        public static IObservable<TView> ToObjectView<TView>(this IObservable<(ObjectView view, EventArgs e)> source) where TView:View 
            => source.Where(t => t.view is TView).Select(t => t.view).Cast<TView>();

        
        public static IObservable<(XafApplication application, DetailViewCreatingEventArgs e)> WhenDetailViewCreating(this XafApplication application,params Type[] objectTypes) 
            => application.WhenEvent<DetailViewCreatingEventArgs>(nameof(XafApplication.DetailViewCreating)).InversePair(application)
                .Where(t => !objectTypes.Any() || objectTypes.Contains(application.Model.Views[t.source.ViewID].AsObjectView.ModelClass.TypeInfo.Type));
        public static IObservable<(XafApplication application, ListViewCreatingEventArgs e)> WhenListViewCreating(this XafApplication application,Type objectType=null,bool? isRoot=null) 
            => application.WhenEvent<ListViewCreatingEventArgs>(nameof(XafApplication.ListViewCreating))
                .Where(pattern => (!isRoot.HasValue || pattern.IsRoot == isRoot) &&
                                  (objectType == null || objectType.IsAssignableFrom(pattern.CollectionSource.ObjectTypeInfo.Type)))
                .InversePair(application);
        
        public static Controller GetController(this Frame frame, string controllerName) 
            => frame.Controllers.Cast<Controller>().FirstOrDefault(controller => controller.Name==controllerName);
        
        public static IEnumerable<T> GetControllers<T>(this Frame frame) where T:Controller
            => frame.GetControllers(typeof(T)).Cast<T>();
        public static IObservable<Frame> NavigateBack(this XafApplication application){
            var viewNavigationController = application.MainWindow.GetController<ViewNavigationController>();
            return viewNavigationController.NavigateBackAction
                .Trigger(application.WhenFrame(Nesting.Root).OfType<Window>()
                        .SelectMany(window => window.View.WhenControlsCreated(emitExisting:true).Take(1).To(window)),
                    () => viewNavigationController.NavigateBackAction.Items.First())
                .Select(window => window);
        }

        public static IObservable<(ITypeInfo typeInfo, object keyValue, Frame frame)> WhenSaveObject(this IObservable<Frame> source,Frame parentFrame)
            => source.If(frame => frame.GetController<DialogController>()==null,frame => {
                    var currentObjectInfo = frame.View.CurrentObjectInfo();
                    return frame.Observe().ToController<ModificationsController>()
                        .SelectMany(controller => controller.SaveAction.Trigger()).To(frame).CloseWindow(parentFrame)
                        .Select(frame1 => (currentObjectInfo.typeInfo,currentObjectInfo.keyValue,source: frame1));
                },
                frame => {
                    var currentObjectInfo = frame.View.CurrentObjectInfo();
                    var acceptAction = frame.GetController<DialogController>().AcceptAction;
                    return acceptAction.Trigger().Take(1).BufferUntilCompleted()
                        .SelectMany(_ => currentObjectInfo.Observe().Select(t1 => (t1.typeInfo,t1.keyValue,parentFrame)));
                }
            );

        public static IObservable<IObjectSpace> WhenObjectSpaceCreated(this XafApplication application,bool includeNonPersistent=true,bool includeNested=false) 
            => application.WhenEvent<ObjectSpaceCreatedEventArgs>(nameof(XafApplication.ObjectSpaceCreated)).InversePair(application)
                .Where(t => (includeNonPersistent || t.source.ObjectSpace is not NonPersistentObjectSpace)&& (includeNested || t.source.ObjectSpace is not INestedObjectSpace)).Select(t => t.source.ObjectSpace);

        public static IObservable<(IObjectSpace objectSpace, CancelEventArgs e)> WhenCommiting(this XafApplication  application)
            => application.WhenObjectSpaceCreated().SelectMany(objectSpace => objectSpace.WhenCommiting().Select(e => (objectSpace,e)));
        
        public static IObservable<(IObjectSpace objectSpace, IEnumerable<T> objects)> WhenCommiting<T>(
            this XafApplication application, ObjectModification objectModification = ObjectModification.All) where T : class 
            => application.WhenObjectSpaceCreated().SelectMany(objectSpace => objectSpace.WhenCommiting<T>(objectModification));
        
        public static IObservable<(IObjectSpace objectSpace, IEnumerable<T> objects)> WhenCommitted<T>(
            this XafApplication application,ObjectModification objectModification,[CallerMemberName]string caller="") where T:class
            => application.WhenObjectSpaceCreated()
                .SelectMany(objectSpace => objectSpace.WhenCommitted<T>(objectModification,caller).TakeUntil(objectSpace.WhenDisposed()));
        public static IObservable<View> WhenRootView(this XafApplication application,Type objectType,params ViewType[] viewTypes) 
            => application.WhenRootFrame(objectType,viewTypes).Select(frame => frame.View);
        public static IObservable<Frame> WhenRootFrame(this XafApplication application,Type objectType,params ViewType[] viewTypes) 
            => application.WhenFrame(objectType,viewTypes).When(TemplateContext.View);
    }
}