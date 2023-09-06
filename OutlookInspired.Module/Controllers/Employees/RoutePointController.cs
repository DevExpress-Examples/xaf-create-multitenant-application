using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers.Maps;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers.Employees{
    public class RoutePointController:ObjectViewController<DetailView,Employee>{
        private IMapsRouteController _mapsRouteController;
        public RoutePointController() => TargetViewId = Employee.MapsDetailView;

        protected override void OnDeactivated(){
            base.OnDeactivated();
            _mapsRouteController.RouteCalculated-=OnRouteCalculated;
        }

        protected override void OnActivated(){
            base.OnActivated();
            var employee = ((Employee)View.CurrentObject);
            var homeOffice = ((IModelOptionsHomeOffice)Application.Model.Options).HomeOffice;
            employee.AAddress = $"{homeOffice.Line}, {homeOffice.City}, {homeOffice.State} {homeOffice.City} {homeOffice.ZipCode}";
            employee.BAddress = $"{employee.Address}, {employee.City}, {employee.State} {employee.City} {employee.ZipCode}";
            _mapsRouteController = Frame.GetControllers<IMapsRouteController>().First();
            _mapsRouteController.RouteCalculated+=OnRouteCalculated;
        }

        private void OnRouteCalculated(object sender, RouteCalculatedArgs e){
            ((Employee)View.CurrentObject).RoutePoints.Clear();
            e.RoutePoints.ForEach(((Employee)View.CurrentObject).RoutePoints.Add);
        }
    }
}