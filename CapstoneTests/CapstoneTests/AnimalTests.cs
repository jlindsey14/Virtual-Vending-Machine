using Capstone;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTests
{
    [TestClass]
    public class AnimalTests
    {
        [TestMethod]
        public void AnimalConstructorTest()
        {
            // Arrange
            string name = "Tori the Turtle";
            string type = "Pony";
            decimal price = 5.25M;
            string slotID = "E1";

            // Act
            Animal tori = new Animal(name, type, price, slotID);

            // Assert
            Assert.AreEqual(name, tori.Name);
            Assert.AreEqual(type, tori.Type);
            Assert.AreEqual(price, tori.Price);
            Assert.AreEqual(slotID, tori.SlotID);
            Assert.AreEqual("Neigh, Neigh, Yay!", tori.DispenseMessage);

        }
    }
}
