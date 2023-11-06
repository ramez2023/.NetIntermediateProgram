using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    public class ThreadAndJoin
    {
        public static void Run()
        {
            int initialState = 10;

            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(DecrementAndPrint);
                thread.Start(initialState);
                thread.Join();
            }
        }

        static void DecrementAndPrint(object state)
        {
            int value = (int)state;

            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {value}");

            if (value > 0)
            {
                Thread newThread = new Thread(DecrementAndPrint);
                newThread.Start(value - 1);
                newThread.Join();
            }
        }
    }
}
