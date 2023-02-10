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
        string outputFilePath = "C:\\Users\\Student\\workspace\\c-sharp-minicapstonemodule1-team2\\Log.txt";
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
            Transaction = new Transaction();
            while (true)
            {
                //Start main menu
                Console.WriteLine(DisplayMainMenu());
                int mainMenuChoice = GetMainMenuChoice();

                //View Inventory
                if (mainMenuChoice == 1)
                {
                    Console.WriteLine(DisplayItems());
                }

                //Purchase Menu Selection
                else if (mainMenuChoice == 2)
                {
                    while (true)
                    {
                        //Start Purchase Menu
                        int purchaseChoice = 0;
                        Console.WriteLine(DisplayPurchaseMenu());
                        purchaseChoice = GetPurchaseInput();

                        //Feed Money
                        if (purchaseChoice == 1)
                        {
                            Console.WriteLine(CallPurchaseOption(1));
                        }
                        //Select Item
                        if (purchaseChoice == 2)
                        {
                            Console.WriteLine(Dispense(SlotToAnimalDictionary[CallPurchaseOption(2)]));
                        }
                        //Finalize Transaction
                        if (purchaseChoice == 3)
                        {
                            Console.WriteLine(CallPurchaseOption(3));
                            break;
                        }
                    }
                }
                //Exit
                else if (mainMenuChoice == 3)
                {
                    break;
                }
                //Secret Sales Report
                else
                {
                    Console.WriteLine(WriteSalesReport());
                }
            }   
        }

        public string DisplayMainMenu()
        {
            return @$"
Please select from the following options: (enter number)

1. Display Vending Machine Items
2. Purchase
3. Exit";
        }

        public int GetMainMenuChoice()
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
        public string DisplayPurchaseMenu()
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
                    return ReadMoney(moneyFed);
                case 2:
                    string slotID = SelectProduct();
                    if (!SlotToAnimalDictionary.ContainsKey(slotID))
                    {
                        return "Invalid Slot ID, try again.";
                    }
                    else if (SlotToAnimalDictionary[slotID].NumRemaining < 1)
                    {
                        return "Sorry, item SOLD OUT!";
                    }
                    else if (SlotToAnimalDictionary[slotID].Price > Transaction.Balance)
                    {
                        return ("Sorry, not enough funds");
                    }
                    return slotID;
                case 3:
                    return FinalizeTransaction();
            }
            return "";
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

        public string ReadMoney(decimal money)
        {
            return $"You entered {money:C2}, your total balance is {Transaction.Balance:C2}";
        }

        public string SelectProduct()
        {
            Console.WriteLine(DisplayItems());

            Console.WriteLine("\nPlease make your selection by entering the Slot ID (ex: A1)\n");
            string slotID = "";
            slotID = Console.ReadLine();
            return slotID;
        }

        public string FinalizeTransaction()
        {
            string changeGiven = Transaction.GiveChange();
            WriteLog();
            return changeGiven + "\nWelcome!";
        }

        public string Dispense(Animal animal)
        {
            Transaction.Dispense(animal);
            string purchaseString = $"You purchased {animal.Name} for {animal.Price:C2}! \n";
            string dispenseMessage = animal.DispenseMessage;
            string remainingBalance = $"\nYour remaining balance is {Transaction.Balance:C2}";
            return purchaseString + dispenseMessage + remainingBalance;

        }


        // REPORTS: 
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

        public void WriteLog()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(outputFilePath, false))
                {
                    foreach (string logitem in Transaction.Logs)
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

        public string WriteSalesReport()
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
            { return "Error writing sales report"; }
            return "Generating secret sales report.....";
        }
    }
}
