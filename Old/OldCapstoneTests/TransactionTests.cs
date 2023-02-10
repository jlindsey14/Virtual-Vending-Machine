using Capstone;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capstone.Tests
{
    [TestClass]
    public class TransactionTests
    {

        [DataTestMethod]
        [DataRow(1.00, "1 dollar")]
        [DataRow(1.15, "1 dollar, 1 dime, 1 nickel")]
        [DataRow(29.55, "29 dollars, 2 quarters, 1 nickel")]
        //[DataRow(29.55, "29 dollars, 2 quarters, 1 dime")]
        public void GiveChangeOutputTest(double balance, string expectedResult)
        {
            VendingMachine VM = new VendingMachine();
            Transaction transaction = new Transaction(VM);
            decimal decimalBalance = (decimal)balance;

            transaction.Balance = decimalBalance;
            string changeMessage = transaction.GiveChange();
            expectedResult = $"Your change is {decimalBalance:C2}. You'll recieve: {expectedResult}. Thank you!";

            Assert.AreEqual(0, transaction.Balance);

            Assert.AreEqual(expectedResult, changeMessage);
        }

        [TestMethod]
        public void GiveChangeLogTest()
        {
            VendingMachine VM = new VendingMachine();
            Transaction transaction = new Transaction(VM);

            int initialLogLength = transaction.Logs.Count;

            transaction.GiveChange();

            int newLogLength = transaction.Logs.Count;

            Assert.AreEqual(newLogLength, initialLogLength + 1);
        }
    }
}
