using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AS.BlockingCollectionDemo.ConsoleUI
{
    static class Program
    {
        static System.Threading.Timer syncTimer;

        const int MAX_BUFFER_SIZE = 500;
        const int MAX_GENERATED = 5000;

        static void Main()
        {
            var blockingCollection = new BlockingCollection<int>();
            var count = 1;

            syncTimer = new Timer(new TimerCallback(TimerElapsed), blockingCollection, 0, 5000);

            var p1 = Task.Factory.StartNew(() =>
            {
                while (count <= MAX_GENERATED)
                {
                    //Console.WriteLine("Producer A: " + count);
                    blockingCollection.Add(count);
                    Interlocked.Increment(ref count);
                    Thread.Sleep(100);
                }
            });

            var p2 = Task.Factory.StartNew(() =>
            {
                while (count <= MAX_GENERATED)
                {
                    //Console.WriteLine("Producer B: " + count);
                    blockingCollection.Add(count);
                    Interlocked.Increment(ref count);
                }
            });

            var p3 = Task.Factory.StartNew(() =>
            {
                while (count <= MAX_GENERATED)
                {
                    //Console.WriteLine("Producer C: " + count);
                    blockingCollection.Add(count);
                    Interlocked.Increment(ref count);
                }
            });

            Task.WaitAll(p1, p2, p3);

            Console.ReadKey(true);
        }

        static void TimerElapsed(object state)
        {
            var buffer = new List<int>(MAX_BUFFER_SIZE);

            int value = 0;
            while (((BlockingCollection<int>)state).TryTake(out value, 0))
            {
                buffer.Add(value);

                if (buffer.Count == MAX_BUFFER_SIZE)
                {
                    break;
                }
            }

            ProcessItems(buffer);
            buffer.Clear();
        }

        static void ProcessItems(List<int> buffer)
        {
            // TODO: Process items accordingly.
            Console.WriteLine("Worker: Processing {0} items", buffer.Count);
        }
    }
}