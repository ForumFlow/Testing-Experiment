using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;

class UserApp
{
    private static string connectionString = "Data Source=mydatabase.db";

    private static string GetHash(string text)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    private static string GetSalt()
    {
        var randomNumber = new byte[32];
        RandomNumberGenerator.Fill(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private static void Register(string username, string password, string firstName, string lastName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = "SELECT COUNT(*) FROM Users WHERE username = $username";
            checkCmd.Parameters.AddWithValue("$username", username);

            long userExists = (long)checkCmd.ExecuteScalar();

            if (userExists > 0)
            {
                Console.WriteLine("\nUsername already exists. Please choose a different username or login with a previous account");
                return;
            }

            string salt = GetSalt();
            string hashedPassword = GetHash(password + salt);

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = "INSERT INTO Users (username, passwordSalt, passwordHash, firstName, lastName) VALUES ($username, $salt, $hashedPassword, $firstName, $lastName)";
            insertCmd.Parameters.AddWithValue("$username", username);
            insertCmd.Parameters.AddWithValue("$salt", salt);
            insertCmd.Parameters.AddWithValue("$hashedPassword", hashedPassword);
            insertCmd.Parameters.AddWithValue("$firstName", firstName);
            insertCmd.Parameters.AddWithValue("$lastName", lastName);

            insertCmd.ExecuteNonQuery();

            Console.WriteLine("\nUser registered successfully.");
        }
    }

    private static bool Authenticate(string username, string password)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT passwordSalt, passwordHash FROM Users WHERE username = $username";
            selectCmd.Parameters.AddWithValue("$username", username);

            using (var reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string salt = reader.GetString(0);
                    string storedHash = reader.GetString(1);
                    string hashedPassword = GetHash(password + salt);
                    return hashedPassword == storedHash;
                }
            }
        }
        return false;
    }

    public static void Run()
    {
        Console.WriteLine("Welcome to User Authentication Demo!");

        while (true)
        {
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter username: ");
                string username = Console.ReadLine();

                Console.Write("Enter password: ");
                string password = ReadPassword();

                Console.Write("\nEnter first name: ");
                string firstName = Console.ReadLine();

                Console.Write("Enter last name: ");
                string lastName = Console.ReadLine();

                Register(username, password, firstName, lastName);
            }
            else if (choice == "2")
            {
                Console.Write("Enter username: ");
                string username = Console.ReadLine();

                Console.Write("Enter password: ");
                string password = ReadPassword();

                if (Authenticate(username, password))
                {
                    Console.WriteLine("\nLogin successful!");
                }
                else
                {
                    Console.WriteLine("\nInvalid username or password. Please try again.");
                }
            }
            else if (choice == "3")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid option. Please try again.");
            }
        }
    }

    private static string ReadPassword()
    {
        string password = string.Empty;
        ConsoleKey key;

        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && password.Length > 0)
            {
                Console.Write("\b \b");
                password = password[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                password += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);

        return password;
    }
}
