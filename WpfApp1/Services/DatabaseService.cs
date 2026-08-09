using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Services
{
    internal class DatabaseService
    {
        private string connectionString = "Data Source=users.db";
        public DatabaseService()
        {
            CreateTable();
        }
        private void CreateTable()
        {
            using(var connection = new SqliteConnection(connectionString)) 
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                   CREATE TABLE IF NOT EXISTS Users (
                   Id INTEGER PRIMARY KEY AUTOINCREMENT ,
                    UserName TEXT NOT NULL,
                    Password TEXT NOT NULL
                    )";
                command.ExecuteNonQuery();
            }
        }
        public void InsertUser(string username, string password)
        {
            using( var connection = new SqliteConnection(connectionString))
                {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO USERS (Username,Password)
                    VALUES($userName, $password)";
                command.Parameters.AddWithValue("$userName", username);
                command.Parameters.AddWithValue ("password", password);

                
                command.ExecuteNonQuery();
                }
            
        }
        public bool ValidateUser(string username, string password) 
        {
            using ( var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT COUNT (*) FROM Users
                    WHERE UserName = $userName AND Password = $password";
                command.Parameters.AddWithValue("$userName", username);
                command.Parameters.AddWithValue("$password", password);
                long count = (long)command.ExecuteScalar();
                return count>0;
            }
        }
    }
}
