namespace OutlookInspired.Module.Services{
    public static class TaskExtensions{
        // public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this Task<T> task){
        //     yield return await task;
        // }
        
        public static async Task Delay(this Task task, TimeSpan delay){
            await task;
            await Task.Delay(delay);
        }

        public static async Task<T> Delay<T>(this Task<T> task, TimeSpan delay){
            var result = await task;
            await Task.Delay(delay);
            return result;
        }
    }
}