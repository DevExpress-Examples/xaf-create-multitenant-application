using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraGrid.Views.Base;
using XAF.Testing.RX;
using ListView = DevExpress.ExpressApp.ListView;
using View = DevExpress.ExpressApp.View;

namespace XAF.Testing.XAF{
    public static class XafApplicationExtensions{
        
        
        public static IObservable<DevExpress.XtraLayout.TabbedGroup> WhenDashboardViewTabControl(this XafApplication application, string viewVariant,Type objectType) 
            => application.WhenDashboardViewCreated().When(viewVariant)
                .Select(_ => application.WhenDetailViewCreated(objectType).ToDetailView()).Switch()
                .SelectMany(detailView => detailView.WhenTabControl()).Cast<DevExpress.XtraLayout.TabbedGroup>();
        
        

        public static IObservable<Unit> ThrowWhenHandledExceptions(this WinApplication application) 
            => application.WhenEvent<CustomHandleExceptionEventArgs>(nameof(application.CustomHandleException))
                .Do(e =>e.Handled= e.Exception.ToString().Contains("DevExpress.XtraMap.Drawing.RenderController.Render"))
                .Where(e => !e.Handled)
                .Select(e => e.Exception)
                .Merge(application.WhenGridListEditorDataError())
                .Do(exception => exception.ThrowCaptured())
                .ToUnit();
        public static IObservable<Exception> WhenGridListEditorDataError(this WinApplication application) 
            => application.WhenFrame(typeof(object),ViewType.ListView)
                .SelectUntilViewClosed(frame => frame.View.ToListView().Editor is GridListEditor gridListEditor
                    ? gridListEditor.WhenControlsCreated().StartWith(gridListEditor.Control).WhenNotDefault().Take(1)
                        .SelectMany(_ => gridListEditor.GridView
                            .WhenEvent<ColumnViewDataErrorEventArgs>(nameof(gridListEditor.GridView.DataError))
                            .Select(e => e.DataException))
                    : Observable.Empty<Exception>());

    }
}