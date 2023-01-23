using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using Microsoft.Extensions.DependencyInjection;
using SAASExample1.Module.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SAASExample1.Module.BusinessObjects;
[DomainComponent]
public class CustomLogonParametersForStandardAuthentication : AuthenticationStandardLogonParameters, ICompany, IServiceProviderConsumer {
    private IServiceProvider serviceProvider;
    private CompanyNameHolder companyName;
    private IReadOnlyList<CompanyNameHolder> companyNameObjs = null;

    [DataSourceProperty(nameof(GetCompanyNames), DataSourcePropertyIsNullMode.SelectAll)]
    public CompanyNameHolder CompanyName {
        get { return companyName; }
        set {
            companyName = value;
        }
    }

    [Browsable(false)]
    [JsonIgnore]
    public IReadOnlyList<CompanyNameHolder> GetCompanyNames {
        get {
            if((companyNameObjs == null) && (serviceProvider != null)) {
                companyNameObjs = new List<CompanyNameHolder>();
                ICompanyNamesHelper companyNamesHelper = serviceProvider.GetRequiredService<ICompanyNamesHelper>();
                foreach(var name in companyNamesHelper.GetCompanyNamesMap().Keys) {
                    ((List<CompanyNameHolder>)companyNameObjs).Add(new CompanyNameHolder(name));
                }
            }
            return companyNameObjs;
        }
    }

    public void SetServiceProvider(IServiceProvider serviceProvider) {
        this.serviceProvider = serviceProvider;
    }
}

[DomainComponent]
//set readonly using model editor
//<DetailView Id = "CompanyNameHolder_DetailView" AllowEdit="False" />
//CustomLogonController checks that the DataBaseNameHolder property is not null
public class CompanyNameHolder : NonPersistentLiteObject {
    public CompanyNameHolder(string name) {
        Name = name;
    }
    public string Name { get; }
}