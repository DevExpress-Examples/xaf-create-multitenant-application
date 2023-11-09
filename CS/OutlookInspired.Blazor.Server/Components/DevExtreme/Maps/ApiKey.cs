namespace OutlookInspired.Blazor.Server.Components.DevExtreme.Maps;
public class ApiKey{
    public string Bing{ get; set; } = Environment.GetEnvironmentVariable("BingKey");
}