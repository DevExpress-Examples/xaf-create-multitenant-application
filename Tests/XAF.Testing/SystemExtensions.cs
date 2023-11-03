using System.Diagnostics;

namespace XAF.Testing{
    public static class SystemExtensions{
        public static Process Start(this ProcessStartInfo processStartInfo) 
            => Process.Start(processStartInfo);

        public static object CreateInstance(this Type type,params object[] args) => Activator.CreateInstance(type,args:args);

        public static void KillAll(this AppDomain appDomain,string processName) 
            => Process.GetProcessesByName(processName)
                .Do(process => {
                    process.Kill();
                    process.WaitForExit();
                }).Enumerate();
        
    }
}