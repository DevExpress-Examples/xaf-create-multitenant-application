using DevExpress.ExpressApp.DC;

namespace XAF.Testing.XAF{
    public static class TypesInfoExtensions{
        
        public static Version XAFVersion(this ITypesInfo typesInfo) 
            => typeof(TypesInfoExtensions).Assembly.GetReferencedAssemblies().First(assemblyName => assemblyName.Name?.Contains("DevExpress.ExpressApp")??false).Version;
    }
}