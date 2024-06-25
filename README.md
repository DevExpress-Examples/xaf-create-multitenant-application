<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/592224624/23.1.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1143380)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->

# XAF - How to Create a Multi-Tenant Application

This example creates multi-tenant XAF applications for six use-case scenarios.

All solutions in this example follow **multitenancy** principles and are configured to serve multiple **tenants** (groups of users). Each “user”  group has access to a subset of data within the application.

> **Note**  
> The `DevExpress.ExpressApp.MultiTenancy` package is currently available as a [community technology preview (CTP)](https://www.devexpress.com/aboutus/pre-release.xml). **We do not recommend its use for production/in production environments**. We are also aware that a few multitenancy scenarios have yet to be implemented. Examples include different module structures for each company or different business classes. We will extend this example as capabilities are made available.
>
> At present, we are unable to offer technical support services for this example. For suggestions, issues, or other feedback, please post a ticket with a detailed description of your usage scenario (via the DevExpress [Support Center](https://supportcenter.devexpress.com/)). We’ll review all incoming tickets and will publish solutions for popular requirements (as they relate to this specific example).
>
> If you’re considering multi-tenancy, please also take a moment to complete the following survey: [https://www.devexpress.com/products/net/application_framework/survey.xml](https://www.devexpress.com/products/net/application_framework/survey.xml). For more information, [refer to the FAQ section](/README.md#frequently-asked-questions) at the bottom of this readme.

## Included Solutions

The following solutions are included in this example:

1. _LogInFirst_ - multiple tenants can be associated with multiple users. Every tenant has its own database connection. A user selects the tenant once they log in with the standard login form. The application then uses the tenant's database connection.

2. _PredefinedTenant_ - same as _LogInFirst_, but a user is strictly bound to a tenant. The tenant is assigned to the user by the admin.

3. _LogInFirstOneDataBase_- multiple tenants can be associated with multiple users. A tenant is selected after login. All application data is loaded from a single database. In the database, every business class extends the `Tenant` class and includes an `Owner` field. A user can only access objects where `Owner` is empty or corresponds to the specified tenant.

4. _PredefinedTenantOneDataBase_ - same as _LogInFirstOneDataBase_ but a user is strictly bound to a tenant. The tenant is assigned to the user by the admin.

5. _TenantFirst_ - a user selects a tenant from a list. Once selected, the application runs as a standard XAF application but uses the connection string of the selected tenant.

6. _TenantFirstOneDataBase_ - data is stored in a single database. All business classes extend the `Tenant` class and include an `Owner` field. The user selects a tenant from a list (at startup). Once selected, the user can only access objects where `Owner` is empty or corresponds to the specified tenant.

## Run the Example Solutions

Follow the steps below to familiarize yourself with solutions included in this example:

1. Select one of the applications included in the example, launch the application, and log in as Admin with an empty company parameter.

![image](https://user-images.githubusercontent.com/39731874/214006275-2675b9a2-64d6-4d9f-845b-03737256a33f.png)

2. Create multiple companies. If a scenario requires a separate database connection, specify appropriate connection strings:

```
Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Company1
Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Company2
```

![image](https://user-images.githubusercontent.com/39731874/214006416-b8ea9832-0e7e-4ab0-bc1a-a0c17116906a.png)

3. Create users and assign them the _Default_ role. Set the _Tenant_ property or populate the Tenants collection if these settings are available in the selected example.

4. Click _LogOff_ and restart the application.

5. If you need to set up separate model differences for each company, navigate to _ModelDifferences_ -> _Shared Model Differences(CompanyName)_ -> _(Default Language)_ and modify the _Xml_ property. You can generate the required XML markup at design time and copy/paste it to the editor. For more information, refer to [Enable the Administrative UI to manage End-User Settings in the Database](https://docs.devexpress.com/eXpressAppFramework/113704/ui-construction/application-model-ui-settings-storage/application-model-storages/enable-the-administrative-ui-for-managing-users-model-differences).

![image](https://user-images.githubusercontent.com/39731874/214009179-5d207892-94e2-449b-ba4e-439052f27505.png)



## Frequently Asked Questions

For following questions and answers, the phrase “Multi-Tenancy Module” refers to the `DevExpress.ExpressApp.MultiTenancy` library.  

 
### Have you set a release date for the DevExpress Multi-Tenancy Module?

Release dates are subject to change. Progress depends on customer interest and user feedback.

- **Early Access Preview (EAP)** – v23.1.1 released [in March 2023](https://community.devexpress.com/blogs/xaf/archive/2023/03/22/xaf-231-eap-runtime-customization-no-code-and-usability-enhancements-to-blazor-ui.aspx).
- **Community Technology Preview (CTP)** -  v23.1.3 scheduled for June 2023.
- **Production Version (RTM)** – v23.2.3 scheduled for December 2023. 


### When do you expect to publish an XPO version of this module/example?

Our current goal is to collect usage scenario feedback that is not ORM-dependent. This feedback may lead to significant changes to our codebase. As such, we wanted to optimize our development process by focusing on a single ORM (we chose EF Core first). Neither ORM will offer significant advantages for multi-tenant usage scenarios. The only reason we haven't added XPO support is resource optimization.

If multi-tenancy is of interest to you, please test our EF Core example. You can run the executable file and review available UI screens and associated flows. Compare our implementation to your project requirements. Use the following resources to share feedback with us:

- **Create a Support Ticket**: https://devexpress.com/ask
- **Fill out our Survey**: https://www.devexpress.com/products/net/application_framework/survey.xml
 
We hope to add full XPO support in our v23.2 (Production Version).

### How can XPO users implement multi-tenant apps with XAF today?

Before you read our instructions below, please note that we expect to extend our Multi-Tenancy Module with XPO support in our v23.2 release cycle (December 2023).

If you need to develop a multi-tenant application before we release v23.2, refer to the following examples:

 - [E1344 - How to Change Connection to the Database at Runtime from the Login Form](https://github.com/DevExpress-Examples/XAF_how-to-change-connection-to-the-database-at-runtime-e1344) 
 - [E4045 - How to separate employees data in different departments using security permissions in XPO](https://github.com/DevExpress-Examples/XAF_how-to-separate-employees-data-in-different-departments-using-security-permissions-in-xpo-e4045/tree/18.2.2+)
 
Many of our customers used code from these examples to successfully build production multi-tenant XAF apps with XPO and EF Core.

 
### What is the processing time for bug reports and suggestions related to the Multi-Tenancy Module and this example?

Thanks for testing our example/module and sharing your feedback. We will do our best to fix known issues and implement high-priority usage scenarios before our official release in December 2023.

Since this module is in a Preview release stage (EAP or CTP), we cannot guarantee the same level of support as we provide for officially released products. We may also choose to modify/remove specific functionality. Accordingly, you should not use this product for production apps/in production environments.

For more information on pre-release versions of DevExpress products, see https://www.devexpress.com/aboutus/pre-release.xml.


### Can a part of a user identifier specify the tenant (instead of separate tenant selection)?

We are aware of the requirement to use email address as a login (where the domain sets a tenant/company). Our first production release (v23.2) should implement this capability.

The Company/Branch/Department selector in the log in form also makes sense and is secure. One possible use case is an application that needs to list branches within the same company. A company branch then serves as a tenant, and all tenants are likely to be connected to the same company database.


### Can I implement my own versions of ORM-dependent classes in this example/module (Tenant, MultiTenancyPermissionPolicyUserWithTenants or others)?

Our core multi-tenancy code is not ORM-dependent. It relies on universal interfaces such as `ITenant` or `IOwner`. You can review the code in the following namespace: `DevExpress.ExpressApp.MultiTenancy.Interfaces`.

Persistent/business classes in this example are all ORM-dependent (see classes like `Tenant` or `MultiTenancyPermissionPolicyUserWithTenants`). This means that you cannot extend these classes so that they work with a different ORM. EF Core class descendants cannot work with XPO, and vice versa.

You can certainly implement custom persistent classes specifically for XPO or EF Core. You will need to familiarize yourself with Multi-Tenancy Module source code and should have a good understanding of XAF core concepts (at this time, we do offer technical support for this example/module). For additional information, please review “Prerequisites and Advanced Customization Tips” in the following article: https://www.devexpress.com/products/net/application_framework/xaf-considerations-for-newcomers.xml.

 
### How will you license the Multi-Tenancy Module?

We have not made final decisions in this regard, but we expect to introduce unique licensing terms - different from other XAF modules. The complexity of SaaS applications and associated usage scenarios creates higher implementation/maintenance/support costs. The use of our Multi-Tenancy Module will likely require client access licenses, similar to [licensing and distribution of our Report Server product](https://docs.devexpress.com/ReportServer/14612/license-and-distribution). A single DevExpress Universal subscription may include a limited number of Client Access Licenses (CALs).

We are also considering other licensing models.

The following articles include a good overview of general implementation complexity for multi-tenant applications:

- https://learn.microsoft.com/en-us/azure/architecture/guide/multitenant/overview
- https://learn.microsoft.com/en-us/azure/azure-sql/database/saas-tenancy-app-design-patterns
- https://learn.microsoft.com/en-us/ef/core/miscellaneous/multitenancy
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=xaf-create-multitenant-application&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=xaf-create-multitenant-application&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
