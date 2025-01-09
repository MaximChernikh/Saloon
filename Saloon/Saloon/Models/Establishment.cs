using SQLite;

namespace Saloon.Models
{
    [Table("establishments")]
    public class Establishment
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("location")]
        public string Location { get; set; }

        public Establishment() { }

        public Establishment(string name, string location)
        {
            Name = name;
            Location = location;
        }
    }
}