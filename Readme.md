<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/592224624/23.2.2%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1143380)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

# XAF - How to Create a Multi-Tenant Application

The Multi-Tenancy Module was first introduced in June 2023 as a Community Technology Preview (CTP). This module helps developers create multi-tenant or SaaS-ready XAF Blazor and WinForms applications (.NET 6+) that target a single host database and one database per tenant. To incorporate this capability, you simply need to write a few declarative lines of code instead of writing hundreds of lines of code.

Our v23.2 major update marks the first official release of the DevExpress Multi-Tenancy Module. The first release supports straightforward CRUD usage scenarios and includes the following features:

 - XPO ORM support (v23.1 supported only EF Core).
 - Authentication: Log in with an email / OAuth2 account (like Microsoft Entra ID or Google), and a password (the domain automatically resolves the tenant and its storage).
 - Tenant Isolation: Multi-tenant app with multiple databases (a database per tenant).
 - Database Creation: The application automatically creates a tenant database and schema at runtime (if the database does not exist).

Please take a moment to [complete a short survey](https://www.devexpress.com/products/net/application_framework/survey.xml) about your multi-tenancy requirements﻿.

[Documentation](https://docs.devexpress.com/eXpressAppFramework/404436/multitenancy-support?v=23.2) | [Getting Started](https://docs.devexpress.com/eXpressAppFramework/404669/multitenancy/create-new-multitenant-application?v=23.2) | [Best Practices and Limitations](https://docs.devexpress.com/eXpressAppFramework/404436/multitenancy?v=23.2#best-practices-and-limitations)  | [Modules in a Multi-Tenant Application](https://docs.devexpress.com/eXpressAppFramework/404695/multitenancy/modules-in-multitenant-application?v=23.2)

![](./Images/ManageTenants.png)

## Files to Review

- [OutlookInspired.Blazor.Server/Services/Internal/ApplicationBuilder.cs](./CS/OutlookInspired.Blazor.Server/Services/Internal/ApplicationBuilder.cs)
- [OutlookInspired.Win/Services/ApplicationBuilder.cs](./CS/OutlookInspired.Win/Services/ApplicationBuilder.cs)
- [OutlookInspired.Module/DatabaseUpdate/Updater.cs](./CS/OutlookInspired.Module/DatabaseUpdate/Updater.cs)
