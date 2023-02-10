using Capstone;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapstoneTests
{
    [TestClass]
    public class TransactionTests
    {
        Transaction transactiontransaction;

        [TestInitialize] 
        public void Init() 
        {
            transactiontransaction = new Transaction();
        }

        [TestMethod]
        public void FeedMoneyTest()
        {
            // Arrange
            decimal input = 2.00M;

            // Act
            transactiontransaction.FeedMoney(input);

            // Assert
            Assert.AreEqual(input, transactiontransaction.Balance);
        }

        [TestMethod]
        public void DispenseTest()
        {
            // Arrange
            string name = "Tori the Turtle";
            string type = "Pony";
            decimal price = 5.25M;
            string slotID = "E1";
            transactiontransaction.Balance = 6.00M;

            // Act
            Animal tori = new Animal(name, type, price, slotID);
            transactiontransaction.Dispense(tori);

            // Assert
            Assert.AreEqual(0.75M, transactiontransaction.Balance);
            Assert.AreEqual(4, tori.NumRemaining);
        }

        [DataTestMethod]
        [DataRow(1.00, "1 dollar")]
        [DataRow(1.15, "1 dollar, 1 dime, 1 nickel")]
        [DataRow(29.55, "29 dollars, 2 quarters, 1 nickel")]
        public void GiveChangeOutputTest(double balance, string expectedResult)
        {
            decimal decimalBalance = (decimal)balance;

            transactiontransaction.Balance = decimalBalance;
            string changeMessage = transactiontransaction.GiveChange();
            expectedResult = $"Your change is {decimalBalance:C2}. You'll recieve: {expectedResult}. Thank you!";

            Assert.AreEqual(0, transactiontransaction.Balance);
            
            Assert.AreEqual(expectedResult, changeMessage);
        }
    }
}
