namespace SemaphorePlus;
public class SemaphoreSlimDelayed : SemaphoreSlim
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private readonly int maxCount;
    private readonly SlimphoreDelayType? delayType;
    private readonly int milliseconds;
    private int connectedCount = 0;
    public SemaphoreSlimDelayed(int maxCount, int milliseconds) : base(1, maxCount)
    {
        this.maxCount = maxCount;
        this.milliseconds = milliseconds;
    }
    public SemaphoreSlimDelayed(int maxCount, SlimphoreDelayType delayType) : base(1, maxCount)
    {
        this.maxCount = maxCount;
        this.delayType = delayType;
        milliseconds = delayType switch
        {
            SlimphoreDelayType.EveryMinute => 1 * 60 * 1000,
            SlimphoreDelayType.EverySecond => 1 * 1000,
            _ => throw new NotImplementedException()
        };
    }

    public int ConnectedCount => connectedCount;

    public async Task RegisterAsync()
    {
        _semaphore.Wait();
        connectedCount++;
        _semaphore.Release();
        if (connectedCount >= maxCount)
        {
            await WaitAsync();
        }
    }
    public async Task InitAsync()
    {
        if (delayType == SlimphoreDelayType.EverySecond)
            await Task.Delay(1000 - DateTime.Now.Millisecond);
        if (delayType == SlimphoreDelayType.EveryMinute)
            await Task.Delay(
                ((60 - DateTime.Now.Second) * 1000) + 
                (1000 - DateTime.Now.Millisecond));
        WaitForInit();
    }
    private void WaitForInit()
    {
        Task.Run(async () =>
        {
            await Task.Delay(milliseconds);
            if (connectedCount > maxCount)
            {
                for (int i = 0; i < maxCount; i++)
                {
                    Release();
                }
                connectedCount -= maxCount;
            }
            else
                connectedCount = 0;
            WaitForInit();
        });
    }
}

public enum SlimphoreDelayType
{
    EveryMinute,
    EverySecond
}