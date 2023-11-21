namespace SemaphorePlus;
public class SemaphoreSlimDelayed : SemaphoreSlim
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private readonly int maxCount;
    private readonly int milliseconds;
    private int connectedCount = 0;
    public SemaphoreSlimDelayed(int maxCount, int milliseconds) : base(1, maxCount)
    {
        this.maxCount = maxCount;
        this.milliseconds = milliseconds;
    }

    public int ConnectedCount => connectedCount;

    public async Task RegisterAsync()
    {
        _semaphore.Wait();
        connectedCount++;
        //Console.ForegroundColor = ConsoleColor.Green;
        //Console.WriteLine("{" + connectedCount + "} It's Connected Count");
        //Console.ForegroundColor = ConsoleColor.White;
        _semaphore.Release();
        if (connectedCount >= maxCount)
        {
            await WaitAsync();
        }
    }
    public void Init()
    {
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
