using SQLite;
using System;
using System.Collections.Generic;

namespace Saloon.Models
{
    [Table("calculations")]
    public class Calculation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("calculation_name")]
        public string CalculationName { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        // Мы не можем напрямую хранить список участников в SQLite,
        // поэтому мы будем хранить их отдельно и связывать по ID расчета

        public Calculation() { }

        public Calculation(string calculationName, DateTime date, decimal totalAmount)
        {
            CalculationName = calculationName;
            Date = date;
            TotalAmount = totalAmount;
        }

        public void CalculateDebt()
        {
            // Реализация расчета долга
        }
    }
}