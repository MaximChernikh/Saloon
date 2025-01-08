namespace Saloon.Models
{
    public class Friend
    {
        public string Name { get; set; }

        // Constructor that accepts two parameters
        public Friend(string name)
        {
            Name = name;
        }
    }
}