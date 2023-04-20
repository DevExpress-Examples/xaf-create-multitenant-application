<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1143380)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

# XAF - How to Implement a Multi-Tenant Application

This example demonstrates how to implement a multi-tenant XAF application in six most popular use-case scenarios.

All solutions in the example follow the **multitenancy** paradigm: they are configured to serve multiple **tenants** (groups of users). Each such group has access only to its own subset of data within the application.

> **Note**  
> The `DevExpress.ExpressApp.MultiTenancy` package is currently available as a [community technology preview (CTP)](https://www.devexpress.com/aboutus/pre-release.xml). **We do not recommend that you use it in production projects**. We are also aware that a few multitenancy scenarios are not yet implemented in this example. Examples include different module structure for each company or different business classes. We will extend this example when we prepare appropriate solutions.
>
> At this time, we do offer technical support for this example. For suggestions, issues and other feedback, please post a ticket with a detailed description of your scenario to the DevExpress [Support Center](https://supportcenter.devexpress.com/). Our R&D team will research incoming tickets and will publish solutions for most popular scenarios as part of this example. Please also take a moment to complete a short survey on your multi-tenancy requirements: **https://www.devexpress.com/products/net/application_framework/survey.xml**. For more information, [refer to the FAQ section at the bottom](/README.md#frequently-asked-questions).


## Included Solutions

The following solutions are included:

1. _LogInFirst_ - Implements a scenario where multiple tenants can be associated with multiple users. Every tenant has its own database connection. A user selects the tenant after the they log in with the standard login form. The application then uses the tenant's database connection.

2. _PredefinedTenant_ - Same as _LogInFirst_, but a user is strictly bound to a tenant. The tenant is assigned to the user by the admin. 

3. _LogInFirstOneDataBase_- Multiple tenants can be associated with multiple users. A tenant is selected after login. All application data comes from a single database. In the database, every business class extends the `Tenant` class, and thus includes an `Owner` field. A user can only access objects where `Owner` is empty or corresponds to the specified tenant.

4. _PredefinedTenantOneDataBase_ - Same as _LogInFirstOneDataBase_ but a user is strictly bound to a tenant. The tenant is assigned to the user by the admin.

5. _TenantFirst_ - A user first selects a tenant from a list. After that, the application runs as a regular XAF application but uses the connection string of the selected tenant.

6. _TenantFirstOneDataBase_ - Data is stored in a single database. All business classes extend the `Tenant` class and thus include an `Owner` field. A user selects a tenant from a list at startup. After that, the user can only access objects where `Owner` is empty or corresponds to the specified tenant.

## Run the Example Solutions

Follow the steps below to familiarize yourself with the solutions included in the example:

1. Choose one of the applications included in the example, launch the application, and log in as Admin with an empty company parameter.

![image](https://user-images.githubusercontent.com/39731874/214006275-2675b9a2-64d6-4d9f-845b-03737256a33f.png)


2. Create several companies. In the scenario that requires a separate database connection, specify connection strings:

```
Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Company1
Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Company2
```

![image](https://user-images.githubusercontent.com/39731874/214006416-b8ea9832-0e7e-4ab0-bc1a-a0c17116906a.png)

3. Create users and assign them the _Default_ role. Set the _Tenant_ property or populate the Tenants collection if these settings are available in the selected example.

4. Click _LogOff_ and restart the application.

5. If you need to set up separate model differences for each company, navigate to _ModelDifferences_ -> _Shared Model Differences(CompanyName)_ -> _(Default Language)_ and modify the _Xml_ property. You can generate the required XML markup at design time and copy-and-paste it to the editor. For more information, refer to [Enable the Administrative UI to manage End-User Settings in the Database](https://docs.devexpress.com/eXpressAppFramework/113704/ui-construction/application-model-ui-settings-storage/application-model-storages/enable-the-administrative-ui-for-managing-users-model-differences).

![image](https://user-images.githubusercontent.com/39731874/214009179-5d207892-94e2-449b-ba4e-439052f27505.png)



## Frequently Asked Questions

In the following questions and answers, the phrase “Multi-Tenancy Module” refers to the `DevExpress.ExpressApp.MultiTenancy` library.  

 
### Did you set a release timeline for the Multi-Tenancy Module?

We aim for release dates listed below. Note that all future dates are subject to change. Our progress greatly depends on developer interest and user feedback.

- **Early Access Preview (EAP)** – v23.1.1 released [in March 2023](https://community.devexpress.com/blogs/xaf/archive/2023/03/22/xaf-231-eap-runtime-customization-no-code-and-usability-enhancements-to-blazor-ui.aspx).
- **Community Technology Preview (CTP)** -  v23.1.3 scheduled for June 2023.
- **Production Version (RTM)** – v23.2.3 scheduled for December 2023. 


### When do you expect to publish an XPO version of this module/example?

Our current goal is to collect usage scenario feedback that is not ORM-dependent. That feedback may lead to significant changes in our codebase, so we wanted to optimize our development process by focusing on a single ORM (as such, we opted for EF Core first). We expect that neither ORM will offer significant advantages in multi-tenant usage scenarios. The only reason why we haven't yet included XPO support is development resource optimization during the Preview stage.

We plan to work on an XPO version of this example/module when we are content with the early feedback we received from customers. If multi-tenancy is of interest to you, we would appreciate if you test our EF Core example. You can run the executable file, and review available UI screens and their flow. Compare our implementation to your project requirements. Use the following resources to share your thoughts with us:

- **Create a Support Ticket**: https://devexpress.com/ask
- **Fill out our Survey**: https://www.devexpress.com/products/net/application_framework/survey.xml
 
We do aim to implement full XPO support in v23.2 (Production Version).

### How can XPO users implement multi-tenant apps with XAF today?

Before you read our instructions below, please note that we expect to extend our Multi-Tenancy Module with XPO support in v23.2 (December 2023).

If you need to develop a multi-tenant application before we release v23.2, refer to the following examples: 

 - [E1344 - How to Change Connection to the Database at Runtime from the Login Form](https://github.com/DevExpress-Examples/XAF_how-to-change-connection-to-the-database-at-runtime-e1344) 
 - [E4045 - How to separate employees data in different departments using security permissions in XPO](https://github.com/DevExpress-Examples/XAF_how-to-separate-employees-data-in-different-departments-using-security-permissions-in-xpo-e4045/tree/18.2.2+)
 
 Many of our customers used code from these examples to successfully build production multi-tenant XAF apps with XPO and EF Core.

 
### What is the processing time for bug reports and suggestions related to the Multi-Tenancy Module and this example?

We appreciate that you took the time to test our example/module and shared your feedback. We will do our best to fix reported issues and implement critical usage scenarios before the official release in December 2023.

While the module is in its Preview stages (EAP or CTP), we cannot guarantee the same level of support as we provide for officially released products. We will process each case individually. Note that response times may be slower than usual and we may not provide immediate solutions. We may also choose to significantly change or remove any functionality. As such, we recommend that you do not use Preview versions of our products in production code.

For more information on pre-release versions of DevExpress products, see https://www.devexpress.com/aboutus/pre-release.xml.


### Can a part of a user identifier specify the tenant (instead of separate tenant selection)?

We are aware of the requirement to use email address as a login, while its domain part sets a tenant/company. Our first production release (v23.2) should implement this capability.

The Company/Branch/Department selector on the logon form also makes sense and is secure. One possible use case is an application that needs to list branches within the same company. A company branch then serves as a tenant, and all tenants are likely to be connected to the same company database. 


### Can I implement my own versions of ORM-dependent classes in this example/module: Tenant, MultiTenancyPermissionPolicyUserWithTenants, and others?

Our core multi-tenancy code is not ORM-dependent. It relies on universal interfaces such as `ITenant` or `IOwner`. You can review the code in the following namespace: `DevExpress.ExpressApp.MultiTenancy.Interfaces`.

Persistent/business classes in this example are all ORM-dependent (see classes like `Tenant` or `MultiTenancyPermissionPolicyUserWithTenants`). This means that in you cannot extend these classes so that they work with a different ORM. EF Core class descendants cannot work with XPO, and vice versa.

You can certainly implement custom persistent classes specifically for XPO or EF Core. You will need to familiarize yourself with Multi-Tenancy Module sources and should have a good understanding of XAF core concepts (at this time, we do offer technical support for this example/module). For additional information, please review “Prerequisites and Advanced Customization Tips” in the following article: https://www.devexpress.com/products/net/application_framework/xaf-considerations-for-newcomers.xml.

 
### How will you license the Multi-Tenancy Module?

We have not made final decisions yet, but we expect to introduce unique licensing terms - different from other XAF modules. The complexity of SaaS applications and their usage scenarios result in high costs of implementation, maintenance, and support. The use of Multi-Tenancy Module in your application will likely require client access licenses, similar to [licensing and distribution of our Report Server product](https://docs.devexpress.com/ReportServer/14612/license-and-distribution). A single DevExpress Universal subscription may include a limited number of Client Access Licenses (CALs). 

We are also considering other licensing models. If you want to discuss licensing terms in context of applications you plan to build, don’t hesitate to contact us.

The following articles include a good overview of the required technical expertise and general implementation complexity for multi-tenant applications:
- https://learn.microsoft.com/en-us/azure/architecture/guide/multitenant/overview
- https://learn.microsoft.com/en-us/azure/azure-sql/database/saas-tenancy-app-design-patterns
- https://learn.microsoft.com/en-us/ef/core/miscellaneous/multitenancy
