using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static ChatManager.ConfigLoader;
using static ChatManager.MuteManager;
using static ChatManager.Commands;
using static ChatManager.DiscordUtils;
using Timer = System.Timers.Timer;

namespace ChatManager;

[MinimumApiVersion(75)]
public sealed class ChatManager : BasePlugin
{

    public override string ModuleName => "ChatManager";
    public override string ModuleAuthor => "BMathers";
    public override string ModuleVersion => "1.0.2";
    public static List<MutedPlayer>? MutedPlayers { get; private set; }
    public static JObject? Config { get; set; }
    public static string? _moduleDirectory;

    public override async void Load(bool hotReload)
    {
        
        base.Load(hotReload);
        
        Console.WriteLine(" ");
        Console.WriteLine("$$$$$$\\  $$\\   $$\\  $$$$$$\\ $$$$$$$$\\ $$\\      $$\\  $$$$$$\\  $$\\   $$\\  $$$$$$\\   $$$$$$\\  $$$$$$$$\\ $$$$$$$\\");
        Console.WriteLine("$$  __$$\\ $$ |  $$ |$$  __$$\\\\__$$  __|$$$\\    $$$ |$$  __$$\\ $$$\\  $$ |$$  __$$\\ $$  __$$\\ $$  _____|$$  __$$\\ ");
        Console.WriteLine("$$ /  \\__|$$ |  $$ |$$ /  $$ |  $$ |   $$$$\\  $$$$ |$$ /  $$ |$$$$\\ $$ |$$ /  $$ |$$ /  \\__|$$ |      $$ |  $$ |");
        Console.WriteLine("$$ |      $$$$$$$$ |$$$$$$$$ |  $$ |   $$\\$$\\$$ $$ |$$$$$$$$ |$$ $$\\$$ |$$$$$$$$ |$$ |$$$$\\ $$$$$\\    $$$$$$$  |");
        Console.WriteLine("$$ |      $$  __$$ |$$  __$$ |  $$ |   $$ \\$$$  $$ |$$  __$$ |$$ \\$$$$ |$$  __$$ |$$ |\\_$$ |$$  __|   $$  __$$< ");
        Console.WriteLine("$$ |  $$\\ $$ |  $$ |$$ |  $$ |  $$ |   $$ |\\$  /$$ |$$ |  $$ |$$ |\\$$$ |$$ |  $$ |$$ |  $$ |$$ |      $$ |  $$ |");
        Console.WriteLine("\\$$$$$$  |$$ |  $$ |$$ |  $$ |  $$ |   $$ | \\_/ $$ |$$ |  $$ |$$ | \\$$ |$$ |  $$ |\\$$$$$$  |$$$$$$$$\\ $$ |  $$ |");
        Console.WriteLine("\\______/ \\__|  \\__|\\__|  \\__|  \\__|   \\__|     \\__|\\__|  \\__|\\__|  \\__|\\__|  \\__| \\______/ \\________|\\__|  \\__|");
        Console.WriteLine("						>> Version: " + ModuleVersion);
        Console.WriteLine("						>> Author: " + ModuleAuthor);
        Console.WriteLine(" ");
        
        MutedPlayers = MuteManagerLoader(ModuleDirectory);
        Config = LoadConfig(ModuleDirectory + "/config.json");
        string? muteCommand = (string?)(ChatManager.Config?["MuteCommands"]?["MuteCommand"] ?? "mute");
        string? unmuteCommand = (string?)(ChatManager.Config?["MuteCommands"]?["UnmuteCommand"] ?? "unmute");
        string? reloadCommand = (string?)(ChatManager.Config?["MuteCommands"]?["ReloadCommand"] ?? "cm_reload");
        _moduleDirectory = ModuleDirectory;
        
        RegisterListener<Listeners.OnClientAuthorized>(OnClientAuthorized);
        RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);
        AddCommandListener("say", OnPlayerChat);
        AddCommandListener("say_team", OnPlayerChatTeam);
        
