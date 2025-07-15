using KalenderV6.Model;
using Microsoft.Xaml.Behaviors.Layout;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace KalenderV6.Database
{
    internal class DatabaseHelper
    {
        //TODO: Fixa database pathen!
        private static string database = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KalenderDB.db");


        // Connection string for SQLite, specifying the data source as the database file path
        private static readonly string connectionString = $"Data Source={database};Version=3;";
        //

        static DatabaseHelper() { InitializeDatabase(); }

        public static void InitializeDatabase()
        {
            Trace.WriteLine($"Database file path: {database}");

            // Check if the database file exists
            if (!File.Exists(database))
            {
                // File does not exist, create it by opening a connection
                SQLiteConnection.CreateFile(database);
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Check if the Records table exists
                string checkTableSql = "SELECT name FROM sqlite_master WHERE type='table' AND name='Events';";

                using (var checkTableCommand = new SQLiteCommand(checkTableSql, connection))
                using (var reader = checkTableCommand.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        // Table does not exist, create it
                        string createTableSql = @"
                        CREATE TABLE Events (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name VARCHAR NOT NULL,
                        TaskType VARCHAR NOT NULL,
                        StartDate VARCHAR NOT NULL,
                        Time VARCHAR,
                        Url VARCHAR,
                        IsCompleted BIT NOT NULL
                    );";

                        using (var createTableCommand = new SQLiteCommand(createTableSql, connection))
                        {
                            createTableCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static void InsertRecord(string Name, string TaskType, DateTime StartDate, DateTime time, int Howlong, string Url, bool IsCompleted)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Base SQL command
                string sql = "INSERT INTO Events (TaskType, Name, StartDate, Time, Url, IsCompleted) VALUES (@TaskType, @Name, @StartDate, @Time, @Url, @IsCompleted)";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TaskType", TaskType);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Time", time.ToString("HH:mm:ss"));
                    command.Parameters.AddWithValue("@Url", Url);
                    command.Parameters.AddWithValue("@IsCompleted", IsCompleted);
                    command.Parameters.Add("@StartDate", DbType.String);

                    // Calculate and insert records based on TaskType
                    DateTime nextDate = StartDate;

                    int iterations = Math.Max(Howlong, 1);

                    for (int i = 0; i < iterations; i++)
                    {
                        command.Parameters["@StartDate"].Value = nextDate.ToString("yyyy-MM-dd");

                        command.ExecuteNonQuery();

                        switch (TaskType)
                        {
                            case "Weekly":
                                nextDate = nextDate.AddDays(7);
                                break;
                            case "Monthly":
                                nextDate = nextDate.AddMonths(1);
                                break;
                            case "Yearly":
                                nextDate = nextDate.AddYears(1);
                                break;
                            default:
                                return;
                                //TODO: Tabort "Other" här och under CreateTask.xaml
                        }
                        nextDate = new DateTime(nextDate.Year, nextDate.Month, nextDate.Day, time.Hour, time.Minute, time.Second);
                    }
                }
            }
        }

        public static Dictionary<DateTime, string> LoadEventsForMonth(int year, int month)
        {
            var events = new Dictionary<DateTime, string>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Name, StartDate FROM Events WHERE strftime('%Y-%m', StartDate) = @yearMonth";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    string yearMonth = $"{year}-{month:D2}";
                    cmd.Parameters.AddWithValue("@yearMonth", yearMonth);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime date = DateTime.Parse(reader["StartDate"].ToString());
                            string eventName = reader["Name"].ToString();
                            events[date] = eventName;
                        }
                    }
                }
            }
            return events;
        }

        public static Dictionary<string, List<(int Id, DateTime Date, string Url, bool IsCompleted)>> LoadAllEvents()
        {
            var events = new Dictionary<string, List<(int Id, DateTime Date, string Url, bool IsCompleted)>>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Id, Name, StartDate, Url, IsCompleted FROM Events";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["Id"]);
                            string name = reader["Name"].ToString();
                            string url = reader["Url"].ToString();
                            DateTime date = DateTime.Parse(reader["StartDate"].ToString());
                            bool isCompleted = Convert.ToBoolean(reader["IsCompleted"]);

                            if (!events.ContainsKey(name))
                            {
                                events[name] = new List<(int Id, DateTime Date, string Url, bool IsCompleted)>();
                            }
                            events[name].Add((id, date, url, isCompleted));
                        }
                    }
                }
            }
            return events;
        }
        public static void CompleteEvent(string EventId) 
        {
            using(var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectSql = "SELECT IsCompleted FROM Events WHERE Id = @EventId";
                using (var selectCmd = new SQLiteCommand(selectSql, connection))
                {

                    selectCmd.Parameters.AddWithValue("@EventId", EventId);
                    bool currentStatus = Convert.ToBoolean(selectCmd.ExecuteScalar());

                    // Toggle the IsCompleted status
                    bool newStatus = !currentStatus;

                    // Update the IsCompleted status
                    string updateSql = "UPDATE Events SET IsCompleted = @NewStatus WHERE Id = @EventId";
                    using (var updateCmd = new SQLiteCommand(updateSql, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@NewStatus", newStatus);
                        updateCmd.Parameters.AddWithValue("@EventId", EventId);
                        updateCmd.ExecuteNonQuery();
                    }
                }
            }
        }
        public static List<Item> GetAllEvents()
        {
            List<Item> events = new List<Item>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Events";

                using (var command = new SQLiteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Item evt = new Item
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            TaskType = reader.IsDBNull(reader.GetOrdinal("TaskType")) ? null : reader.GetString(reader.GetOrdinal("TaskType")),
                            Url = reader.IsDBNull(reader.GetOrdinal("Url")) ? null : reader.GetString(reader.GetOrdinal("Url")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            Time = reader.GetDateTime(reader.GetOrdinal("Time")),
                            IsCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted"))
                        };
                        events.Add(evt);
                    }
                }
            }
            return events;
        }
        public static void UpdateDatabase(Item item)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Events SET Name = @Name, TaskType = @TaskType, Url = @Url, StartDate = @StartDate, Time = @Time, IsCompleted = @IsCompleted WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", item.Name);
                    command.Parameters.AddWithValue("@TaskType", item.TaskType);
                    command.Parameters.AddWithValue("@Url", item.Url);
                    command.Parameters.AddWithValue("@StartDate", item.StartDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@Time", item.Time.ToString("HH:mm:ss"));
                    command.Parameters.AddWithValue("@IsCompleted", item.IsCompleted);
                    command.Parameters.AddWithValue("@Id", item.Id);

                    command.ExecuteNonQuery();
                }
            }
        }
        public static void DeleteItem(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Events WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}