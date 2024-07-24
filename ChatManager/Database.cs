using Dapper;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace ChatManager;

public static class Database
{
    private static async Task<MySqlConnection> ConnectAsync()
    {

        try
        {

            var databaseSettings = ChatManager.Instance.Config.Database;
            var connection = new MySqlConnection($"Server={databaseSettings.Host};Port={databaseSettings.Port};Username={databaseSettings.Username};Password='{databaseSettings.Password}';Database={databaseSettings.Database}");
            await connection.OpenAsync();
            return connection;

        }
        catch (Exception e)
        {
            
            ChatManager.Instance.Logger.LogError($"[Database Error]: {e}");
            throw;
            
        }
        
    }

    public static IEnumerable<dynamic> Query(string query)
    {

        try
        {

            var databaseSettings = ChatManager.Instance.Config.Database;
            using MySqlConnection connection = new MySqlConnection($"Server={databaseSettings.Host};Port={databaseSettings.Port};Username={databaseSettings.Username};Password='{databaseSettings.Password}';Database={databaseSettings.Database}");
            connection.Open();
            var connectionQuery = connection.Query(query);
            connection.Close();
            return connectionQuery;

        }
        catch (Exception e)
        {
            
            ChatManager.Instance.Logger.LogError($"[Database Error]: {e}");
            throw;

        }
        
    }

    // public static void Execute(string query, object? param)
    // {
    //
    //     try
    //     {
    //
    //         Task.Run(async () =>
    //         {
    //
    //             var connection = await ConnectAsync();
    //             await connection.ExecuteAsync(query, param);
    //             await connection.CloseAsync();
    //
    //         });
    //
    //     }
    //     catch (Exception e)
    //     {
    //         
    //         ChatManager.Instance.Logger.LogError($"[Database Error]: {e}");
    //         throw;
    //         
    //     }
    //     
    // }

    public static async Task CreateDatabase()
    {
        
        ChatManager.Instance.Logger.LogInformation($"Checking the database...");
        await using MySqlConnection connection = await ConnectAsync();
        await using MySqlTransaction transaction = await connection.BeginTransactionAsync();

        try
        {

            await connection.ExecuteAsync(@"
                    CREATE TABLE IF NOT EXISTS cm_bans (
                        id INT NOT NULL AUTO_INCREMENT,
                        steamid BIGINT NOT NULL,
                        playername VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
                        duration INT NOT NULL,
                        created_at DATETIME NOT NULL,
                        expired_at DATETIME NOT NULL,
                        PRIMARY KEY (id)
                    )", transaction: transaction);

            await connection.ExecuteAsync(@"
                    CREATE TABLE IF NOT EXISTS cm_advertisements (
                        id INT NOT NULL AUTO_INCREMENT,
                        message TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
                        duration INT NOT NULL,
                        created_at DATETIME NOT NULL,
                        PRIMARY KEY (id)
                    )", transaction: transaction);

            await transaction.CommitAsync();
            await connection.CloseAsync();
            ChatManager.Instance.Logger.LogInformation($"ChatManager database has been created.");

        }
        catch (Exception e)
        {

            await transaction.RollbackAsync();
            ChatManager.Instance.Logger.LogError($"[Database Error]: {e}");
            throw;
            
        }

    }

}