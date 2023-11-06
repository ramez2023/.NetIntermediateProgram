/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // feel free to add your code
            //OptionA();
            //OptionB();
            //OptionC();
            await OptionD();



            Console.ReadLine();
        }

        static void OptionA()
        {
            Task parentTask = Task.Run(() =>
            {
                // Simulate some work
                Console.WriteLine("Parent Task (Case A) is working...");
            });

            Task continuationTask = parentTask.ContinueWith((completedTask) =>
            {
                Console.WriteLine($"Continuation Task (Case A) (Thread ID - {Thread.CurrentThread.ManagedThreadId}) executed, regardless of the parent task result.");
            });

            Task.WaitAll(parentTask, continuationTask);
        }

        static void OptionB()
        {
            Task parentTask = Task.Run(() =>
            {
                // Simulate some work
                Console.WriteLine("Parent Task (Case B) is working...");
                throw new Exception("Parent Task (Case B) failed.");
            });

            Task continuationTask = parentTask.ContinueWith((completedTask) =>
            {
                if (completedTask.IsFaulted)
                {
                    Console.WriteLine($"Continuation Task (Case B) (Thread ID - {Thread.CurrentThread.ManagedThreadId}) executed because the parent task failed.");
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            try
            {
                Task.WaitAll(parentTask, continuationTask);
            }
            catch (AggregateException)
            {
                // Handle the exception from the parent task
            }
        }


        static void OptionC()
        {
            Task parentTask = Task.Run(() =>
            {
                // Simulate some work
                Console.WriteLine("Parent Task (Case C) is working...");
                throw new Exception("Parent Task (Case C) failed.");
            });

            Task continuationTask = parentTask.ContinueWith((completedTask) =>
            {
                if (completedTask.IsFaulted)
                {
                    Console.WriteLine($"Continuation Task (Case C) (Thread ID - {Thread.CurrentThread.ManagedThreadId}) executed on the same thread because the parent task failed.");
                }
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            try
            {
                Task.WaitAll(parentTask, continuationTask);
            }
            catch (AggregateException)
            {
                // Handle the exception from the parent task
            }
        }

        static async Task OptionD()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            cts.CancelAfter(1000);

            Task parentTask = Task.Run(() =>
            {
                // Simulate some work
                Thread.Sleep(2000); // Sleep for 2 seconds
                token.ThrowIfCancellationRequested();

            }, token);


            var child = parentTask.ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Console.WriteLine($"Continuation Task (Case D) (Thread ID - {Thread.CurrentThread.ManagedThreadId}) executed on the same thread because the parent task failed.");
                    Console.WriteLine($"IsThreadPoolThread {Thread.CurrentThread.IsThreadPoolThread}");
                }

            }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);


            await child;
        }

    }
}
