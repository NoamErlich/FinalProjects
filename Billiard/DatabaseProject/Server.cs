using DatabaseProject.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Devices.PointOfService;
using Windows.Graphics.Printing3D;
using Windows.Media.Devices;
using Windows.Storage;
using Windows.System;

namespace DatabaseProject
{
    /// <summary>
    /// Provides methods for managing user registration, login, and game data in the database.
    /// </summary>
    public static class Server
    {       

        public static string dbPath = ApplicationData.Current.LocalFolder.Path; //computer path
        public static string connectionString = "Filename=" + dbPath + "\\DBGame.db"; //


        /// <summary>
        /// Validates the registration of a user by checking if the provided username or email already exists in the database.
        /// </summary>
        /// <param name="userName">The username to be validated.</param>
        /// <param name="userMail">The email address to be validated.</param>
        /// <returns>
        /// Returns the User ID if either the username or email already exists in the database; otherwise, returns null.
        /// </returns>
        public static int? ValidateUserRegister(string userName, string userMail)
        {
            // SQL queries to check if the provided username or email exists in the database
            string queryUserName = $"SELECT UserId FROM [User] WHERE UserName='{userName}'";
            string queryEmail = $"SELECT UserId FROM [User] WHERE UserMail='{userMail}'";

            // Establishes a connection to the SQLite database
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Executes the query to check if the provided username exists
                SqliteCommand command = new SqliteCommand(queryUserName, connection);
                SqliteDataReader reader = command.ExecuteReader();

                // If the username exists, returns the corresponding User ID
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }

                // Executes the query to check if the provided email exists
                command = new SqliteCommand(queryEmail, connection);
                reader = command.ExecuteReader();

                // If the email exists, returns the corresponding User ID
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }

