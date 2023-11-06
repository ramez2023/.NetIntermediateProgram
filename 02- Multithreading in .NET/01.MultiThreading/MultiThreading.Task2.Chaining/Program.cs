/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static readonly Random Random = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            // feel free to add your code
            Task<int[]> createArrayTask = Task.Run(() =>
            {
                int[] randomArray = new int[10];
                for (int i = 0; i < randomArray.Length; i++)
                {
                    randomArray[i] = Random.Next(1, 100);
                }
                Console.WriteLine("Created Array: " + string.Join(", ", randomArray));
                return randomArray;
            });

            Task<int[]> multiplyArrayTask = createArrayTask.ContinueWith(prevTask =>
            {
                int[] array = prevTask.Result;
                int multiplier = Random.Next(2, 10);
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] *= multiplier;
                }
                Console.WriteLine($"Multiplied Array (*{multiplier}): {string.Join(", ", array)}");
                return array;
            });

            Task<int[]> sortArrayTask = multiplyArrayTask.ContinueWith(prevTask =>
            {
                int[] array = prevTask.Result;
                Array.Sort(array);
                Console.WriteLine("Sorted Array: " + string.Join(", ", array));
                return array;
            });

            Task<double> calculateAverageTask = sortArrayTask.ContinueWith(prevTask =>
            {
                int[] array = prevTask.Result;
                double average = array.Average();
                Console.WriteLine("Average Value: " + average);
                return average;
            });

            Task.WhenAll(createArrayTask, multiplyArrayTask, sortArrayTask, calculateAverageTask).Wait();

            Console.ReadLine();
        }
    }
}
