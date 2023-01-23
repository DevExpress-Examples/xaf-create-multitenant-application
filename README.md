<!-- default badges list -->
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

This application allows to create new companies with its own data, security and model settings without recompilation.

Launch the application, enter as Admin with empty company.



2. Создайте две компании с двумя разными ConnectionString

Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Company1
Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Company2



3. Нажмите Логофф и в комбике выберите FirstCompany, юзер - Admin.




4. Добавьте новый Employee, настройте сесурити рули так чтоб запретить доступ к классу Position.

5.  Нажмите Логофф и в комбике выберите SecondCompany, юзер - Admin. Настройте сесурити рули так чтоб запретить доступ к классу Payment.

6. Если нужно настроить отдельные модельные дифы для каждой компании, то открываем ModelDifferences->Shared Model Differences->(Default Language) и правим Xml. Более удобного метода нет, xml можно в дизайн тайме сгенерить. Ссылка на доку: Enable the Administrative UI to manage End-User Settings in the Database



В итоге у нас есть одна сервисная база с компаниями и две отдельные базы со своими настройками сесурити и модели под каждую компанию. Из минусов данного подхода - нет возможности делать шареные данные так как базы отдельные. Можно обойтийсь без сервисной базы и список компаний получать из любого удобного вам источника.
