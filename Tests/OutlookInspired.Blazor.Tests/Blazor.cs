// using DevExpress.ExpressApp;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Hosting;
// using NUnit.Framework;
// using OutlookInspired.Blazor.Server;
// using OutlookInspired.Blazor.Tests.Common;
// using XAF.Testing;
//
// #pragma warning disable CS8974 
//
// namespace OutlookInspired.Blazor.Tests{
//     
//     [Apartment(ApartmentState.STA)][Order(1)]
//     public class Blazor:TestBase{
// #if TEST
//         [OutlookInspired.Tests.Common.RetryTestCaseSource(nameof(TestCases),MaxTries = 3)]
//         [Category("BlazorTest")]
// #else
//         [TestCaseSource(nameof(TestCases))]
// #endif
//         public async Task Test(string navigationView, string viewVariant,string user,Func<XafApplication,string,string,IObservable<Frame>> assert){
//             // using var application = await SetupBlazorApplication(useServer:true,runInMainMonitor:false);
//
//             Host.CreateDefaultBuilder(Array.Empty<string>())
//                 .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
//             // application.StartWinTest(assert(application,navigationView, viewVariant),user);
//         }
//
//         [SetUp]
//         public void Setup(){
// #if TEST
//             XAF.Testing.RX.UtilityExtensions.TimeoutInterval=TimeSpan.FromSeconds(120);
// #else
//             this.Await(async () => await LogContext.None.WriteAsync());
// #endif
//         }
//         
//         
//
//         [TearDown]
//         public void TearDown(){
//             
//         }
//     }
// }