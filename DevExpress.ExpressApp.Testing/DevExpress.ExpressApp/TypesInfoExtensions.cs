using DevExpress.ExpressApp.DC;

namespace DevExpress.ExpressApp.Testing.DevExpress.ExpressApp{
    public static class TypesInfoExtensions{
        
        public static Version XAFVersion(this ITypesInfo typesInfo) 
            => typeof(TypesInfoExtensions).Assembly.GetReferencedAssemblies().First(assemblyName => assemblyName.Name?.Contains("DevExpress.ExpressApp")??false).Version;
    }
}