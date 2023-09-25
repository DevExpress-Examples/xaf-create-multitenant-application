using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;

namespace XAF.Testing.XAF{
    public static class TypesInfoExtensions{
        public static void SetValue(this IObjectSpace objectSpace, object newObject,IMemberInfo memberInfo, object existingObject){
            var existingValue = memberInfo.GetValue(existingObject);
            memberInfo.SetValue(newObject, memberInfo.IsPersistent ? objectSpace.GetObject(existingValue) : existingValue);
        }
        public static void SetValue(this IObjectSpaceLink newObject,IMemberInfo memberInfo, object existingObject) 
            => newObject.ObjectSpace.SetValue(newObject, memberInfo, existingObject);
        
        public static Version XAFVersion(this ITypesInfo typesInfo) 
            => typeof(TypesInfoExtensions).Assembly.GetReferencedAssemblies().First(assemblyName => assemblyName.Name?.Contains("DevExpress.ExpressApp")??false).Version;
    }
}