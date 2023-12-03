using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;

namespace ChatManager.Utils;

public class Discord
{
    
    public static async Task Send(CCSPlayerController? player, string message, string type)
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
                
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(webhookUrl, content);
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ChatManager] Discord Webhook Error: {e.Message}");
                throw;
            }
            
        }
    }
    
}