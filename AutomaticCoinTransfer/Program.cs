using System;
using System.Linq;
using System.Net.Http;

namespace AutomaticCoinTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            Scheduler scheduler = new Scheduler();
            scheduler.Start();

            Console.ReadKey();
        }
    }
}
