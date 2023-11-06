/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and 
 * the second should print all elements in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static List<int> sharedCollection = new List<int>();
        private static object lockObject = new object();

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

        static async Task AddToCollectionAsync()
        {
            for (int i = 1; i <= 10; i++)
            {
                AddToCollection(i);
                await Task.Delay(100); // Simulate some work
            }
        }

        static async Task PrintCollectionAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                PrintCollection();
                await Task.Delay(100); // Simulate some work
            }
        }

        static void AddToCollection(int value)
        {
            lock (lockObject)
            {
                sharedCollection.Add(value);
            }
        }

        static void PrintCollection()
        {
            lock (lockObject)
            {
                Console.WriteLine("Elements in the collection: " + string.Join(", ", sharedCollection));
            }
        }
    }
}
