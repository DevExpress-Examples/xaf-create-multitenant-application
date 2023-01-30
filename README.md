<!-- default badges list -->
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

-----
This example just a demonstration how this specific task can be implemented. It was created based on a lot of customer requests. We take in mind other requests(different modules structure for each company, different business classes, etc...) and will update the example when we will make an appropriate soltion. We do not support this example. If you need an assistance with it, create a new ticket with detailed description fo your scenrio in Support Center. We will cosider most popular requests and will add them to the example. 

-----

This example demonstrates how to create an application that allow to create new companies with their own data, security and model settings without recompilation. A new database is used for each company.

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


4. Create an Employee.

5. Click LogOff and select SecondCompany in the Company Name combobox, enter as Admin. Create an Employee.

6. If you need to setup separate model differences for each company, open ModelDifferences -> Shared Model Differences(CompanyName) -> (Default Language) and modify the Xml property. You can generate required xml in design-time and just past it here. See also: [Enable the Administrative UI to manage End-User Settings in the Database](https://docs.devexpress.com/eXpressAppFramework/113704/ui-construction/application-model-ui-settings-storage/application-model-storages/enable-the-administrative-ui-for-managing-users-model-differences)

![image](https://user-images.githubusercontent.com/39731874/214009179-5d207892-94e2-449b-ba4e-439052f27505.png)

As a result we have an application with one service database with companies and two separate databases for each company with their own data, security and model settings. Disadvantage of this approach is that you can not share data between companies.


### Customizations

1) You can get companies without the service database. In this case modify the CompanyNamesHelper class and return companies from your source.

2) Uncomment the
CreateCustomLogonWindowControllers += SAASExample1WindowsFormsApplication_CreateCustomLogonWindowControllers;
string if you want to select company before user.

### Used Approaches
1) [Application Builder](https://docs.devexpress.com/eXpressAppFramework/403980/application-shell-and-base-infrastructure/application-solution-components/integrate-application-builders-into-existing-applications).
2) CustomLogonParameters with the Company property - CustomLogonParametersForStandardAuthentication class.
3) DbContext with a dymamic connection string set on configiring - SAASExample1EFCoreDbContext class.
4) SelectCompanyController used to hide users info and the Login button while the company is not selected.
5) Set of predefined navigation, type and other permissions in the Updater.cs file.
6) Services that provides company names and connection strings: CompanyNamesHelper, ConnectionStringProvider, ConfigurationConnectionStringProvider.
7) xafml files used to hide extra navigation items for Admin: ServiceModel.xafml, CompaniesModel.xafml.
8) CreateCustomUserModelDifferenceStore handler that add a company specific model layer using the AddExtraDiffStore method.

### Articles and examples used in this example:

https://github.com/DevExpress-Examples/XAF_how-to-manage-users-register-a-new-user-restore-a-password-etc-from-the-logon-e4037

https://docs.devexpress.com/eXpressAppFramework/113698/ui-construction/application-model-ui-settings-storage/application-model-storages/store-the-application-model-differences-in-the-database

https://docs.devexpress.com/eXpressAppFramework/113704/ui-construction/application-model-ui-settings-storage/application-model-storages/enable-the-administrative-ui-for-managing-users-model-differences

https://docs.devexpress.com/eXpressAppFramework/403980/application-shell-and-base-infrastructure/application-solution-components/integrate-application-builders-into-existing-applications


