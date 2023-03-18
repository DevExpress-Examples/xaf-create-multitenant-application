<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1143380)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

# XAF - How to Implement a Multi-Tenant Application

This example demonstrates how to implement a multi-tenant XAF application in six most popular use-case scenarios.

All solutions in the example follow the **multitenancy** paradigm, which means that they are configured to serve multiple **tenants** (groups of users). Each such group only has access to its own subset of data within the application.

> **Note**  
> The `DevExpress.ExpressApp.MultiTenancy` package is currently at the CTP stage and is not recommended for use in production. We are aware of other scenarios that are not yet featured in this example (different modules structure for each company, different business classes, and so on) and will update the example after we prepare an appropriate solution. 
>
> At the current stage, we do provide support service for this example. If you need assistance with it, create a new ticket with a detailed description of your scenario in the [Support Center](https://supportcenter.devexpress.com/). Our R&D team will research your requests and publish solutions for the most popular use case scenarios as a pat of this example.


## Included Solutions

The following solutions are included:

1. _LogInFirst_ - Implements a scenario where multiple tenants can be associated with multiple users. Every tenant has its own database connection. A user selects the tenant after the they log in with the standard login form. The application then uses the tenant's database connection.

2. _PredefinedTenant_ - Same as _LogInFirst_, but a user is strictly bound to a tenant. A user must select their tenant in the login form. 

3. _LogInFirstOneDataBase_- Multiple tenants can be associated with multiple users. The solution stores data in a single database. All business objects extend a `Tenant` class that has an `Owner` field. A tenant is selected after login. After that all data is filtered by the `Owner` field so a user can only access their tenant's objects or objects with no `Owner` specified.

4. _PredefinedTenantOneDataBase_ - Same as _LogInFirstOneDataBase_ bun a use is strictly bound to a tenant ad must select their tenant in the login form.

5. _TenantFirst_ - A user first selects a tenant from a list. After that, the application runs as a regular XAF application but uses the connection string of the selected tenant.

6. _TenantFirstOneDataBase_ - Similarly to the _LogInFirstOneDataBase_ solution, data is stored in a single database and all business objects extend a `Tenant` class with an `Owner` field. A user selects their tenant in the login form and can only access their tenant's objects or objects with no `Owner` specified.

## Run the Example

Follow the steps bellow to familiarize yourself with the solution's functionality:

1. Choose one of the applications included into the example, launch the application and log in as Admin with empty an empty company parameter.

![image](https://user-images.githubusercontent.com/39731874/214006275-2675b9a2-64d6-4d9f-845b-03737256a33f.png)


2. Create several companies. In the scenario that requires separate database connection, specify connection strings:

```
Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Company1
Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Company2
```

![image](https://user-images.githubusercontent.com/39731874/214006416-b8ea9832-0e7e-4ab0-bc1a-a0c17116906a.png)

3. Create users and assign them the _Default_ role. Set the _Tenant_ property or fill the Tenants collection if these settings are available in the selected example.

4. Click _LogOff_ and restart the application.

5. If you need to set up separate model differences for each company, navigate to _ModelDifferences_ -> _Shared Model Differences(CompanyName)_ -> _(Default Language)_ and modify the _Xml_ property. You can generate the required XML markup at design time and copy-and-past it to the editor.

   **See also:** [Enable the Administrative UI to manage End-User Settings in the Database](https://docs.devexpress.com/eXpressAppFramework/113704/ui-construction/application-model-ui-settings-storage/application-model-storages/enable-the-administrative-ui-for-managing-users-model-differences)

![image](https://user-images.githubusercontent.com/39731874/214009179-5d207892-94e2-449b-ba4e-439052f27505.png)


