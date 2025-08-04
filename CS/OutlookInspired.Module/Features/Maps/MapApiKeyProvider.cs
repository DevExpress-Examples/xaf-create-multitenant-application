namespace OutlookInspired.Module.Features.Maps{
    public class MapApiKeyProvider : IMapApiKeyProvider {
        public string Key => "CDOhtwNBSsbmBiN3rUEjmBJGHW2tRMbp5XVwu4J55VBZg8PdRe9MJQQJ99BEACYeBjFllM6LAAAgAZMP1cA4";
    }
    
    public interface IMapApiKeyProvider {
        public string Key{ get; }
    }

}