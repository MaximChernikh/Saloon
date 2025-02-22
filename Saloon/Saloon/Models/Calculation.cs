using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

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

        [Ignore]
        public List<EstablishmentExpense> EstablishmentExpenses { get; set; }

        [Column("is_calculated")]
        public bool IsCalculated { get; set; }

        public Calculation()
        {
            EstablishmentExpenses = new List<EstablishmentExpense>();
        }

        public Calculation(string calculationName, DateTime date)
        {
            CalculationName = calculationName;
            Date = date;
            EstablishmentExpenses = new List<EstablishmentExpense>();
            IsCalculated = false;
        }

        public void CalculateDebt()
        {
            if (EstablishmentExpenses == null || EstablishmentExpenses.Count == 0)
                return;

            foreach (var establishmentExpense in EstablishmentExpenses)
            {
                if (establishmentExpense.FriendExpenses == null || establishmentExpense.FriendExpenses.Count == 0 || establishmentExpense.Payer == null)
                    continue;

                decimal totalExpense = establishmentExpense.FriendExpenses.Sum(fe => fe.Amount);
                int participantCount = establishmentExpense.FriendExpenses.Count;

                foreach (var friendExpense in establishmentExpense.FriendExpenses)
                {
                    decimal shouldPay = totalExpense / participantCount;
                    decimal debt = shouldPay - friendExpense.Amount;

                    if (friendExpense.Friend.Id == establishmentExpense.Payer.Friend.Id)
                    {
                        friendExpense.Friend.Debt += establishmentExpense.FriendExpenses
                            .Where(fe => fe.Friend.Id != establishmentExpense.Payer.Friend.Id)
                            .Sum(fe => shouldPay - fe.Amount);
                    }
                    else
                    {
                        friendExpense.Friend.Debt += debt;
                    }
                }
            }

            IsCalculated = true;
        }
    }

    public class EstablishmentExpense
    {
        public Establishment Establishment { get; set; }
        public FriendExpense Payer { get; set; }
        public List<FriendExpense> FriendExpenses { get; set; }

        public EstablishmentExpense()
        {
            FriendExpenses = new List<FriendExpense>();
        }
    }
    public class FriendExpense
    {
        public Friend Friend { get; set; }
        public decimal Amount { get; set; }
    }
}