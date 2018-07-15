using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                return;
            }

            var parentApplication = args.First();

            Console.WriteLine(parentApplication);

            for (var index = 0; index < 5; index++)
            {
                Console.WriteLine("Waiting...");
                System.Threading.Thread.Sleep(1000 * 1);
            }

            Console.WriteLine("Done.");
        }
    }
}
