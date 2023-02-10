using System;

namespace OldCapstone
{
    class Program
    {
        static void Main(string[] args)
        {
            VendingMachine VM = new VendingMachine();
            Transaction transaction = new Transaction(VM);
            VM.Transaction = transaction;
            Console.Write("Welcome!");
            VM.DisplayOptions();
        }
    }
}