                // If neither the username nor the email exists, returns null
                return null;
            }
        }


        /// <summary>
        /// Validates a user's login credentials by checking if the provided username/email 
        /// and password match an existing user record in the database.
        /// </summary>
        /// <param name="userNameORMail">The username or email address provided by the user for login.</param>
        /// <param name="userPassword">The password provided by the user for login.</param>
        /// <returns>
        /// Returns the User ID if the provided username/email and password match an existing 
        /// user record in the database; otherwise, returns null.
        /// </returns>
        public static int? ValidateUserLogin(string userNameORMail, string userPassword)
        {
            // SQL queries to check if the provided username/email and password match an existing user record in the database
            string queryUserName = $"SELECT UserId FROM [User] WHERE UserName='{userNameORMail}' AND UserPassword='{userPassword}'";
            string queryEmail = $"SELECT UserId FROM [User] WHERE UserMail='{userNameORMail}' AND UserPassword='{userPassword}'";

            // Establishes a connection to the SQLite database
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Executes the query to check if the provided username and password match
                SqliteCommand command = new SqliteCommand(queryUserName, connection);
                SqliteDataReader reader = command.ExecuteReader();

                // If the username and password match, returns the corresponding User ID
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }

                // Executes the query to check if the provided email and password match
                command = new SqliteCommand(queryEmail, connection);
                reader = command.ExecuteReader();

                // If the email and password match, returns the corresponding User ID
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }

                // If neither the username/email and password match, returns null
                return null;
            }
        }


        /// <summary>
        /// Adds a new user to the system if the provided username and 
        /// email are unique, and returns the user's information.
        /// </summary>
        /// <param name="name">The username of the new user.</param>
        /// <param name="password">The password of the new user.</param>
        /// <param name="mail">The email address of the new user.</param>
        /// <returns>
        /// Returns the GameUser object containing the information of the newly added user if the registration is successful;
        /// otherwise, returns null if the provided username or email already exists in the database.
        /// </returns>
        public static GameUser AddNewUser(string name, string password, string mail)
        {
            // Checks if the user already exists in the database
            int? userId = ValidateUserRegister(name, mail);

            // If the user already exists, returns null
            if (userId != null)
                return null;

            // If the user doesn't exist, adds the new user to the database
            string query = $"INSERT INTO [User] (UserName, UserPassword, UserMail) VALUES ('{name}', '{password}', '{mail}')";
            Execute(query);

            // Retrieves the newly added user's ID after insertion
            userId = ValidateUserRegister(name, mail);

            // Adds default game and product data for the new user
            AddGameData(userId.Value);
            AddUserProduct(userId.Value);

            // Retrieves and returns the information of the newly added user
            return GetUser(userId.Value);
        }


        /// <summary>
        /// Sets the user information from the database based on the provided user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>
        /// Returns a GameUser object containing the information of the user with the specified ID if found; otherwise, returns null.
        /// </returns>
        public static GameUser GetUser(int userId)
        {
            GameUser user = null;
            string query = $"SELECT UserId, UserName, UserMail FROM [User] WHERE UserId={userId}";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(query, connection);
                SqliteDataReader reader = command.ExecuteReader();

                // If the user with the specified ID is found, creates a GameUser object with the retrieved information
                if (reader.HasRows)
                {
                    reader.Read();
                    user = new GameUser
                    {
                        UserId = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        UserMail = reader.GetString(2),
                    };
                }
            }

            // If the user information is retrieved, continues filling additional user data
            if (user != null)
            {
                SetUser(user);
            }

            return user; // Returns the user object if found; otherwise, returns null
        }

        /// <summary>
        /// Fills additional user data such as money, current product name, and avatar pictures based on the provided GameUser object.
        /// </summary>
        /// <param name="user">The GameUser object for which additional data needs to be retrieved and set.</param>
        /// <remarks>
        /// This method queries the database to retrieve additional user-specific data stored in the [GameData] table,
        /// including the user's money balance, the name of the product they are currently using, and their avatar pictures.
        /// If data is found, it sets the corresponding properties of the provided GameUser object.
        /// </remarks>
        private static void SetUser(GameUser user)
        {
            // SQL query to retrieve additional user data based on the provided user ID
            string query = $"SELECT UserId, Money, CurrentProductName, CurrentAvatarPic, CurrentAvatarEditPic FROM [GameData] WHERE UserId={user.UserId}";

            // Establishes a connection to the SQLite database
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(query, connection);
                SqliteDataReader reader = command.ExecuteReader();

                // If additional data is found for the user, sets the corresponding properties of the GameUser object
                if (reader.HasRows)
                {
                    reader.Read();
                    user.Money = reader.GetInt32(1);
                    user.UsingProductName = reader.GetString(2);
                    user.UserAvatarPic = reader.GetString(3);
                    user.UserAvatarEditPic = reader.GetString(4);
                }
            }
        }

        /// <summary>
        /// Adds a new product to the user's inventory in the database.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="productName">The name of the product to add to the user's inventory. Default is "GreenTable".</param>
        /// <remarks>
        /// This method inserts a new record into the [UserProduct] table, associating the specified user with the provided product name.
        /// If no product name is provided, the default product name "GreenTable" is used.
        /// </remarks>
        public static void AddUserProduct(int userId, string productName = "GreenTable")
        {
            string query = $"INSERT INTO [UserProduct] (UserId, productString) VALUES ({userId}, '{productName}')";
            Execute(query);
        }


        /// <summary>
        /// Adds default game data for a new user to the database.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="currentProductName">The name of the product currently in use by the user. Default is "GreenTable".</param>
        /// <param name="money">The amount of money associated with the user. Default is 0.</param>
        /// <param name="currentAvatarPic">The file name of the user's avatar picture. Default is "DefaultAvatar".</param>
        /// <param name="currentAvatarEditPic">The file name of the user's edited avatar picture. Default is "DefaultAvatarEdit".</param>
        /// <remarks>
        /// This method inserts a new record into the [GameData] table, initializing the game-related data for a new user.
        /// By default, the user starts with the product "GreenTable", 0 money, and default avatar pictures.
        /// The method allows specifying custom values for the current product name, money, and avatar pictures if desired.
        /// </remarks>
        private static void AddGameData(int userId, string currentProductName = "GreenTable", int money = 0, string currentAvatarPic = "DefaultAvatar", string currentAvatarEditPic = "DefaultAvatarEdit")
        {
            string query = $"INSERT INTO [GameData] (UserId, currentProductName, Money, CurrentAvatarPic, CurrentAvatarEditPic) VALUES ({userId}, '{currentProductName}', {money}, '{currentAvatarPic}', '{currentAvatarEditPic}')";
            Execute(query);
        }


        /// <summary>
        /// Updates the game data for a user in the database upon exiting the game session.
        /// </summary>
        /// <param name="user">The GameUser object containing the updated game data to be saved.</param>
        /// <remarks>
        /// This method updates the user's game data stored in the [GameData] table in the database with the latest information upon exiting the game session.
        /// It updates the user's money balance, the name of the currently used product, and the avatar pictures to reflect the changes made during the game session.
        /// </remarks>
        public static void UpdateDataInExit(GameUser user)
        {
            string query = $"UPDATE [GameData] SET Money = {user.Money}, CurrentProductName = '{user.UsingProductName}', CurrentAvatarPic = '{user.UserAvatarPic}', CurrentAvatarEditPic = '{user.UserAvatarEditPic}' WHERE UserId={user.UserId}";
            Execute(query);
        }


        /// <summary>
        /// Checks if the user possesses a specific product.
        /// </summary>
        /// <param name="user">The GameUser object representing the user whose inventory is to be checked.</param>
        /// <param name="productName">The name of the product to check for in the user's inventory.</param>
        /// <returns>
        /// Returns true if the user possesses the specified product; otherwise, returns false.
        /// </returns>
        public static bool IsHavingTheProduct(GameUser user, string productName)
        {
            // SQL query to check if the user possesses the specified product
            string query = $"SELECT * FROM UserProduct WHERE UserId = {user.UserId} AND ProductString = '{productName}'";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(query, connection);
                SqliteDataReader reader = command.ExecuteReader();

                // If the query returns rows, the user possesses the product; otherwise, they do not
                if (reader.HasRows)
                {
                    return true;
                }
                return false;
            }
        }


        /// <summary>
        /// Retrieves the money balances of all players from the database and returns 
        /// them in a dictionary with player names as keys and their corresponding money 
        /// balances as values.
        /// </summary>
        /// <returns>
        /// Returns a dictionary containing player names as keys and their corresponding money balances as values.
        /// </returns>
        public static Dictionary<string, int> ReturnAllPlayersMoney()
        {
            // Dictionary to store player IDs and their money balances
            Dictionary<int, int> moneyList = new Dictionary<int, int>();

            // Dictionary to store player IDs and their names
            Dictionary<int, string> namesList = new Dictionary<int, string>();

            // Dictionary to store player names and their money balances
            Dictionary<string, int> playersList = new Dictionary<string, int>();

            // SQL query to retrieve player IDs and money balances from the [GameData] table
            string query = $"SELECT UserId, Money FROM [GameData]";

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(query, connection);
                SqliteDataReader reader = command.ExecuteReader();

                // Populates the moneyList dictionary with player IDs and their money balances
                while (reader.Read())
                {
                    moneyList.Add(reader.GetInt32(0), reader.GetInt32(1));
                }
            }

            // SQL query to retrieve player IDs and names from the [User] table
            query = $"SELECT UserId, UserName FROM [User]";

            // Establishes a connection to the SQLite database
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(query, connection);
                SqliteDataReader reader = command.ExecuteReader();

                // Populates the namesList dictionary with player IDs and their names
                while (reader.Read())
                {
                    namesList.Add(reader.GetInt32(0), reader.GetString(1));
                }
            }

            // Iterates through the moneyList dictionary to populate the playersList dictionary with player names and their money balances
            foreach (var money in moneyList)
            {
                playersList.Add(namesList[money.Key], money.Value);
            }

            return playersList;
        }


        /// <summary>
        /// Executes a SQL query that doesn't return any data, such as INSERT, UPDATE, or DELETE statements.
        /// </summary>
        /// <param name="query">The SQL query to be executed.</param>
        /// <remarks>
        /// This method opens a connection to the SQLite database using the provided connection string, executes the given SQL query,
        /// and performs the corresponding action on the database without returning any data. It is typically used for executing
        /// INSERT, UPDATE, or DELETE statements to modify database records.
        /// </remarks>
        private static void Execute(string query)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }


    }
}
