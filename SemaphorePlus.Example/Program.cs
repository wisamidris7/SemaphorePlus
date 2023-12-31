﻿using SemaphorePlus;

//var semaphoreSlimDelayed = new SemaphoreSlimDelayed(3, 10000);
//await semaphoreSlimDelayed.InitAsync();

//foreach (var item in Enumerable.Range(0, 1000))
//{
//    await Task.Delay(Random.Shared.Next(100, 200));
//    Task.Run(async () =>
//    {
//        Console.WriteLine("I'm Started " + item.ToString());
//        await semaphoreSlimDelayed.RegisterAsync();
//        Console.ForegroundColor = ConsoleColor.Red;
//        Console.WriteLine("I'm Have Been Done " + item.ToString());
//        Console.ForegroundColor = ConsoleColor.White;
//    });
//}

// --------------------------------------------------------------------

var semaphoreSlimDelayed = new SemaphoreSlimDelayed(3, SlimphoreDelayType.EveryMinute);
await semaphoreSlimDelayed.InitAsync();

foreach (var item in Enumerable.Range(0, 10))
{
    await Task.Delay(Random.Shared.Next(10, 20));
    Task.Run(async () =>
    {
        Console.WriteLine("I'm Started " + item.ToString());
        await semaphoreSlimDelayed.RegisterAsync();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("I'm Have Been Done " + item.ToString());
        Console.ForegroundColor = ConsoleColor.White;
    });
}

Console.ReadKey();