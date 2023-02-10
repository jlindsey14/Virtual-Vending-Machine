using System;
using System.Speech.Synthesis;

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
