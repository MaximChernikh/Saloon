using SQLite;

namespace Saloon.Models
{
    [Table("friends")]
    public class Friend
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        public Friend() { }

        public Friend(string name)
        {
            Name = name;
        }
    }
}