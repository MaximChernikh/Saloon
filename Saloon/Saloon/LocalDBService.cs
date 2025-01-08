using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saloon.Models;


namespace Saloon
{
    public class LocalDBService
    {
        private const string DB_NAME = "SaloonDB.db";
        readonly SQLite.SQLiteConnection _connection;

        public LocalDBService()
        {
            _connection = new SQLite.SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTable<Friend>();
            _connection.CreateTable<Establishment>();
            _connection.CreateTable<Calculation>();
        }
    }
}