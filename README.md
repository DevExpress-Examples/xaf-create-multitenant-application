<!-- default badges list -->
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

This application allows to create new companies with their own data, security and model settings without recompilation.

1. Launch the application, enter as Admin with empty company.

![image](https://user-images.githubusercontent.com/39731874/214006275-2675b9a2-64d6-4d9f-845b-03737256a33f.png)


2. Create two companies with different connection strings

```
Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Company1
Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Company2
```

![image](https://user-images.githubusercontent.com/39731874/214006416-b8ea9832-0e7e-4ab0-bc1a-a0c17116906a.png)


3. Click LogOff and select FirstCompany in the Company Name combobox, enter as Admin.

![image](https://user-images.githubusercontent.com/39731874/214006706-1b2280b1-88a1-4191-8794-864a806e1b8a.png)



4. Create a Employee, setup security rules to prohibit access to the Position class.

5. Click LogOff and select SecondCompany in the Company Name combobox, enter as Admin. Setup sesurity rules to prohibit access to the Payment class.


6. If you need to setup separate model differences for each company, open ModelDifferences -> Shared Model Differences -> (Default Language) and modify the Xml property. You can generate required xml in design-time and just past it here. See also: [Enable the Administrative UI to manage End-User Settings in the Database](https://docs.devexpress.com/eXpressAppFramework/113704/ui-construction/application-model-ui-settings-storage/application-model-storages/enable-the-administrative-ui-for-managing-users-model-differences)

![image](https://user-images.githubusercontent.com/39731874/214009179-5d207892-94e2-449b-ba4e-439052f27505.png)


As a result we have an application with one service database with companyes and two separate databases for each company with their own security and model settings. You  can get companies without the service database. In this case modify the CompanyNamesHelper class and return companies for your source. Disadvantage of this approach is that you can not share data between companies.

