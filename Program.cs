using System;
using Microsoft.Data.Sqlite;
using System.IO;

namespace MySQLiteApp
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true){
                Console.WriteLine("1. Setup Database");
                Console.WriteLine("2. Run Application");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                
                if (choice == "1"){
                    SetupDatabase();
                    break;
                }
                else if (choice == "2")
                {
                    UserApp.Run();
                    break;
                }
                else{
                    Console.WriteLine("Invalid option. Please try again.");
                }
            }
            Console.WriteLine("Done!");
        }

        static void SetupDatabase()
        {
            string connectionString = "Data Source=mydatabase.db";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                Console.WriteLine("Setting up the database will drop the existing database if it exists.");

                string sql = File.ReadAllText("commands.txt");
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }

                Console.WriteLine("Database setup completed.");
            }
        }
    }
}
