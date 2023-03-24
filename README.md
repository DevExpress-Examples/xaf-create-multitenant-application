<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1143380)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

# XAF - How to Implement a Multi-Tenant Application

This example demonstrates how to implement a multi-tenant XAF application in six most popular use-case scenarios.

All solutions in the example follow the **multitenancy** paradigm: they are configured to serve multiple **tenants** (groups of users). Each such group has access only to its own subset of data within the application.

> **Note**  
> The `DevExpress.ExpressApp.MultiTenancy` package is currently available as CTP. We do not recommend that you use it in production projects. We are also aware that a few multi-tenancy scenarios are not yet implemented in this example. Examples include different module structure for each company or different business classes. We will extend this example when we prepare appropriate solutions. 
>
> At this time, we do offer technical support for this example. If you require assistance, post a ticket with a detailed description of your scenario to the DevExpress [Support Center](https://supportcenter.devexpress.com/). Our R&D team will research incoming tickets and will publish solutions for most popular scenarios as part of this example.


## Included Solutions

The following solutions are included:

1. _LogInFirst_ - Implements a scenario where multiple tenants can be associated with multiple users. Every tenant has its own database connection. A user selects the tenant after the they log in with the standard login form. The application then uses the tenant's database connection.

2. _PredefinedTenant_ - Same as _LogInFirst_, but a user is strictly bound to a tenant. The tenant is assigned to the user by the admin. 

3. _LogInFirstOneDataBase_- Multiple tenants can be associated with multiple users. A tenant is selected after login. All application data comes from a single database. In the database, every business class extend the `Tenant` class, and thus includes an `Owner` field. A user can only access objects where `Owner` is empty or corresponds to the specified tenant.

4. _PredefinedTenantOneDataBase_ - Same as _LogInFirstOneDataBase_ but a user is strictly bound to a tenant. The tenant is assigned to the user by the admin.

5. _TenantFirst_ - A user first selects a tenant from a list. After that, the application runs as a regular XAF application but uses the connection string of the selected tenant.

6. _TenantFirstOneDataBase_ - Data is stored in a single database. All business classes extend the `Tenant` class and thus include an `Owner` field. A user selects a tenant from a list at startup. After that, the user can only access objects where `Owner` is empty or corresponds to the specified tenant.

## Run the Example Solutions

Follow the steps below to familiarize yourself with the solutions included into the example:

1. Choose one of the applications included into the example, launch the application and log in as Admin with empty an empty company parameter.

![image](https://user-images.githubusercontent.com/39731874/214006275-2675b9a2-64d6-4d9f-845b-03737256a33f.png)


2. Create several companies. In the scenario that requires separate database connection, specify connection strings:

```
Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Company1
Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Company2
```

![image](https://user-images.githubusercontent.com/39731874/214006416-b8ea9832-0e7e-4ab0-bc1a-a0c17116906a.png)

3. Create users and assign them the _Default_ role. Set the _Tenant_ property or populate the Tenants collection if these settings are available in the selected example.

4. Click _LogOff_ and restart the application.

5. If you need to set up separate model differences for each company, navigate to _ModelDifferences_ -> _Shared Model Differences(CompanyName)_ -> _(Default Language)_ and modify the _Xml_ property. You can generate the required XML markup at design time and copy-and-paste it to the editor.

   **See also:** [Enable the Administrative UI to manage End-User Settings in the Database](https://docs.devexpress.com/eXpressAppFramework/113704/ui-construction/application-model-ui-settings-storage/application-model-storages/enable-the-administrative-ui-for-managing-users-model-differences)

![image](https://user-images.githubusercontent.com/39731874/214009179-5d207892-94e2-449b-ba4e-439052f27505.png)


## Your Feedback Counts

Please take a moment to complete a short survey: https://www.devexpress.com/products/net/application_framework/survey.xml