        AddCommand($"css_{muteCommand}", "Mute a player.", MuteCommand);
        AddCommand($"css_{unmuteCommand}", "Unmute a player.", UnmuteCommand);
        AddCommand($"css_{reloadCommand}", "Reload configuration.", ReloadCommand);
        
        while (true)
        {
            CheckMutedPlayers();
            await Task.Delay(30000);
        }

    }

    private void OnClientAuthorized(int playerSlot, SteamID steamId)
    {
        
        CCSPlayerController? player = Utilities.GetPlayerFromSlot(playerSlot);
        if (player == null || !player.IsValid || player.IsBot) return;
        SetPlayerTag(player);

    }

    private HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {

        CCSPlayerController? player = @event.Userid;
        if (player == null || !player.IsValid || player.IsBot) return HookResult.Continue;
        SetPlayerTag(player);
        return HookResult.Continue;

    }

    private HookResult OnPlayerChat(CCSPlayerController? player, CommandInfo info)
    {

        if (player == null || !player.IsValid) return HookResult.Continue;
        string steamid = new SteamID(player.SteamID).SteamId64.ToString();
        var checkMute = IsPlayerMuted(player);
        if (string.IsNullOrWhiteSpace(info.GetArg(1))) return HookResult.Handled;
        
        if (info.GetArg(1).StartsWith("!") || info.GetArg(1).StartsWith("/"))
        {
            if (Config?["Logs"]?["Commands"]?["Active"]?.ToString().ToLower() == "true")
            {
                _ = Task.Run(() => SendDiscord(player, info.GetArg(1), "Command", Config));
            }
            return HookResult.Continue;
        }
        
        if (checkMute)
        {
            player.PrintToChat($"\u200e{ChatColors.Purple}[ChatManager] {ChatColors.Darkred}{Config?["Localization"]?["Mute"]?["CanNotSendMessage"]}");
            return HookResult.Handled;
        }
        
        if (Config != null && Config.TryGetValue("Tags", out var Tags) && Tags is JObject tagsObject)
        {

            string deadStatus = !player.PawnIsAlive ? GetDeadTag() : "";

            if (tagsObject.TryGetValue(steamid, out var playerTag) && playerTag is JObject)
            {

                string Prefix = playerTag["Prefix"]?.ToString() ?? "";
                string? NickColor = !string.IsNullOrEmpty(playerTag?["NickColor"]?.ToString())
                    ? playerTag?["NickColor"]?.ToString()
                    : "{Default}";
                string MessageColor = playerTag?["MessageColor"]?.ToString() ?? "{Default}";

                string playerName = AdvertisementFiltering(player.PlayerName);
                Server.PrintToChatAll(ReplaceTags(
                    $"{deadStatus}\u200e{Prefix}{NickColor}{playerName}{ChatColors.Default}: {MessageColor}{info.GetArg(1)}",
                    player.TeamNum));
                
                if (Config?["Logs"]?["ChatMessages"]?["Active"]?.ToString().ToLower() == "true")
                {
                    _ = Task.Run(() => SendDiscord(player, info.GetArg(1), "Message", Config));
                }

                return HookResult.Handled;

            }

            foreach (var tagKey in tagsObject.Properties())
            {

                if (tagKey.Name.StartsWith("@"))
                {

                    string Permission = tagKey.Name;
                    bool hasPermission = AdminManager.PlayerHasPermissions(player, Permission);

                    if (hasPermission)
                    {

                        if (tagsObject.TryGetValue(Permission, out var permissionTag) && permissionTag is JObject)
                        {

                            string Prefix = permissionTag["Prefix"]?.ToString() ?? "";
                            string? NickColor = !string.IsNullOrEmpty(permissionTag?["NickColor"]?.ToString())
                                ? permissionTag["NickColor"]?.ToString()
                                : "{Default}";
                            string MessageColor = permissionTag?["MessageColor"]?.ToString() ?? "{Default}";
                            
                            string playerName = AdvertisementFiltering(player.PlayerName);
                            Server.PrintToChatAll(ReplaceTags(
                                $"{deadStatus}\u200e{Prefix}{NickColor}{playerName}{ChatColors.Default}: {MessageColor}{info.GetArg(1)}",
                                player.TeamNum));
                            
                            if (Config?["Logs"]?["ChatMessages"]?["Active"]?.ToString().ToLower() == "true")
                            {
                                _ = Task.Run(() => SendDiscord(player, info.GetArg(1), "Message", Config));
                            }
                            
                            return HookResult.Handled;

                        }
                        
                    }

                }
                
            }

            if (tagsObject.TryGetValue("everyone", out var everyoneTag) && everyoneTag is JObject)
            {
                
                string Prefix = everyoneTag["Prefix"]?.ToString() ?? "";
                string? NickColor = !string.IsNullOrEmpty(everyoneTag?["NickColor"]?.ToString())
                    ? everyoneTag["NickColor"]?.ToString()
                    : "{Default}";
                string MessageColor = everyoneTag?["MessageColor"]?.ToString() ?? "{Default}";
                            
                string playerName = AdvertisementFiltering(player.PlayerName);
                Server.PrintToChatAll(ReplaceTags(
                    $"{deadStatus}\u200e{Prefix}{NickColor}{playerName}{ChatColors.Default}: {MessageColor}{info.GetArg(1)}",
                    player.TeamNum));
                
                if (Config?["Logs"]?["ChatMessages"]?["Active"]?.ToString().ToLower() == "true")
                {
                    _ = Task.Run(() => SendDiscord(player, info.GetArg(1), "Message", Config));
                }
                
                return HookResult.Handled;
                
            }
            

        }

        return HookResult.Continue;

    }

    private HookResult OnPlayerChatTeam(CCSPlayerController? player, CommandInfo info)
    {
        
        if (player == null || !player.IsValid) return HookResult.Continue;
        string steamid = new SteamID(player.SteamID).SteamId64.ToString();
        var checkMute = IsPlayerMuted(player);
        if (string.IsNullOrWhiteSpace(info.GetArg(1))) return HookResult.Handled;
        
        if (info.GetArg(1).StartsWith("!") || info.GetArg(1).StartsWith("/"))
        {
            if (Config?["Logs"]?["Commands"]?["Active"]?.ToString().ToLower() == "true")
            {
                _ = Task.Run(() => SendDiscord(player, info.GetArg(1), "Command", Config));
            }
            return HookResult.Continue;
        }
        
        if (checkMute)
        {
            player.PrintToChat($"\u200e{ChatColors.Purple}[ChatManager] {ChatColors.Darkred}{Config?["Localization"]?["Mute"]?["CanNotSendMessage"]}");
            return HookResult.Handled;
        }
        
        if (Config != null && Config.TryGetValue("Tags", out var Tags) && Tags is JObject tagsObject)
        {

            string deadStatus = !player.PawnIsAlive ? GetDeadTag() : "";

            if (tagsObject.TryGetValue(steamid, out var playerTag) && playerTag is JObject)
            {

                string Prefix = playerTag["Prefix"]?.ToString() ?? "";
                string? NickColor = !string.IsNullOrEmpty(playerTag?["NickColor"]?.ToString())
                    ? playerTag?["NickColor"]?.ToString()
                    : "{Default}";
                string MessageColor = playerTag?["MessageColor"]?.ToString() ?? "{Default}";

                
                for (int i = 1; i <= Server.MaxPlayers; i++)
                {

                    CCSPlayerController? p = Utilities.GetPlayerFromIndex(i);
                    if (p == null || !p.IsValid || p.IsBot || p.TeamNum != player.TeamNum) continue;
                    string playerName = AdvertisementFiltering(player.PlayerName);
                    p.PrintToChat(ReplaceTags(
                        $"{deadStatus}{setTeamName(player.TeamNum)}{ChatColors.Default}\u200e{Prefix}{NickColor}{playerName}{ChatColors.Default}: {MessageColor}{info.GetArg(1)}",
                        player.TeamNum));
                    
                    if (Config?["Logs"]?["ChatMessages"]?["Active"]?.ToString().ToLower() == "true")
                    {
                        _ = Task.Run(() => SendDiscord(player, info.GetArg(1), "Message", Config));
                    }

                }

                return HookResult.Handled;

            }

            foreach (var tagKey in tagsObject.Properties())
            {

                if (tagKey.Name.StartsWith("@"))
                {

                    string Permission = tagKey.Name;
                    bool hasPermission = AdminManager.PlayerHasPermissions(player, Permission);

                    if (hasPermission)
                    {

                        if (tagsObject.TryGetValue(Permission, out var permissionTag) && permissionTag is JObject)
                        {

                            string Prefix = permissionTag["Prefix"]?.ToString() ?? "";
                            string? NickColor = !string.IsNullOrEmpty(permissionTag?["NickColor"]?.ToString())
                                ? permissionTag["NickColor"]?.ToString()
                                : "{Default}";
                            string MessageColor = permissionTag?["MessageColor"]?.ToString() ?? "{Default}";
                            
                            
                            for (int i = 1; i <= Server.MaxPlayers; i++)
                            {
                                string playerName = AdvertisementFiltering(player.PlayerName);
                                CCSPlayerController? p = Utilities.GetPlayerFromIndex(i);
                                if (p == null || !p.IsValid || p.IsBot || p.TeamNum != player.TeamNum) continue;
                                p.PrintToChat(ReplaceTags(
                                    $" {deadStatus}{setTeamName(player.TeamNum)}{ChatColors.Default}\u200e{Prefix}{NickColor}{playerName}{ChatColors.Default}: {MessageColor}{info.GetArg(1)}",
                                    player.TeamNum));
                                
                                if (Config?["Logs"]?["ChatMessages"]?["Active"]?.ToString().ToLower() == "true")
                                {
                                    _ = Task.Run(() => SendDiscord(player, info.GetArg(1), "Message", Config));
                                }

                            }
                            
                            return HookResult.Handled;

                        }
                        
                    }

                }
                
            }

            if (tagsObject.TryGetValue("everyone", out var everyoneTag) && everyoneTag is JObject)
            {
                
                string Prefix = everyoneTag["Prefix"]?.ToString() ?? "";
                string? NickColor = !string.IsNullOrEmpty(everyoneTag?["NickColor"]?.ToString())
                    ? everyoneTag["NickColor"]?.ToString()
                    : "{Default}";
                string MessageColor = everyoneTag?["MessageColor"]?.ToString() ?? "{Default}";
                            

                for (int i = 1; i <= Server.MaxPlayers; i++)
                {

                    CCSPlayerController? p = Utilities.GetPlayerFromIndex(i);
                    if (p == null || !p.IsValid || p.IsBot || p.TeamNum != player.TeamNum) continue;

                    string playerName = AdvertisementFiltering(player.PlayerName);
                    
                    p.PrintToChat(ReplaceTags(
                        $"{deadStatus}{setTeamName(player.TeamNum)}{ChatColors.Default}\u200e{Prefix}{NickColor}{playerName}{ChatColors.Default}: {MessageColor}{info.GetArg(1)}",
                        player.TeamNum));
                    
                    if (Config?["Logs"]?["ChatMessages"]?["Active"]?.ToString().ToLower() == "true")
                    {
                        _ = Task.Run(() => SendDiscord(player, info.GetArg(1), "Message", Config));
                    }

                }
                
                return HookResult.Handled;
                
            }

        }

        return HookResult.Continue;
        
    }

    private void SetPlayerTag(CCSPlayerController? player)
    {

        if (player == null || !player.IsValid || player.IsBot) return;
        string steamid = new SteamID(player.SteamID).SteamId64.ToString();

        if (Config != null && Config.TryGetValue("Tags", out var tags) && tags is JObject tagsObject)
        {
            
            string replacePlayerName = AdvertisementFiltering(player.PlayerName);
            SchemaString<CBasePlayerController> playerName = new SchemaString<CBasePlayerController>(player, "m_iszPlayerName");
            playerName.Set(replacePlayerName);
            
            if (tagsObject.TryGetValue(steamid, out var playerTag) && tags is JObject)
            {
                
                player.Clan = playerTag["ScoreboardTag"]?.ToString() ?? "";
                return;
            
            }
            
            foreach (var tagKey in tagsObject.Properties())
            {

                if (tagKey.Name.StartsWith("@"))
                {
                    
                    string Permission = tagKey.Name;
                    bool hasPermission = AdminManager.PlayerHasPermissions(player, Permission);

                    if (hasPermission)
                    {

                        if (tagsObject.TryGetValue(Permission, out var permissionTag) && permissionTag is JObject)
                        {

                            player.Clan = permissionTag["ScoreboardTag"]?.ToString() ?? "";
                            return;

                        }
                        
                    }

                }

            }

            if (Config != null && Config["Tags"]?["everyone"]?["ScoreboardTag"] != null)
            {

                player.Clan = Config["Tags"]?["everyone"]?["ScoreboardTag"]?.ToString() ?? "";

            }
            
        }

    }

    private string GetDeadTag()
    {
        
        string tag = Config?["Localization"]?["Teams"]?["Dead"]?["Tag"]?.ToString() ?? "*Dead*";
        string color = Config?["Localization"]?["Teams"]?["Dead"]?["Color"]?.ToString() ?? "{Default}";
        return $"{color}{tag}";
        
    }

    private string AdvertisementFiltering(string playerName)
    {
        
        string ipRegexPattern = @"\b(?:\d{1,3}\.){3}\d{1,3}\b";
        string urlRegexPattern = @"(?:http(s)?://)?[\w.-]+\.[a-zA-Z]{2,}(?:/\S*)?";

        playerName = Regex.Replace(playerName, ipRegexPattern, "****", RegexOptions.IgnoreCase);
        return Regex.Replace(playerName, urlRegexPattern, "****", RegexOptions.IgnoreCase);
        
    }

    private string setTeamName(int teamNum)
    {

        string teamName = "";

        switch (teamNum)
        {
            case 0:
                teamName = $"{Config?["Localization"]?["Teams"]?["None"]?["Color"]}{Config?["Localization"]?["Teams"]?["None"]?["Tag"]}";
                break;
            case 1:
                teamName = $"{Config?["Localization"]?["Teams"]?["Spec"]?["Color"]}{Config?["Localization"]?["Teams"]?["Spec"]?["Tag"]}";
                break;
            case 2:
                teamName = $"{Config?["Localization"]?["Teams"]?["T"]?["Color"]}{Config?["Localization"]?["Teams"]?["T"]?["Tag"]}";
                break;
            case 3:
                teamName = $"{Config?["Localization"]?["Teams"]?["CT"]?["Color"]}{Config?["Localization"]?["Teams"]?["CT"]?["Tag"]}";
                break;
        }

        return teamName;

    }
    
    private string TeamColor(int teamNum)
    {
        string teamColor = "";

        switch (teamNum)
        {
            case 0:
                teamColor = $"{Config?["Localization"]?["Teams"]?["None"]?["Color"]}";
                break;
            case 1:
                teamColor = $"{Config?["Localization"]?["Teams"]?["Spec"]?["Color"]}";
                break;
            case 2:
                teamColor = $"{Config?["Localization"]?["Teams"]?["T"]?["Color"]}";
                break;
            case 3:
                teamColor = $"{Config?["Localization"]?["Teams"]?["CT"]?["Color"]}";
                break;
            default:
                teamColor = "";
                break;
        }

        return teamColor;
    }
    
    private string ReplaceTags(string message, int teamNum = 0)
    {
        if (message.Contains('{'))
        {
            string modifiedValue = message;
            foreach (FieldInfo field in typeof(ChatColors).GetFields())
            {
                string pattern = $"{{{field.Name}}}";
                if (message.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    modifiedValue = modifiedValue.Replace(pattern, field.GetValue(null)!.ToString(), StringComparison.OrdinalIgnoreCase);
                }
            }
            return modifiedValue.Replace("{TEAMCOLOR}", TeamColor(teamNum));
        }

        return message;
    }

}