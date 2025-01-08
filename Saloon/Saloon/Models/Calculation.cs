using System;
using System.Collections.Generic;
using SQLite;

namespace Saloon.Models
{
    [Table("calculations")]
    public class Calculation
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("calculation_name")]
        public string CalculationName { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("participants")]
        public List<Friend> Participants { get; set; }


        public Calculation(string calculationName, DateTime date, List<Friend> participants, decimal totalAmount)
        {
            CalculationName = calculationName;
            Date = date;
            Participants = participants;
            TotalAmount = totalAmount;
        }

        public void CalculateDebt()
        {
            // Реализация расчета долга
        }
    }
}