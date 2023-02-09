using Capstone;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capstone.Tests
{
    [TestClass]
    public class TransactionTests
    {
        [DataTestMethod]
        [DataRow()]
        public void GiveChangeTest(decimal balance, string expectedResult)
        {
            VendingMachine VM = new VendingMachine();
            Transaction transaction = new Transaction(VM);
        }
    }
}
