using System.Text;
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

        if (!string.IsNullOrEmpty(webhookUrl))
        {
            var httpClient = new HttpClient();
            DateTimeOffset now = DateTimeOffset.Now;
            string timestamp = now.ToString("yyyy-MM-ddTHH:mm:ss.fffK");

            var payload = new
            {
                content = "",
                tts = false,
                embeds = new[]
                {
                    new
                    {
                        type = "rich",
                        title = $"{ConVar.Find("hostname")!.StringValue}",
                        description = $"```\n{message}\n```",
                        color = 0xff0000,
                        fields = new[]
                        {
                            new { name = $"{ChatManager._config?.Messages.Discord.PlayerName}", value = $"{player?.PlayerName}", inline = true },
                            new { name = $"{ChatManager._config?.Messages.Discord.SteamId}", value = $"{player?.SteamID}", inline = true },
                            new { name = $"{ChatManager._config?.Messages.Discord.Profile}", value = $"[{ChatManager._config?.Messages.Discord.ProfileLink}](https://steamcommunity.com/profiles/{player?.SteamID})", inline = true }
                        },
                        timestamp = $"{timestamp}",
                        author = new
                        {
                            name = $"{player?.PlayerName}",
                            icon_url = $"{ChatManager._config?.Messages.Discord.PlayerImage}"
                        }
                    }
                }
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            await httpClient.PostAsync(webhookUrl, content);
        }
    }
    
}