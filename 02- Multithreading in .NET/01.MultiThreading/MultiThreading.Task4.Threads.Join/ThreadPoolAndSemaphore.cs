using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    internal class ThreadPoolAndSemaphore
    {
        static Semaphore semaphore = new Semaphore(0, 1);

        public static void Run()
        {
            int initialState = 10;

            ThreadPool.QueueUserWorkItem(DecrementAndPrint, initialState);
            semaphore.WaitOne();

            for (int i = 0; i < 9; i++)
            {
                semaphore.WaitOne();
            }
        }

        static void DecrementAndPrint(object state)
        {
            int value = (int)state;

            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {value}");

            if (value > 0)
            {
                ThreadPool.QueueUserWorkItem(DecrementAndPrint, value - 1);
            }

            semaphore.Release();
        }
    }
}
