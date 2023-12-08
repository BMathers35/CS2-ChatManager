using System.Text;
using CounterStrikeSharp.API.Core;

namespace ChatManager.Utils;

public class Discord
{
    
    public static void Send(CCSPlayerController? player, string message, string type)
    {
        string webhookUrl;

            switch (type)
            {
                case "Message":
                    webhookUrl = ChatManager._config?.DiscordWebhooks.MessagesWebhook ?? "";
                    break;
                case "Command":
                    webhookUrl = ChatManager._config?.DiscordWebhooks.CommandsWebhook ?? "";
                    break;
                default:
                    webhookUrl = "";
                    break;
            }
            
            Console.WriteLine($"Webhook URL: {webhookUrl}");

            if (!string.IsNullOrEmpty(webhookUrl))
            {

                try
                {
                    
                    var httpClient = new HttpClient();
                    DateTime now = DateTime.Now;
                    string formattedDateTime = now.ToString("dd-MM-yyyy HH:mm:ss");
                    
                    var payload = new
                    {
                        username = "ChatManager",
                        content = "",
                        tts = false,
                        embeds = new[]
                        {
                            new
                            {
                                type = "rich",
                                title = $"{player.PlayerName}",
                                description = $"```\n[{formattedDateTime}] {message.ToString()}\n```",
                                color = 0xff0000,
                                url = $"https://steamcommunity.com/profiles/{player?.SteamID}"
                            }
                        }
                    };
                    
                    Task.Run(async () =>
                    {
                        
                        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await httpClient.PostAsync(webhookUrl, content);
                        response.EnsureSuccessStatusCode();
                        string result = await response.Content.ReadAsStringAsync();
                        
                        if (ChatManager._config != null && ChatManager._config.GeneralSettings.DebugDiscordWebhook)
                        {
                            Console.WriteLine($"[ChatManager] Result of HTTP Request: {result}");
                        }
                        
                    });
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[ChatManager] Discord Webhook Error: {e.Message}");
                    throw;
                }
            
            }
        
    }
    
}