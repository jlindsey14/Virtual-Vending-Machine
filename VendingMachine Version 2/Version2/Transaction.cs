using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone
{
    public class Transaction
    {
        //public Dictionary<string, Animal> SlotToAnimalDictionary { get; } = new Dictionary<string, Animal>();
        //public VendingMachine VM { get; }
        public List<string> Logs { get; set; } = new List<string>();// Contains list of logs items that detail: DateTime, action, amount deposited/spent, new balance
        public decimal Balance { get; set; } = 0M;

        string outputFilePath = "C:\\Users\\Student\\workspace\\c-sharp-minicapstonemodule1-team2\\Log.txt";

        public Transaction()
        {
            

            
        }





        public void FeedMoney(decimal input)
        {
            Balance += input;
        }

        //public void SelectProduct()
        //{
        //    Console.WriteLine(DisplayItems());

        //    Console.WriteLine("\nPlease make your selection by entering the Slot ID (ex: A1)\n");
        //    string slotID = "";
        //    slotID = Console.ReadLine();
        //    if (!SlotToAnimalDictionary.ContainsKey(slotID))
        //    {
        //        Console.WriteLine("Invalid Slot ID, try again.");
        //        DisplayPurchaseOptions();
        //    }
        //    else if (SlotToAnimalDictionary[slotID].NumRemaining < 1)
        //    {
        //        Console.WriteLine("Sorry, item SOLD OUT!");
        //        DisplayPurchaseOptions();
        //    }
        //    else if (SlotToAnimalDictionary[slotID].Price > Balance)
        //    {
        //        Console.WriteLine("Sorry, not enough funds");
        //        DisplayPurchaseOptions();
        //    }
        //    else
        //    {
        //        Dispense(SlotToAnimalDictionary[slotID]);
        //        DisplayPurchaseOptions();
        //    }
        //}

        public void FinalizeTransaction()
        {
            GiveChange();
            WriteLog();
            Console.WriteLine("\nWelcome!");
            VM.DisplayOptions();
        }

        public void Dispense(Animal animal)
        {
            Balance -= animal.Price;
            Console.WriteLine($"You purchased {animal.Name} for {animal.Price:C2}!");
            Console.WriteLine(animal.DispenseMessage);
            Console.WriteLine($"Your remaining balance is {Balance:C2}");
            animal.NumRemaining--;
            AddToLog(animal.Name + " " + animal.SlotID, animal.Price);
        }

        public string GiveChange()
        {
            //Calculating change denominations
            decimal decimalBalance = Balance;
            decimal totalChange = decimalBalance % 1;

            int numDollars = Decimal.ToInt32(Balance);
            int numQuarters = (int)(totalChange / 0.25M);
            totalChange -= (numQuarters * .25M);
            int numDimes = (int)(totalChange / 0.1M);
            totalChange -= (numDimes * .1M);
            int numNickels = (int)(totalChange / 0.05M);
            totalChange -= (numNickels * .05M);

            decimal totalChangeDispensed = Balance;
            Balance = 0;

            //Creating a dictionary to print change 
            Dictionary<string, int> changeDict = new Dictionary<string, int>{
                { "dollar", numDollars },
                { "quarter", numQuarters },
                { "dime", numDimes},
                { "nickel", numNickels }, };
            string changeMessage = $"Your change is {totalChangeDispensed:C2}. You'll recieve: ";
            foreach (KeyValuePair<string, int> kvp in changeDict)
            {
                if (kvp.Value > 0)
                {
                    changeMessage += $"{kvp.Value} {kvp.Key}{(kvp.Value != 1 ? "s" : String.Empty)}, ";
                }
            }
            changeMessage = changeMessage.Substring(0, changeMessage.Length - 2);
            changeMessage += ". Thank you!";
            Console.WriteLine(changeMessage);
            AddToLog("Give Change", totalChangeDispensed);
            return changeMessage;
        }

        public void AddToLog(string actionName, decimal decimalAmount)
        {
            string logMessage = $"{DateTime.Now} {actionName} {decimalAmount:C2} {Balance:C2}";
            Logs.Add(logMessage);
        }

        public void WriteLog()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(outputFilePath, false))
                {
                    foreach (string logitem in Logs)
                    {
                        sw.WriteLine(logitem);
                    }

                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error writing file. GIT GUD");
            }

        }
    }
}
