using System.Data;
using System.Data.SqlClient;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using Microsoft.Extensions.DependencyInjection;

namespace XAF.Testing.XAF{
    public static class XafApplicationExtensions{
        public static IObservable<ITabControlProvider> WhenTabControl(this XafApplication application, Type objectType=null, Func<DetailView, bool> match=null)
            => application.WhenTabControl<ITabControlProvider>(objectType, match);
        public static IObservable<TTabbedControl> WhenTabControl<TTabbedControl>(this XafApplication application, Type objectType=null, Func<DetailView, bool> match=null) 
            => application.WhenDetailViewCreated(objectType).ToDetailView()
                .Where(view => match?.Invoke(view)??true)
                .SelectMany(detailView => detailView.WhenTabControl()).Cast<TTabbedControl>();

        public static bool IsDisposed(this XafApplication application)
            => (bool)application.GetPropertyValue("IsDisposed");
        
        public static T GetService<T>(this XafApplication application) where T : notnull
            => application.ServiceProvider.GetService<T>();
        public static T GetRequiredService<T>(this XafApplication application) where T : notnull 
            => application.ServiceProvider.GetRequiredService<T>();

        public static IObservable<Frame> WhenFrameCreated(this XafApplication application,TemplateContext templateContext=default)
            => application.WhenEvent<FrameCreatedEventArgs>(nameof(XafApplication.FrameCreated)).Select(e => e.Frame)
                .Where(frame => frame.Application==application&& (templateContext==default ||frame.Context == templateContext));

        public static IObservable<Frame> WhenFrame(this XafApplication application)
            => application.WhenFrameViewChanged().Merge(application.MainWindow.Observe().WhenNotDefault()
                .SelectMany(window => new[]{window,window.ToActive()}).WhenViewChanged().WhenNotDefault(window => window.View));
        public static IObservable<Frame> WhenFrame(this XafApplication application, Type objectType , params ViewType[] viewTypes) 
            => application.WhenFrame(objectType).WhenFrame(viewTypes);

        public static IObservable<Frame> WhenFrame(this XafApplication application, params string[] viewIds) 
            => application.WhenFrame().WhenFrame(viewIds);
        
        public static IObservable<Frame> WhenFrame(this XafApplication application, Type objectType ,
            ViewType viewType = ViewType.Any, Nesting nesting = Nesting.Any) 
            => application.WhenFrame(_ => objectType,_ => viewType,nesting);
        
        public static IObservable<Frame> WhenFrame(this XafApplication application, params ViewType[] viewTypes) 
            => application.WhenFrame().WhenFrame(viewTypes);

        public static IObservable<Frame> WhenFrame(this XafApplication application, Func<Frame,Type> objectType,
            Func<Frame,ViewType> viewType = null, Nesting nesting = Nesting.Any) 
            => application.WhenFrame().WhenFrame(objectType,viewType,nesting);

        public static IObservable<T> WhenFrame<T>(this IObservable<T> source, Func<Frame,Type> objectType = null,
            Func<Frame,ViewType> viewType = null, Nesting nesting = Nesting.Any) where T:Frame
            => source.Where(frame => frame.When(nesting))
                .SelectMany(frame => frame.WhenFrame(viewType?.Invoke(frame)??ViewType.Any, objectType?.Invoke(frame)));

        public static IObservable<Window> Navigate(this XafApplication application,string viewId,Func<Window,IObservable<Unit>> navigate=null) 
            => application.Navigate(viewId,frame =>frame.WhenFrame(viewId).Select(frame1 => frame1),navigate).Take(1).Cast<Window>();
        
        public static IObservable<Frame> WhenFrameViewChanged(this XafApplication application) 
            => application.WhenFrameCreated().WhenViewChanged();

        public static IObservable<LayoutManager> WhenLayoutCreated(this LayoutManager layoutManager) 
            => layoutManager.WhenEvent(nameof(layoutManager.LayoutCreated)).To(layoutManager);

        public static IObservable<Frame> WhenDeleteObject(this IObservable<(ITypeInfo typeInfo, object keyValue, Frame frame,Frame parent)> source,Frame parentFrame)
            => source.SelectMany(t => t.frame.GetController<DeleteObjectsViewController>().DeleteAction
                .Trigger( t.frame.View.ObjectSpace.GetObjectByKey(t.frame.View.ObjectTypeInfo.Type, t.keyValue)).To<Frame>().IgnoreElements()
                .Concat(t.frame.WhenAggregatedSave(t.parent).Select(frame => frame).IgnoreElements()
                    .ConcatDefer(() => t.frame.WhenDialogAccept(parentFrame).Select(_ => t.frame)))
                );

