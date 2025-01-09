using Saloon.Models;
using SQLite;

namespace Saloon.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Friend>().Wait();
            _database.CreateTableAsync<Establishment>().Wait();
            _database.CreateTableAsync<Calculation>().Wait();
        }

        // Существующие методы для Friend
        public Task<List<Friend>> GetFriendsAsync()
        {
            return _database.Table<Friend>().ToListAsync();
        }

        public Task<int> SaveFriendAsync(Friend friend)
        {
            if (friend.Id != 0)
            {
                return _database.UpdateAsync(friend);
            }
            else
            {
                return _database.InsertAsync(friend);
            }
        }

        public Task<int> DeleteFriendAsync(Friend friend)
        {
            return _database.DeleteAsync(friend);
        }

        // Методы для Establishment
        public Task<List<Establishment>> GetEstablishmentsAsync()
        {
            return _database.Table<Establishment>().ToListAsync();
        }

        public Task<int> SaveEstablishmentAsync(Establishment establishment)
        {
            if (establishment.Id != 0)
            {
                return _database.UpdateAsync(establishment);
            }
            else
            {
                return _database.InsertAsync(establishment);
            }
        }

        public Task<int> DeleteEstablishmentAsync(Establishment establishment)
        {
            return _database.DeleteAsync(establishment);
        }

        public Task<List<Calculation>> GetCalculationsAsync()
        {
            return _database.Table<Calculation>().ToListAsync();
        }

        public Task<Calculation> GetCalculationAsync(int id)
        {
            return _database.Table<Calculation>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveCalculationAsync(Calculation calculation)
        {
            if (calculation.Id != 0)
            {
                return _database.UpdateAsync(calculation);
            }
            else
            {
                return _database.InsertAsync(calculation);
            }
        }

        public Task<int> DeleteCalculationAsync(Calculation calculation)
        {
            return _database.DeleteAsync(calculation);
        }
    }
}