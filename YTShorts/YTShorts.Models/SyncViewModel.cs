namespace YTShorts.Models;

public class SyncViewModel<T>
{
    public DateTime ServerSyncTime { get; set; }

    public List<T> Updates { get; set; } = [];

    public List<int> Deletes { get; set; } = [];
}
