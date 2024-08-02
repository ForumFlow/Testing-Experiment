using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;


class UserAuthProgram
{
    private static Dictionary<string, User> users = new Dictionary<string, User>();

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

    private static void Register(string username, string password)
    {

        if (users.ContainsKey(username)){
            Console.WriteLine("\nUsername already exists. Please choose a different username or login with a previous account");
            return;
        }

        string salt = GetSalt();
        string hashedPassword = GetHash(password + salt);

        users[username] = new User(username, salt, hashedPassword);

        Console.WriteLine("\nUser registered successfully.");
    }

    private static bool Authenticate(string username, string password)
    {
        if (users.TryGetValue(username, out User user))
        {
            string hashedPassword = GetHash(password + user.Salt);
            return hashedPassword == user.HashedPassword;
        }
        return false;
    }

    static void Main(string[] args)
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

                Register(username, password);
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
