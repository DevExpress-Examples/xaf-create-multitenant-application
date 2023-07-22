namespace OutlookInspired.Module.Services{
    public static class ReflectionExtensions{
        public static T CreateInstance<T>(this Type type) => (T)CreateInstance(type);

        public static object CreateInstance(this Type type){
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            if (type.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(type);
            throw new InvalidOperationException($"Type {type.FullName} does not have a parameterless constructor.");
        }
        
        public static bool IsDefaultValue<TSource>(this TSource source) {
            var def = default(TSource);
            return def != null ? def.Equals(source) : typeof(TSource) == typeof(object)
                ? source == null || source.Equals(source.GetType().DefaultValue()) : source == null;
        }

        public static bool IsDefaultValue(this object source) 
            => source == null || source.Equals(source.GetType().DefaultValue());
		
        public static bool IsDefaultValue(this object source,Type objectType) 
            => objectType.DefaultValue()==source;
        
        public static object DefaultValue(this Type t) => t.IsValueType ? Activator.CreateInstance(t) : null;
		
        public static T DefaultValue<T>(this Type t) => t.IsValueType||t.IsArray ? t.CreateInstance<T>() : default;
    }
}