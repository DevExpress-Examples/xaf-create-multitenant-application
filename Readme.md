![Azure DevOps tests with custom labels](https://img.shields.io/azure-devops/tests/eXpandDevOps/eXpandFramework/96)
 ![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/eXpandDevOps/eXpandFramework/96)
## Table of Contents
  - [Solution Overview](#solution-overview)
    - [Introduction](#introduction)
    - [Architecture](#architecture)
      - [Domain Diagram](#domain-diagram)
      - [Solution Structure](#solution-structure)
      - [OutlookInspired.Module Project](#outlookinspiredmodule-project)
        - [Services Folder](#services-folder)
        - [Attributes Folder](#attributes-folder)
        - [Controllers Folder](#controllers-folder)
        - [Features Folder](#features-folder)


# Solution Overview

## Introduction

Welcome to the OutlookInspired XAF application—a modern iteration of our original WinForms-based solution. This cross-platform **ERP/LOB** application is tailored for both Windows and Blazor, with plans for further platform expansion. It serves as the central data management hub for DevAv Company, overseeing various business entities such as Employees, Products, Orders, Quotes, Customers, and Stores.


## Architecture

### Domain Diagram

The domain architecture is depicted in the diagram below:

![](Images/DomainModel.png)

### Solution Structure

The solution consists of five distinct projects.

![Project Structure Diagram](Images/Solution.png)

- **OutlookInspired.Module**: This is a platform-agnostic module on which all other projects rely.
- **OutlookInspired.Blazor.Server**: This is the Blazor platform port of the existing `OutlookInspired` WinForms demo.
- **OutlookInspired.Win**: This represents the Windows port of the original demo.
- **OutlookInspired.MiddleTier**: This serves as the MiddleTier layer and is exclusively used by the Windows platform.


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

- `FontSizeDeltaAttribute`
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
* <a name="splitter"></a>The `SplitterPositionController`: extends the XAF model withe a `RelativePosition` so we can configure the splitter position.
##### `Features` Folder
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

  <a name="mapsviewcontroller_"></a>
  - ###### `MapsViewController`

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
This is the Windows frontend project. It utilizes the previously mentioned agnostic `OutlookInspired.Module` and adheres to the same architectural folder structure.

Next, we describe the functionality found in each project folder.

##### `Controllers` Folder
In this folder, we house controllers that have no dependencies. If desired, they could be relocated to a different library.

- `DisableSkinsController`: This controller disables the XAF default theme-switching action. We strive for consistency in this demo across multiple platforms. Testing our views in each supported skin would require significant resources.

- **`SplitterPositionController`**: This is the Windows implementation of the [SplitterPositionController](#splitter). We discussed its platform agnostic counterpart in the `OutlookInspired.Module`.


##### `Editors` Folder
In this folder, we house custom user controls and XAF [property editors](https://docs.devexpress.com/eXpressAppFramework/113097/ui-construction/view-items-and-property-editors/property-editors).

- `ColumnViewUserControl`: This is a base user control that implements the [IUserControl](#masterdetailcontroller-iusercontrol) discussed previously and has view-like behavior.

- `EnumPropertyEditor`: This is a subclass of the built-in EnumPropertyEditor, with the difference that it displays only the image.
   
   ![](Images/EnumPropertyEditorWin.png)

- `HyperLinkPropertyEditor`: This editor displays hyperlinks with mailto support.

   ![](Images/HyperLinkEditorWin.png)

- `LabelControlPropertyEditor`: This is an editor that renders a label.

  ![](Images/LabelWinEditor.png)

- `PdfViewerEditor`: This is a PDF viewer using the [pdfviewer](https://docs.devexpress.com/WindowsForms/15216/controls-and-libraries/pdf-viewer) component.

  ![](Images/PdfViewerWin.png)

- `PrintLayoutRichTextEditor`: This editor extends the built-in RichTextPropertyEditor, but uses the PrintLayout mode.

- `ProgressPropertyEditor`: This editor is used to display progress in various contexts.

  ![](Images/ProgressEditorWin.png)

##### `Services` Folder

Similarly to the agnostic [Services Folder](#services-folder) we keep all classes as thin as possible and we write the bussiness logig in extension methods.

##### `Features` Folder

This folder contains implementations specific to the solution.

- ##### Maps Subfolder

  This subfolder contains functionality related to maps.

  - **MapsViewController**: This controller overrides the platform-agnostic MapsViewController to further configure the state of map actions.
  
  - **WinMapsViewController**: This is an abstract controller that provides functionality used by its derived classes, `SalesMapsViewController` and `RouteMapsViewController`. It configures Map views for all objects that implement the `ISalesMapsMarker` (Customer, Product) and `IRouteMapsMarker` (Order, Employee) interfaces.
  
  ![Win Opportunity Maps](Images/WinOppurtunityMaps.png)
  
  ![Win Sales Map](Images/WinSalesMap.png)

- ##### Customers Subfolder

  This subfolder contains functionality related to customers.

  - **CustomerGridView**, **CustomerLayoutView**, and **CustomerStoreView**: These classes derive from the previously discussed `ColumnViewUserControl` and exhibit view-like behavior. They host custom GridControl variants, such as master-detail layouts.
  
  ![Win Master Detail Grid](Images/WinMasterDetailGrid.png)

- ##### Employees Subfolder

  This subfolder contains functionality related to employees.

  - **EmployeesLayoutView**: This is a descendant of `ColumnViewUserControl` that hosts a GridControl LayoutView.

  ![Win Employee Layout](Images/EmployeeWinLayout.png)

- ##### GridListEditor Subfolder

  This subfolder contains functionality related to default XAF GridListEditor.

  - `FontSizeController`: Uses the `FontSizeDelta` discussed in the agnostic method to increase the font size in a row cell of an AdvancedBanded Grid

  - `NewItemRowHandlingModeController`: Modifies how new object are handled when a dashboard master detailview (discussed in the agnostic module) objects are created.

- ##### Products Subfolder

  This subfolder contains functionality related to employees.
  
  - `ProductCardView`: This is a descendant of `ColumnViewUserControl` that hosts a GridControl LayoutView.

    ![](Images/WinProductLayout.png)

- ##### Quotes Subfolder

  This subfolder contains functionality related to opportunities.

  - `WinMapsController`, `PaletteEntriesController`: Configures the opportunities maps view.

     ![](Images/OpportunitiesMap.png) 

  - `FunnelFilterController`, `PropertyEditorController`: The first filters the Funnel chart when the FilterManager discussed in the agnostic section is executed. The later assigns a progress to the Pivot cell.

    ![](Images/OpportunitiesListView.png)