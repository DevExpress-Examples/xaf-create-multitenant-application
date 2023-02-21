using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using Microsoft.Extensions.DependencyInjection;
using SAASExtension.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SAASExtension.BusinessObjects;
[DomainComponent]
public class CustomLogonParametersForStandardAuthentication : AuthenticationStandardLogonParameters, ITenantName, IServiceProviderConsumer {
    private IServiceProvider serviceProvider;
    private TenantNameHolder tenant;
    private string tenantName;

    public CustomLogonParametersForStandardAuthentication() : base() {
    }
    public CustomLogonParametersForStandardAuthentication(string userName, string password) : base(userName, password) {
    }
    protected CustomLogonParametersForStandardAuthentication(SerializationInfo info, StreamingContext context) : base(info, context) {
        tenantName = info.GetString("tenantName");
    }

    [DataSourceProperty(nameof(GetTenantNames), DataSourcePropertyIsNullMode.SelectAll)]
    [JsonIgnore]
    public TenantNameHolder Tenant {
        get { return tenant; }
        set {
            tenant = value; 
            TenantName = value?.Name;
            RaisePropertyChanged(nameof(Tenant));
        }
    }
    [Browsable(false)]
    public string TenantName {
        get { return tenantName; }
        set {
            tenantName = value;
        }
    }
    [Browsable(false)]
    [JsonIgnore]
    public IReadOnlyList<TenantNameHolder> GetTenantNames {
        get {
            IReadOnlyList<TenantNameHolder> tenantNameObjs = new List<TenantNameHolder>();
            if (serviceProvider != null) {
                ITenantNamesHelper tenantNamesHelper = serviceProvider.GetRequiredService<ITenantNamesHelper>();
                foreach(var name in tenantNamesHelper.GetTenantNamesMap().Keys) {
                    ((List<TenantNameHolder>)tenantNameObjs).Add(new TenantNameHolder(name));
                }
            }
            return tenantNameObjs;
        }
    }
    public void SetServiceProvider(IServiceProvider serviceProvider) {
        this.serviceProvider = serviceProvider;
    }
}

[DomainComponent]
public class TenantNameHolder : NonPersistentLiteObject {
    public TenantNameHolder(string name) {
        Name = name;
    }
    public string Name { get; }
}