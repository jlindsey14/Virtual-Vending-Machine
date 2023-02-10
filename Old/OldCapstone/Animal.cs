using System;
using System.Collections.Generic;
using System.Text;

namespace OldCapstone
{
    public class Animal
    {
        public string Name { get; }
        public string Type { get; }
        public decimal Price { get; }
        public string SlotID { get; }
        public string DispenseMessage { get; } // Print out message that displays when an animal is purchased
        public int NumRemaining { get; set; } = 5; // Number of animals remaining in each inventory slot

        public Animal(string name, string type, decimal price, string slotID)
        {
            Name = name;
            Type = type;
            Price = price;
            SlotID = slotID;

            // Sets message depending on type of animal
            switch (Type)
            {
                case ("Duck"):
                    DispenseMessage = "Quack, Quack, Splash!";
                    break;
                case ("Penguin"):
                    DispenseMessage = "Squawk, Squawk, Whee!";
                    break;
                case ("Cat"):
                    DispenseMessage = "Meow, Meow, Meow!";
                    break;
                case ("Pony"):
                    DispenseMessage = "Neigh, Neigh, Yay!";
                    break;
            }
        }





    }
}
