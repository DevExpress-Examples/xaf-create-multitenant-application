<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1143380)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

# XAF - How to Implement a Multi-Tenant Application

This example demonstrates how to implement a multi-tenant XAF application in six most popular use-case scenarios. 

> **Note**  
> The `DevExpress.ExpressApp.MultiTenancy` package is currently at the CTP stage and is not recommended for use in production. We are aware of other scenarios that are not yet featured in this example (different modules structure for each company, different business classes, and so on) and will update the example after we prepare an appropriate solution. 
>
> At the current stage we do provide support service for this example. If you need an assistance with it, create a new ticket with a detailed description of your scenario in the [Support Center](https://supportcenter.devexpress.com/). Our R&D team will research your requests and publish solutions for the most popular use case scenarios as a pat of this example.

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


