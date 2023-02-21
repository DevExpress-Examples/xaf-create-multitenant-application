using DevExpress.ExpressApp.MiddleTier;
using DevExpress.ExpressApp.Win.Core;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp;
using SAASExtension.Interfaces;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace SAASExtensionWin;
public class SAASWinApplication : WinApplication {
    private static ConcurrentDictionary<string, bool> isCompatibilityChecked = new ConcurrentDictionary<string, bool>();
    protected override bool IsCompatibilityChecked {
        get => isCompatibilityChecked.ContainsKey(ServiceProvider.GetRequiredService<IConnectionStringProvider>().GetConnectionString());
        set => isCompatibilityChecked.TryAdd(ServiceProvider.GetRequiredService<IConnectionStringProvider>().GetConnectionString(), value);
    }
}
