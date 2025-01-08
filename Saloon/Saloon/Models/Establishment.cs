namespace Saloon.Models
{
    public class Establishment
    {
        public string Name { get; set; }
        public string Location { get; set; }

        // Constructor that accepts two parameters
        public Establishment(string name, string location)
        {
            Name = name;
            Location = location;
        }
    }
}