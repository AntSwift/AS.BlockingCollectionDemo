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
        const int MAX_PRODUCERS = 2;

        static void Main()
        {
            var blockingCollection = new BlockingCollection<Pulse>();
            var count = 1;

            syncTimer = new Timer(new TimerCallback(TimerElapsed), blockingCollection, 0, 5000);

            Task[] producers = new Task[MAX_PRODUCERS];

            for (int i = 0; i < MAX_PRODUCERS; i++)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    while (count <= MAX_GENERATED)
                    {
                        blockingCollection.Add(new Pulse(1, DateTime.Now));
                        Interlocked.Increment(ref count);
                        Thread.Sleep(100);
                    }
                });

                producers[i] = task;
            }

            Task.WaitAll(producers);

            Console.ReadKey(true);
        }

        static void TimerElapsed(object state)
        {
            var buffer = new List<Pulse>(MAX_BUFFER_SIZE);

            Pulse pulse = null;
            while (((BlockingCollection<Pulse>)state).TryTake(out pulse, 0))
            {
                buffer.Add(pulse);

                if (buffer.Count == MAX_BUFFER_SIZE)
                {
                    break;
                }
            }

            ProcessItems(buffer);
            buffer.Clear();
        }

        static void ProcessItems(List<Pulse> buffer)
        {
            // TODO: Process items accordingly.
            Console.WriteLine("Worker: Processing {0} pulses", buffer.Count);
        }
    }
}