# Solution Overview

## Introduction

Welcome to the OutlookInspired XAF application—a modern iteration of our original WinForms-based solution. This cross-platform ERP/LOB application is tailored for both Windows and Blazor, with plans for further platform expansion. It serves as the central data management hub for DevAv Company, overseeing various business entities such as Employees, Products, Orders, Quotes, Customers, and Stores.

## Core Objectives

- **Data Management**: This comprehensive tool is designed for managing an array of business entities, aiming to streamline DevAv Company's ordering processes and customer relations.

- **Employee Evaluations**: A built-in scheduler is used for systematically scheduling employee evaluations, ensuring regular performance assessments.

## Architecture

### Domain Diagram

The domain architecture is depicted in the diagram below:

![](Images/DomainModel.png)

### Project Structure

The solution is structured into five independent projects.

![Project Structure Diagram](Images/Solution.png)

#### OutlookInspired.Module

Next, we describe the functionality found in each project folder.

##### Services folder

This is the most important folder as it is use to store our bussiness logic. We keep all other classes as think as possible and we use this folder to store the logic. For example if a method uses XafApplication you will find it in Services/XafApplicationExtensions e.g.

```cs
public static IObjectSpace NewObjectSpace(this XafApplication application) 
    => application.CreateObjectSpace(typeof(OutlookInspiredBaseObject));
```

if it uses an `IObjectSpace` it will live in the Services/ObjectSpaceExtensions e.g

```cs
public static TUser CurrentUser<TUser>(this IObjectSpace objectSpace) where TUser:ISecurityUser 
    => objectSpace.GetObjectByKey<TUser>(objectSpace.ServiceProvider.GetRequiredService<ISecurityStrategyBase>().UserId);
```

##### Attributes Folder

In the `Attributes` folder, you can find attribute declarations whose implementations reside in another library.

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

###### Appearance Subfolder

The following attributes are located here:

- `DeactivateActionAttribute`: This is an extension of the [Conditional Appearance module](https://docs.devexpress.com/eXpressAppFramework/113286/conditional-appearance) used to deactivate actions.

```cs
public class DeactivateActionAttribute : AppearanceAttribute {
    public DeactivateActionAttribute(params string[] actions) : 
        base($"Deactivate {string.Join(" ", actions)}", DevExpress.ExpressApp.ConditionalAppearance.AppearanceItemType.Action, "1=1") {
        Visibility = ViewItemVisibility.Hide;
        TargetItems = string.Join(";", actions);
    }
}

```

Similarly, we derive from this attribute to create the other attributes found in the same folder (`ForbidCRUDAttribute`, `ForbidDeleteAttribute`, `ForbidDeleteAttribute`).

###### Validation Subfolder
In this folder, you can find attributes that extend the [XAF Validation module](https://docs.devexpress.com/eXpressAppFramework/113684/validation-module). Available are `EmailAddressAttribute`, `PhoneAttribute`, `UrlAttribute`, `ZipCodeAttribute`. Below is the latter, and the rest are implemented similarly.

```cs
public class ZipCodeAttribute : RuleRegularExpressionAttribute {
    public ZipCodeAttribute() : base(@"^[0-9][0-9][0-9][0-9][0-9]$") {
        CustomMessageTemplate = "Not a valid ZIP code.";
    }
}
```
##### Controllers Folder
In this folder we have controllers with no dependecies, they could reside in a different library if we wish.
* The `HideToolBarController`: extends the XAF IModelListView interface with a HideToolBar attribte so we can hide the nested listviews toolbare. 
* The `SplitterPositionController`: extends the XAF model withe a `RelativePosition` so we can configure the splitter position.
##### Features Folder
