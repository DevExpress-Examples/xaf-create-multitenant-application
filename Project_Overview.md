![Azure DevOps tests with custom labels](https://img.shields.io/azure-devops/tests/eXpandDevOps/eXpandFramework/96)
 ![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/eXpandDevOps/eXpandFramework/96)


# Solution Overview

## Introduction

Welcome to the OutlookInspired XAF application—a modern iteration of our original WinForms-based solution. This cross-platform **ERP/LOB** application is tailored for both Windows and Blazor, with plans for further platform expansion. It serves as the central data management hub for DevAv Company, overseeing various business entities such as Employees, Products, Orders, Quotes, Customers, and Stores.

## Core Objectives

- **Data Management**: This is a comprehensive tool for managing an array of business entities and is designed to streamline DevAv Company's ordering processes and customer relations.

- **Employee Evaluations**: A built-in scheduler is utilized for the systematic scheduling of employee evaluations, ensuring regular performance assessments.

## Architecture

### Domain Diagram

The domain architecture is depicted in the diagram below:

![](Images/DomainModel.png)

### Solution Structure

The solution is structured into five independent projects.

![Project Structure Diagram](Images/Solution.png)

#### `OutlookInspired.Module` project

Next, we describe the functionality found in each project folder.

##### `Services` Folder

This folder is pivotal, serving as the storage for our business logic. The aim is to keep all other classes lean, centralizing logic within this folder. For instance, methods that utilize `XafApplication` are located in `Services/XafApplicationExtensions`.

```cs
public static IObjectSpace NewObjectSpace(this XafApplication application) 
    => application.CreateObjectSpace(typeof(OutlookInspiredBaseObject));
```

Methods that use IObjectSpace can be found in Services/ObjectSpaceExtensions. For example:

```cs
public static TUser CurrentUser<TUser>(this IObjectSpace objectSpace) where TUser:ISecurityUser 
    => objectSpace.GetObjectByKey<TUser>(objectSpace.ServiceProvider.GetRequiredService<ISecurityStrategyBase>().UserId);
```
Take note of the SecurityExtensions class, where we configure a diverse set of permissions for each department. For instance, the Management department will have:

1. CRUD permissions for EmployeeTypes
2. Read-only permissions for CustomerTypes
3. Navigation permissions for Employees, Evaluations, and Customers
4. Mail merge permissions for orders and customers
5. Permissions for various reports including Revenue, Contacts, TopSalesMan, and Locations.

```cs
private static void AddManagementPermissions(this PermissionPolicyRole role) 
    => EmployeeTypes.AddCRUDAccess(role)
        .Concat(CustomerTypes.Prepend(typeof(ApplicationUser)).AddReadAccess(role)).To<string>()
        .Concat(new[]{ EmployeeListView,EvaluationListView,CustomerListView}.AddNavigationAccess(role))
        .Finally(() => {
            role.AddMailMergePermission(data => new[]{ MailMergeOrder, MailMergeOrderItem, ServiceExcellence }.Contains(data.Name));
            role.AddReportPermission(data => new[]{ RevenueReport, Contacts, LocationsReport, TopSalesPerson }.Contains(data.DisplayName));
        })
        .Enumerate();
```

##### `Attributes` Folder

In the `Attributes` folder, you'll find attribute declarations, the implementations of which reside in another library.

* `FontSizeDeltaAttribute`
  This attribute is applied to properties of `Customer`, `Employee`, `Evaluation`, `EmployeeTask`, `Order`, and `Product` to configure the font size. The implementation is context-dependent; for Windows, it is utilized from the `LabelPropertyEditor`.


  ```cs
   protected override object CreateControlCore() 
   => new LabelControl{
         BorderStyle = BorderStyles.NoBorder,
         AutoSizeMode = LabelAutoSizeMode.None,
         ShowLineShadow = false,
         Appearance ={
            FontSizeDelta = MemberInfo.FindAttribute<FontSizeDeltaAttribute>()?.Delta??0,
            TextOptions = { WordWrap =MemberInfo.Size==-1? WordWrap.Wrap:WordWrap.Default}
         }
   };
  ```

  also from the ListEditor `GridView`:

  ```cs
   public static void IncreaseFontSize(this GridView gridView, ITypeInfo typeInfo){
      var columns = typeInfo.AttributedMembers<FontSizeDeltaAttribute>().ToDictionary(
            attribute => gridView.Columns[attribute.memberInfo.BindingName].VisibleIndex,
            attribute => attribute.attribute.Delta);
      gridView.CustomDrawCell += (_, e) => {
            if (columns.TryGetValue(e.Column.VisibleIndex, out var column)) e.DrawCell( column);
      };
   }
  ```

  ![](Images/WinFontDelta.png)

  for Blazor it is implemented from an extension method:
  
  ```cs
    public static string FontSize(this IMemberInfo info){
        var fontSizeDeltaAttribute = info.FindAttribute<FontSizeDeltaAttribute>();
        return fontSizeDeltaAttribute != null ? $"font-size: {(fontSizeDeltaAttribute.Delta == 8 ? "1.8" : "1.2")}rem" : null;
    }
  ```

  and used similarly:
  
  ![](Images/BlazorFontDelta.png)

- ##### `Appearance` Subfolder

  The following [Conditional Appearance module](https://docs.devexpress.com/eXpressAppFramework/113286/conditional-appearance) attributes are located here:

  `DeactivateActionAttribute`: This is an extension of the [Conditional Appearance module](https://docs.devexpress.com/eXpressAppFramework/113286/conditional-appearance) used to deactivate actions.

  ```cs
  public class DeactivateActionAttribute : AppearanceAttribute {
      public DeactivateActionAttribute(params string[] actions) : 
          base($"Deactivate {string.Join(" ", actions)}", DevExpress.  ExpressApp.ConditionalAppearance.AppearanceItemType.Action, "1=1")   {
          Visibility = ViewItemVisibility.Hide;
          TargetItems = string.Join(";", actions);
      }
  }
  
  ```

  Similarly, we derive from this attribute to create the other attributes   found in the same folder (`ForbidCRUDAttribute`, `ForbidDeleteAttribute`,`ForbidDeleteAttribute`).

- ##### `Validation` Subfolder: In this folder, you can find attributes that extend the [XAF Validation module](https://docs.devexpress.com/eXpressAppFramework/113684/validation-module). Available are `EmailAddressAttribute`, `PhoneAttribute`, `UrlAttribute`, `ZipCodeAttribute`. Below is the latter, and the rest are implemented similarly.

  ```cs
  public class ZipCodeAttribute : RuleRegularExpressionAttribute {
      public ZipCodeAttribute() : base(@"^[0-9][0-9][0-9][0-9][0-9]$") {
          CustomMessageTemplate = "Not a valid ZIP code.";
      }
  }
  ```
##### `Controllers` Folder
In this folder we have controllers with no dependecies, they could reside in a different library if we wish.
* The `HideToolBarController`: extends the XAF IModelListView interface with a HideToolBar attribte so we can hide the nested listviews toolbare. 
* The `SplitterPositionController`: extends the XAF model withe a `RelativePosition` so we can configure the splitter position.
##### Features Folder
This folder contains implementations specific to the solution.
- ##### `CloneView` Subfolder

  This subfolder contains the declaration for the `CloneViewAttribute`,   which is used to generate views in addition to the default ones. For   example:
  
  ```cs
  [CloneView(CloneViewType.DetailView, LayoutViewDetailView)]
  [CloneView(CloneViewType.DetailView, ChildDetailView)]
  [CloneView(CloneViewType.DetailView, MapsDetailView)]
  [VisibleInReports(true)]
  [ForbidDelete()]
  public class Employee : OutlookInspiredBaseObject, IViewFilter,   IObjectSpaceLink, IResource, ITravelModeMapsMarker {
      public const string MapsDetailView = "Employee_DetailView_Maps";
      public const string ChildDetailView = "Employee_DetailView_Child";
      public const string LayoutViewDetailView =   "EmployeeLayoutView_DetailView";
  }
- ##### `Customers` Subfolder
  This subfolder contains implementations related to customers, such as:

  - ###### `MailMergeController`
    XAF offers built-in support for [mail merging](https://docs.devexpress.com/eXpressAppFramework/400006/document-management/office-module/mail-merge). This controller modifies the default `ShowInDocumentAction` icons.

      ![Modified ShowInDocumentAction Icon](Images/ShowInDocumentIcon.png)

  - ###### `ReportsController`
    This controller declares an action for displaying Customer Reports, utilizing the [XAF Reports module](https://docs.devexpress.com/eXpressAppFramework/113591/shape-export-print-data/reports/reports-v2-module-overview) API.

      ![Customer Report Action Icon](Images/CustomerReportAction.png)

- ##### `Employees` Subfolder
  This subfolder contains implementations related to Employees, such as:

  - ###### `RoutePointController`
    This controller sets the travel distance once calculated from the MAP service.

    Windows:
    ![](Images/WinTravelDistance.png)

    Blazor:
    ![BlazorTravelDistance](Images/BlazorTravelDistance.jpg)

- ##### `Employees` Subfolder

  This subfolder houses implementations related to map functionalities, including:

  - ###### `RoutePointController`

    This controller declares actions related to map features (`MapItAction`, `TravelModeAction`, `ExportMapAction`, `PrintPreviewMapAction`, `PrintAction`, `StageAction`, `SalesPeriodAction`) and manages their state based on the `ISalesMapMarker` and `IRoutePointMapMarker` interfaces.

- ##### `MasterDetail` Subfolder
  This subfolder houses a platform-agnostic Master-detail implementation using XAF's [DashboardViews](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.DashboardView).

  - ###### `MasterDetailController, IUserControl`
    The `IUserControl` is implemented in a manner similar to the [XAF documentation](https://docs.devexpress.com/eXpressAppFramework/113610/ui-construction/using-a-custom-control-that-is-not-integrated-by-default/using-a-custom-control-that-is-not-integrated-by-default). The distinction lies in adding the UserControl (for Win) or the Component (for Blazor) to a DetailView. With the aid of `IUserControl`, which derives from `ISelectionContext, IComplexControl`, it becomes possible to exhibit view-like behavior. Additionally, XAF actions for the object type function as expected, as they are bound to the view.


- ##### `Orders` Subfolder
  This subfolder focuses on implementations related to sales activities.

  - ###### `FollowUpController`
    Declares an action to display the follow-up mail merge template for the selected order.
    
    ![Follow-Up Template](Images/FollowUp.png)

  - ###### `InvoiceController`
    Utilizes a master-detail mail merge template pair to generate an invoice, converts it to a PDF, and displays it using the `PdfViewEditor`.
  
  - ###### `Pay/Refund Controllers`
    These controllers declare actions to mark the selected order as either Paid or Refunded.
  
  - ###### `ReportController`
    Provides access to reports related to Order Revenue.

    ![Revenue Analysis](Images/RevenueAnalysis.png)

  - ###### `ShipmentDetailController`
    Adds a watermark to the Shipment Report based on the order status.

    ![Watermarked Report](Images/WatermarkReport.png)
    

- ##### `Products` Subfolder
  This subfolder focuses on implementations related to Products.

  - ###### `ReportsController`
    Declares an action to display report for Sales, Shipments, Comparisons, Top Sales Person .
  
  ![ProductReports](Images/ProductReports.png)
  

- ##### `Quotes` Subfolder
  This subfolder focuses on implementations related to Products.

  - ###### `QuoteMapItemController`
    Calculates the QuoteMapItem non-persistent objects used from the Opportunities view
  
  ![Opportunities](Images/Opportunities.png)

- ##### `ViewFilter` Subfolder
  This subfolder has a Filter manager implementation which can be used from the end user to create and save view filters.
  
  ![ViewFilterAction](Images/ViewFilterAction.png)
  ![ViewFilterView](Images/ViewFilterView.png)

#### `OutlookInspired.Win` project
This is the windows fornt end project which uses the previously discussed agnostic `OutlookInspired.Module` and follows the same architectural folder structure.

