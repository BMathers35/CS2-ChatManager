using System.Text;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace ChatManager.Utils;

public static class Discord
{
    
    public static async Task SendAsync(CCSPlayerController? player, string message)
    {

        string webhookUrl = ChatManager.Instance.Config.Logging.DiscordWebhook;
        if (string.IsNullOrEmpty(webhookUrl)) return;

        try
        {
            
            using var httpClient = new HttpClient();
            string formattedDateTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            var payload = new
            {
                content = "",
                tts = false,
                embeds = new[]
                {
                    new
                    {
                        type = "rich",
                        title = player?.PlayerName,
                        description = $"```\n[{formattedDateTime}] {message}\n```",
                        color = 0xff0000,
                        url = $"https://steamcommunity.com/profiles/{player?.SteamID}"
                    }
                }
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(webhookUrl, content);
            response.EnsureSuccessStatusCode();
            
        }
        catch (Exception e)
        {
            
            ChatManager.Instance.Logger.LogError($"Discord Webhook Error: {e.Message}");
            throw;
            
        }
        
    }
    
}