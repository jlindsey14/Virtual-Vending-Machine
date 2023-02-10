using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OldCapstone
{
    public class VendingMachine
    {
        public List<Animal> Inventory { get; } = new List<Animal>();
        public Transaction Transaction { get; set; }
        string inputFilePath = "C:\\Users\\Student\\workspace\\c-sharp-minicapstonemodule1-team2\\vendingmachine.csv";
        string outputFilePath = "C:\\Users\\Student\\workspace\\c-sharp-minicapstonemodule1-team2\\Log.txt";
        string salesReportFilePath = "C:\\Users\\Student\\workspace\\c-sharp-minicapstonemodule1-team2\\SalesReport.txt";

        public VendingMachine()
        {
            FileRead();
        }


        public void DisplayOptions()
        {
            Console.WriteLine(@$"
Please select from the following options: (enter number)

1. Display Vending Machine Items
2. Purchase
3. Exit");

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

            switch (option)
            {
                case 1:
                    Transaction.DisplayItems();
                    DisplayOptions();
                    break;
                case 2:
                    Transaction.DisplayPurchaseOptions();
                    break;
                case 3:
                    break;
                case 4:
                    WriteSalesReport();
                    Console.WriteLine("Generating secret sales report.....");
                    break;
            }
        }

        public void FileRead()
        {
            try
            {
                using (StreamReader sr = new StreamReader(inputFilePath))
                {
                    while(!sr.EndOfStream)
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
            catch(Exception)
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
                using(StreamWriter sw = new StreamWriter(salesReportFilePath, false))
                {
                    foreach(string item in salesReportItems)
                    {
                        sw.WriteLine(item);
                    }
                    sw.WriteLine();
                    sw.WriteLine($"TOTAL SALES: {totalSale:C2}");
                }
            }
            catch(Exception)
            { Console.WriteLine("Error writing sales report"); }


        }
    }
}
