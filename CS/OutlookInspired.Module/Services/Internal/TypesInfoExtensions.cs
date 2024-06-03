using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;

namespace OutlookInspired.Module.Services.Internal{
    internal static partial class TypesInfoExtensions{
        public static ITypeInfo ToTypeInfo(this Type type) => XafTypesInfo.Instance.FindTypeInfo(type);
        public static IEnumerable<ITypeInfo> ToTypeInfo(this IEnumerable<Type> source) => source.Select(type => type.ToTypeInfo());
    }
}