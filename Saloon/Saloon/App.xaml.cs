using Saloon.Services;

namespace Saloon
{
    public partial class App : Application
    {
        static DatabaseService Database;
        public static DatabaseService DatabaseInstance
        {
            get
            {
                if (Database == null)
                {
                    Database = new DatabaseService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Friends.db3"));
                }
                return Database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}