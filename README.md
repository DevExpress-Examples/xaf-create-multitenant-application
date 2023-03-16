<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1143380)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

-----
This example contains six basic and most requested scenarios. The DevExpress.ExpressApp.MultiTenancy package is in CTP mode, so we do not recomend to use it in production. We take in mind other requests(different modules structure for each company, different business classes, etc...) and will update the example when we will make an appropriate soltion. At the current stage we do provide support for this example. If you need an assistance with it, create a new ticket with detailed description of your scenrio in Support Center. We will cosider most popular requests and add them to the example. 

-----

1. Launch the application, enter as Admin with empty company.

![image](https://user-images.githubusercontent.com/39731874/214006275-2675b9a2-64d6-4d9f-845b-03737256a33f.png)


2. Create companies(with connection strings for multiple database case).
You can use the following connections strings:

```
Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Company1
Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Company2
```

![image](https://user-images.githubusercontent.com/39731874/214006416-b8ea9832-0e7e-4ab0-bc1a-a0c17116906a.png)

3. Create users and add them to the Default role. Set the Tenant property or fill the Tenants collection if they available in the selected example.

4. Click LogOff and restart the application.

5. If you need to setup separate model differences for each company, open ModelDifferences -> Shared Model Differences(CompanyName) -> (Default Language) and modify the Xml property. You can generate required xml in design-time and just past it here. See also: [Enable the Administrative UI to manage End-User Settings in the Database](https://docs.devexpress.com/eXpressAppFramework/113704/ui-construction/application-model-ui-settings-storage/application-model-storages/enable-the-administrative-ui-for-managing-users-model-differences)

![image](https://user-images.githubusercontent.com/39731874/214009179-5d207892-94e2-449b-ba4e-439052f27505.png)


