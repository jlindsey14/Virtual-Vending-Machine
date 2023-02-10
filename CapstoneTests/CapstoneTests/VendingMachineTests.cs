using Capstone;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapstoneTests
{
    [TestClass]
    public class VendingMachineTests
    {
        VendingMachine vm;

        [TestInitialize]
        public void Init()
        {
            vm = new VendingMachine();
        }

        [DataTestMethod]
        [DataRow("Cube Duck", "Duck", 2.50, "A2")]
        [DataRow("Tabby Cat", "Cat", 2.50, "C3")]
        [DataRow("Horse", "Pony", 0.90, "D3")]
        public void ConstructorTest(string name, string type, double price, string slotID)
        {
            // Arrange
            decimal decimalPrice = (decimal)price;
            Animal animal = new Animal(name, type, decimalPrice, slotID);

            // Act
            Animal dictionaryValue = vm.SlotToAnimalDictionary[animal.SlotID];

            // Assert
            Assert.AreEqual(dictionaryValue.Name, animal.Name);
            Assert.AreEqual(dictionaryValue.Price, animal.Price);
            Assert.AreEqual(dictionaryValue.Type, animal.Type);
            Assert.AreEqual(dictionaryValue.SlotID, animal.SlotID);
        }
    }
}
