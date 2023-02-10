using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Transactions;

namespace Capstone
{
    public class VendingMachine
    {
        public List<Animal> Inventory { get; } = new List<Animal>();
        public Dictionary<string, Animal> SlotToAnimalDictionary { get; } = new Dictionary<string, Animal>();
        public Transaction Transaction { get; set; }
        string inputFilePath = "C:\\Users\\Student\\workspace\\c-sharp-minicapstonemodule1-team2\\vendingmachine.csv";
        //string outputFilePath = "C:\\Users\\Student\\workspace\\c-sharp-minicapstonemodule1-team2\\Log.txt";
        string salesReportFilePath = "C:\\Users\\Student\\workspace\\c-sharp-minicapstonemodule1-team2\\SalesReport.txt";

        public VendingMachine()
        {
            FileRead();
            foreach (Animal animal in Inventory)
            {
                SlotToAnimalDictionary[animal.SlotID] = animal;
            }
        }

        public void Run()
        {
            Transaction = new Transaction(Inventory);
            while (true)
            {
                Console.WriteLine(DisplayOptions());
                int optionChoice = GetOptionChoice();
                if (optionChoice == 3)
                {
                    break;
                }
                Console.WriteLine(CallMenuOption(optionChoice));
                int purchaseChoice = GetPurchaseInput();
                Console.WriteLine(CallPurchaseOption(purchaseChoice));
            }
        }

        public string DisplayOptions()
        {
            return @$"
Please select from the following options: (enter number)

1. Display Vending Machine Items
2. Purchase
3. Exit";
        }

        public int GetOptionChoice()
        {
            Console.WriteLine();
            // Defensive coding for user input to choose an option
            int option = 0;
            do
            {
                try
                {
                    option = int.Parse(Console.ReadLine());
                    if (option < 1 || option > 4)
                    {
                        Console.WriteLine("\nHmm... Try entering 1, 2, or 3.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter 1, 2, or 3");
                }
            }

            while (option < 1 || option > 4);
            return option;
        }

        public string CallMenuOption(int option)
        {
            switch (option)
            {
                case 1:
                    return DisplayItems();
                case 2:
                    return DisplayPurchaseOptions();
                case 3:
                    return "Exit";
                case 4:
                    WriteSalesReport();
                    return "Generating secret sales report.....";
            }        
            return "";
        }

        public string DisplayItems()
        {
            string displayItems = "";

            displayItems += "\nAvailable Items:\n";
            foreach (Animal animal in Inventory)
            {
                string availability = animal.NumRemaining > 0 ? (animal.NumRemaining).ToString() + " left" : "Sold Out";
                displayItems += $"\n{animal.SlotID}. {animal.Name} {animal.Price:C2} ({availability})";
            }

            return displayItems;
        }

        // Purchase menu
        public string DisplayPurchaseOptions()
        {
            return @$"
Current money provided: {Transaction.Balance:C2}

1. Feed Money
2. Select Product
3. Finish Transaction";
        }

        public int GetPurchaseInput()
        {
            Console.WriteLine();
            // Defensive coding for user input to choose an option
            int option = 0;
            do
            {
                try
                {
                    option = int.Parse(Console.ReadLine());
                    if (option < 1 || option > 3)
                    {
                        Console.WriteLine("\nHmm... Try entering 1, 2, or 3.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter 1, 2, or 3");
                }
            }
            while (option < 1 || option > 3);
            return option;
        }

        public string CallPurchaseOption(int option)
        {
            switch (option)
            {
                case 1:
                    decimal moneyFed = FeedMoney();
                    Transaction.FeedMoney(moneyFed);
                    return "";
                case 2:
                    return SelectProduct();
                case 3:
                    return FinalizeTransaction();
            }
        }

        public decimal FeedMoney()
        {
            Console.WriteLine("Please input your money, in WHOLE DOLLAS");
            int dollarAmount = 0;
            do
            {
                try
                {
                    dollarAmount = int.Parse(Console.ReadLine());
                    if (dollarAmount < 1)
                    {
                        Console.WriteLine("Please enter a POSTIVE whole number");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a positive whole number");
                }
            }
            while (dollarAmount < 1);
            
            decimal decimalAmount = (decimal)dollarAmount;
            return decimalAmount;
        }

        public void SelectProduct()
        {
            Console.WriteLine(DisplayItems());

            Console.WriteLine("\nPlease make your selection by entering the Slot ID (ex: A1)\n");
            string slotID = "";
            slotID = Console.ReadLine();
            if (!SlotToAnimalDictionary.ContainsKey(slotID))
            {
                Console.WriteLine("Invalid Slot ID, try again.");
            }
            else if (SlotToAnimalDictionary[slotID].NumRemaining < 1)
            {
                Console.WriteLine("Sorry, item SOLD OUT!");
            }
            else if (SlotToAnimalDictionary[slotID].Price > Balance)
            {
                Console.WriteLine("Sorry, not enough funds");
            }
            else
            {
                Dispense(SlotToAnimalDictionary[slotID]);
            }
        }
        public void FileRead()
        {
            try
            {
                using (StreamReader sr = new StreamReader(inputFilePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string lineOfText = sr.ReadLine();
                        string[] splitLine = lineOfText.Split("|");
                        string slotID = splitLine[0];
                        string name = splitLine[1];
                        decimal price = decimal.Parse(splitLine[2]);
                        string type = splitLine[3];
                        Animal animal = new Animal(name, type, price, slotID);
                        Inventory.Add(animal);
                    }
                }
            }
            catch (Exception)
            { Console.WriteLine("Error reading file"); }
        }

        //public void WriteLog()
        //{
        //    try
        //    {
        //        using (StreamWriter sw = new StreamWriter(outputFilePath, false))
        //        {
        //            foreach(string logitem in Transaction.Logs)
        //            {
        //                sw.WriteLine(logitem);
        //            }

        //        }
        //    }
        //    catch (Exception)
        //    {
        //        Console.WriteLine("Error writing file. GIT GUD");
        //    }

        //}

        public void WriteSalesReport()
        {
            decimal totalSale = 0;
            List<string> salesReportItems = new List<string>();

            foreach (Animal animal in Inventory)
            {
                int numPurchased = 5 - animal.NumRemaining;
                totalSale += numPurchased * animal.Price;
                string lineOfText = $"{animal.Name}|{numPurchased}";
                salesReportItems.Add(lineOfText);
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(salesReportFilePath, false))
                {
                    foreach (string item in salesReportItems)
                    {
                        sw.WriteLine(item);
                    }
                    sw.WriteLine();
                    sw.WriteLine($"TOTAL SALES: {totalSale:C2}");
                }
            }
            catch (Exception)
            { Console.WriteLine("Error writing sales report"); }


        }
    }
}
