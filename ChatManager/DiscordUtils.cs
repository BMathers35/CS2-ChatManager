using System.Reflection;
using System.Text;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using Newtonsoft.Json.Linq;

namespace ChatManager;

public class DiscordUtils
{
    
    public static async Task SendDiscord(CCSPlayerController? player, string Message, string Type, JObject Config)
    {
        string WebhookUrl = "";

        switch (Type)
        {
            case "Message":
                WebhookUrl = Config?["Logs"]?["ChatMessages"]?["Discord_Webhook"]?.ToString() ?? "";
                break;
            case "Command":
                WebhookUrl = Config?["Logs"]?["Commands"]?["Discord_Webhook"]?.ToString() ?? "";
                break;
            default:
                WebhookUrl = "";
                break;
        }

        if (!string.IsNullOrEmpty(WebhookUrl))
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
                        description = $"```\n{Message.ToString()}\n```",
                        color = 0xff0000,
                        fields = new[]
                        {
                            new { name = $"{ChatManager.Config?["Localization"]?["Discord"]?["PlayerName"]}", value = $"{player?.PlayerName}", inline = true },
                            new { name = $"{ChatManager.Config?["Localization"]?["Discord"]?["SteamID"]}", value = $"{player?.SteamID}", inline = true },
                            new { name = $"{ChatManager.Config?["Localization"]?["Discord"]?["Profile"]}", value = $"[{ChatManager.Config?["Localization"]?["Discord"]?["ProfileLink"]}](https://steamcommunity.com/profiles/{player?.SteamID})", inline = true }
                        },
                        timestamp = $"{timestamp}",
                        author = new
                        {
                            name = $"{player?.PlayerName}",
                            icon_url = $"{ChatManager.Config?["Localization"]?["Discord"]?["PlayerImage"]}"
                        }
                    }
                }
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            await httpClient.PostAsync(WebhookUrl, content);
        }
    }
    
}