using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OutlookInspired.Win;
using OutlookInspired.Win.Extensions;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData{
    public abstract class TestBase{
        protected static TimeSpan Timeout = UtilityExtensions.TimeoutInterval;

        public async Task<WinApplication> SetupWinApplication(string title, Func<WinApplication, Task> beforeSetup = null){
            var builder = WinApplication.CreateBuilder();
            builder.UseApplication<OutlookInspiredWindowsFormsApplication>();
            builder.AddModules();
            builder.AddObjectSpaceProviders(options => options.UseSqlServer("Integrated Security=SSPI;Pooling=true;MultipleActiveResultSets=true;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=OutlookInspired"));
            var application = builder.Build();
            application.ApplicationName = title;
            application.SplashScreen = null;  
            if (beforeSetup != null){
                await beforeSetup(application);
            }
            application.Setup();
            application.DeleteModelDiffs();
            application.ChangeStartupState(FormWindowState.Maximized);
            return application;
        }

        public Task<WinApplication> SetupWinApplication(Func<WinApplication, Task> beforeSetup=null) 
            => SetupWinApplication(TestContext.CurrentContext.Test.FullName, beforeSetup);
    }
}