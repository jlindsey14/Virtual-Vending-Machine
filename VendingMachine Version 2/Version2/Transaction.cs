using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone
{
    public class Transaction
    {
        public List<string> Logs { get; set; } = new List<string>();// Contains list of logs items that detail: DateTime, action, amount deposited/spent, new balance
        public decimal Balance { get; set; } = 0M;

        public void FeedMoney(decimal input)
        {
            Balance += input;
            AddToLog("Feed Money", input);
        }

        public void Dispense(Animal animal)
        {
            Balance -= animal.Price;
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
            AddToLog("Give Change", totalChangeDispensed);
            return changeMessage;
        }

        public void AddToLog(string actionName, decimal decimalAmount)
        {
            string logMessage = $"{DateTime.Now} {actionName} {decimalAmount:C2} {Balance:C2}";
            Logs.Add(logMessage);
        }

    }       
}
