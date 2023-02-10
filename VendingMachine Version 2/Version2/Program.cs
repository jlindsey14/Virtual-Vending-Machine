using System;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            VendingMachine VM = new VendingMachine();
            Console.Write("Welcome!");
            VM.Run();
        }
    }
}
