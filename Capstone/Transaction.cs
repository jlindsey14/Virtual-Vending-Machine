using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Transaction
    {
        public DateTime TimeOfTransaction { get; set; }
        public List<Animal> Inventory { get; set; }
        public List<Animal> Cart { get; set; }
        public List<string> Logs { get; set; } // Contains list of logs items that detail: DateTime, action, amount deposited/spent, new balance
        public decimal CartPrice { get; set; }
        public decimal Balance { get; set; } = 0M;
        
        public Transaction() { }


        // Purchase menu
        public void DisplayPurchaseOptions()
        {
            Console.WriteLine(@$"
Current money provided: {Balance:C2}

1. Feed Money
2. Select Product
3. Finish Transaction");

            // Defensive coding for user input to choose an option
            bool isValidOption = false;
            int option = 0;
            do
            {
                try
                {
                    option = int.Parse(Console.ReadLine());
                    isValidOption= true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Please enter 1, 2, or 3");
                }
            }
            while (!isValidOption);
            
            switch(option)
            {
                case 1:
                    FeedMoney();
                    break;
                case 2:
                    SelectProduct();
                    break;
                case 3:
                    FinalizeTransaction();
                    break;
            }
        }

    }
}
