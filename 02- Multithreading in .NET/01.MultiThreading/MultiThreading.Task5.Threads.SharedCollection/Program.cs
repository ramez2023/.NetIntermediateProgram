/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and 
 * the second should print all elements in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static List<int> sharedCollection = new List<int>();

        static AutoResetEvent writingLock = new AutoResetEvent(true);
        static AutoResetEvent readingLock = new AutoResetEvent(false);

        static async Task Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            // feel free to add your code
            Task addTask = AddToCollectionAsync();
            Task printTask = PrintCollectionAsync();

            await Task.WhenAll(addTask, printTask);
            Console.ReadLine();
        }

        static Task AddToCollectionAsync()
        {
            return Task.Run(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    writingLock.WaitOne();
                    readingLock.Reset();

                    AddToCollection(i);
                    Console.WriteLine($"Element is added {i}");

                    readingLock.Set();

                }
            });
        }

        static Task PrintCollectionAsync()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    readingLock.WaitOne();
                    writingLock.Reset();

                    PrintCollection();

                    writingLock.Set();
                }
            });
        }

        static void AddToCollection(int value)
        {
            sharedCollection.Add(value);
        }

        static void PrintCollection()
        {
            Console.WriteLine("Elements in the collection: " + string.Join(", ", sharedCollection));
        }
    }
}