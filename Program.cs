using Microsoft.Data.Sqlite;


// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddControllersWithViews();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

// app.Run();

using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace MySQLiteApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=mydatabase.db";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                Console.WriteLine("Making the database will drop the existing database if it exists.");
                // Read SQL commands from file
                string sql = File.ReadAllText("commands.txt");
                // Execute SQL commands
                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }

                // Query data to verify inserts
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = "SELECT username, passwordSalt, passwordHash, firstName, lastName, ID FROM Users";
                using (var reader = selectCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var username = reader.GetString(0);
                        var passwordSalt = reader.GetString(1);
                        Console.WriteLine($"userName: {username}, passwordSalt: {passwordSalt}, passwordHash: {reader.GetString(2)}, firstName: {reader.GetString(3)}, lastName: {reader.GetString(4)} ID: {reader.GetInt32(5)}");
                    }
                }
            }

            Console.WriteLine("Done!");
        }
    }
}