        public static IObservable<Frame> Navigate(this XafApplication application,string viewId, Func<Frame,IObservable<Frame>> afterNavigation,Func<Window,IObservable<Unit>> navigate=null) 
            => application.Defer(() => application.MainWindow == null ? application.WhenWindowCreated(true)
                    .SelectMany(window => window.Navigate(viewId,afterNavigation(window),navigate))
                : application.MainWindow.Navigate(viewId, afterNavigation(application.MainWindow),navigate));
        
        public static bool TenantsExist(this XafApplication application,string connectionString=null,int recordCount=2){
            connectionString ??= application.ConnectionString;
            if (!application.DbExist(connectionString)) return false;
            using var sqlConnection = new SqlConnection(connectionString);
            var cmdText = @"
            IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Tenant')
            BEGIN
                SELECT CASE WHEN COUNT(*) = @RecordCount THEN 1 ELSE 0 END FROM dbo.Tenant;
            END
            ELSE
            BEGIN
                SELECT 0;
            END";

            using var command = new SqlCommand(cmdText, sqlConnection);
            command.Parameters.AddWithValue("@RecordCount", recordCount);
            sqlConnection.Open();
            var result = command.ExecuteScalar()!;
            sqlConnection.Close();
            return result == (object)1;

        }

        public static bool DbExist(this XafApplication application,string connectionString=null) 
            => new SqlConnectionStringBuilder(connectionString??application.ConnectionString).DbExists();

        public static bool DbExists(this SqlConnectionStringBuilder builder){
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

        public static bool DBExist(this SqlConnectionStringBuilder builder) {
            var initialCatalog = builder.InitialCatalog;
            builder.Remove("Initial catalog");
            using var sqlConnection = new SqlConnection(builder.ConnectionString);
            return sqlConnection.DbExists(initialCatalog);
        }
        
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
            => windowCreated.When(TemplateContext.ApplicationWindow).TemplateChanged().Cast<Window>().Take(1)
                .Select(window => window.Application.MainWindow ?? window);

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


        public static IObservable<XafApplication> WhenLoggedOn<TParams>(
            this XafApplication application, string userName, string pass=null) where TParams:IAuthenticationStandardLogonParameters
            => application.WhenFrame(typeof(TParams), ViewType.DetailView).Take(1)
                .Do(frame => {
                    var logonParameters = ((TParams)frame.View.CurrentObject);
                    logonParameters.UserName = userName;
                    logonParameters.Password = pass;
                })
                .ToController<DialogController>().WhenAcceptTriggered(application.WhenLoggedOn().Select(t => t.application));
        public static IObservable<XafApplication> WhenLoggedOn(this XafApplication application, string userName, string pass=null) 
            => application.WhenLoggedOn<AuthenticationStandardLogonParameters>(userName,pass);

        public static IObservable<(XafApplication application, LogonEventArgs e)> WhenLoggedOn(this XafApplication application) 
            => application.WhenEvent<LogonEventArgs>(nameof(XafApplication.LoggedOn)).InversePair(application);
        public static IObservable<XafApplication> WhenLogOff(this XafApplication application) 
            => application.WhenEvent<EventArgs>(nameof(XafApplication.LoggedOff)).Select(_ => application);


        public static IObservable<(XafApplication application, ListViewCreatingEventArgs e)> WhenListViewCreating(this XafApplication application,Type objectType=null,bool? isRoot=null) 
            => application.WhenEvent<ListViewCreatingEventArgs>(nameof(XafApplication.ListViewCreating))
                .Where(pattern => (!isRoot.HasValue || pattern.IsRoot == isRoot) &&
                                  (objectType == null || objectType.IsAssignableFrom(pattern.CollectionSource.ObjectTypeInfo.Type)))
                .InversePair(application);
        
        public static IObservable<T> CloseTrialWindow<T>(this IObservable<T> source) where T:XafApplication 
            => source.MergeIgnored(application => "About DevExpress".CloseWindow().TakeUntil(application.WhenLoggedOn()));

        public static IObservable<Frame> NavigateBack(this XafApplication application){
            var viewNavigationController = application.MainWindow.GetController<ViewNavigationController>();
            viewNavigationController.NavigateBackAction.SelectedItem = viewNavigationController.NavigateBackAction.Items.First();
            return viewNavigationController.NavigateBackAction.Trigger(application.MainWindow.WhenViewChanged());
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

        public static IObservable<(IObjectSpace objectSpace, IEnumerable<T> objects)> WhenCommitted<T>(
            this XafApplication application,ObjectModification objectModification,[CallerMemberName]string caller="") where T:class
            => application.WhenObjectSpaceCreated()
                .SelectMany(objectSpace => objectSpace.WhenCommitted<T>(objectModification,caller).TakeUntil(objectSpace.WhenDisposed()));
    }
}