﻿using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using OutlookInspired.Blazor.Server;
using OutlookInspired.Blazor.Server.Components.Models;
using OutlookInspired.Blazor.Server.Editors;
using OutlookInspired.Tests.Services;
using XAF.Testing;
using XAF.Testing.Blazor.XAF;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Blazor.Tests.Common{
    class UserControlProcessSelectedObject:IUserControlProcessSelectedObject{
        public IObservable<Frame> Process(Frame frame, object gridControl) 
            => frame.Application.WhenFrame(ViewType.DetailView).Select(frame1 => frame1)
                .Merge(gridControl.Observe().Cast<IUserControlProcessObject>()
                    .Do(processObject => processObject.ProcessSelectedObject())
                    .IgnoreElements().To<Frame>());
    }
    public class AssertMapControl : IAssertMapControl{
        public IObservable<Unit> Assert(DetailView detailView) 
            => detailView.AssertViewItemControl<ComponentModelBase>(
                model => model.WhenEvent(nameof(ComponentModelBase.ClientReady))
                .Select(pattern => pattern).ToUnit()).ToUnit();
    }
    
    class PdfViewerAssertion:IPdfViewerAssertion{
        public IObservable<Unit> Assert(DetailView detailView) 
            => detailView.GetItems<PdfViewerEditor>().ToNowObservable().ControlCreated()
                .SelectMany(editor => ((PdfModelAdapter)editor.Control).Model.WhenEvent(nameof(ComponentModelBase.ClientReady)).Take(1))
                .Select(pattern => pattern)
                .Assert().ToUnit();
    }

    class FilterViewManager:IFilterViewManager{
        public bool InlineEdit => false;
    }
    public class AssertAssertFilterView:IAssertFilterView{
        public IObservable<Unit> AssertCreateNew(SingleChoiceAction action){
            // return source.AssertDialogControllerListView(typeof(ViewFilter), _ => AssertAction.AllButProcess)
                // .ToSecond().IgnoreElements();
                throw new NotImplementedException();
        }
    }
    class UserControlProperties:IUserControlProperties{
        public int ObjectsCount(object control) => ((IUserControlDataSource)control).Objects.Count;
    }

    class UserControlProvider:IUserControlProvider{
        public IObservable<object> WhenGridControl(DetailView detailView) 
            => detailView.WhenControlViewItemControl().OfType<IUserControlDataSource>()
                .SelectMany(source => source.WhenEvent(nameof(IUserControlDataSource.DataSourceChanged)).To(source).StartWith(source));
    }
    
    public class DashboardColumnViewObjectSelector : IDashboardColumnViewObjectSelector{
        public IObservable<Unit> SelectDashboardColumnViewObject(DashboardViewItem item) 
            => item.InnerView.ToDetailView().GetItems<ControlViewItem>().Select(viewItem => viewItem.Control).Cast<IModelSelectObject>()
                .Do(model => model.SelectObject(model.Objects.Cast<object>().First()))
                .ToNowObservable()
                .ToUnit();
    }

}