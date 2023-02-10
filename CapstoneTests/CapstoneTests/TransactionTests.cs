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
        [TestMethod]
        public void FeedMoneyTest()
        {
            // Arrange
            Transaction transactiontransaction = new Transaction();
            decimal input = 2.00M;

            // Act
            transactiontransaction.FeedMoney(input);

            // Assert
            Assert.AreEqual(input, transactiontransaction.Balance);
        }
    }
}
