using ChatManager.Utils;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using Newtonsoft.Json.Linq;

namespace ChatManager.Events;

public class OnPlayerTeamChat
{
    
    public static HookResult Run(CCSPlayerController? player, CommandInfo info)
    {
        
        // if (player == null || !player.IsValid) return HookResult.Continue;
        // string steamid = new SteamID(player.SteamID).SteamId64.ToString();
        // var checkMute = MuteManager.IsPlayerMuted(player);
        // if (string.IsNullOrWhiteSpace(info.GetArg(1))) return HookResult.Handled;
        //
        // if (info.GetArg(1).StartsWith("!") || info.GetArg(1).StartsWith("/"))
        // {
        //     if (Config?["Logs"]?["Commands"]?["Active"]?.ToString().ToLower() == "true")
        //     {
        //         _ = Task.Run(() => Utils.Discord.Send(player, info.GetArg(1), "Command", Config));
        //     }
        //     return HookResult.Continue;
        // }
        //
        // if (checkMute)
        // {
        //     player.PrintToChat($"\u200e{ChatColors.Purple}[ChatManager] {ChatColors.Darkred}{Config?["Localization"]?["Mute"]?["CanNotSendMessage"]}");
        //     return HookResult.Handled;
        // }
        //
        // if (Config != null && Config.TryGetValue("Tags", out var Tags) && Tags is JObject tagsObject)
        // {
        //
        //     string deadStatus = !player.PawnIsAlive ? Utils.Helpers.GetDeadTag() : "";
        //
        //     if (tagsObject.TryGetValue(steamid, out var playerTag) && playerTag is JObject)
        //     {
        //
        //         string Prefix = playerTag["Prefix"]?.ToString() ?? "";
        //         string? NickColor = !string.IsNullOrEmpty(playerTag?["NickColor"]?.ToString())
        //             ? playerTag?["NickColor"]?.ToString()
        //             : "{Default}";
        //         string MessageColor = playerTag?["MessageColor"]?.ToString() ?? "{Default}";
        //
        //         
        //         for (int i = 1; i <= Server.MaxPlayers; i++)
        //         {
        //
        //             CCSPlayerController? p = Utilities.GetPlayerFromIndex(i);
        //             if (p == null || !p.IsValid || p.IsBot || p.TeamNum != player.TeamNum) continue;
        //             string playerName = Utils.Helpers.AdvertisementFiltering(player.PlayerName);
        //             p.PrintToChat(Colors.Tags(
        //                 $"{deadStatus}{Utils.Helpers.setTeamName(player.TeamNum)}{ChatColors.Default}\u200e{Prefix}{NickColor}{playerName}{ChatColors.Default}: {MessageColor}{info.GetArg(1)}",
        //                 player.TeamNum));
        //             
        //             if (Config?["Logs"]?["ChatMessages"]?["Active"]?.ToString().ToLower() == "true")
        //             {
        //                 _ = Task.Run(() => Utils.Discord.Send(player, info.GetArg(1), "Message", Config));
        //             }
        //
        //         }
        //
        //         return HookResult.Handled;
        //
        //     }
        //
        //     foreach (var tagKey in tagsObject.Properties())
        //     {
        //
        //         if (tagKey.Name.StartsWith("@"))
        //         {
        //
        //             string Permission = tagKey.Name;
        //             bool hasPermission = AdminManager.PlayerHasPermissions(player, Permission);
        //
        //             if (hasPermission)
        //             {
        //
        //                 if (tagsObject.TryGetValue(Permission, out var permissionTag) && permissionTag is JObject)
        //                 {
        //
        //                     string Prefix = permissionTag["Prefix"]?.ToString() ?? "";
        //                     string? NickColor = !string.IsNullOrEmpty(permissionTag?["NickColor"]?.ToString())
        //                         ? permissionTag["NickColor"]?.ToString()
        //                         : "{Default}";
        //                     string MessageColor = permissionTag?["MessageColor"]?.ToString() ?? "{Default}";
        //                     
        //                     
        //                     for (int i = 1; i <= Server.MaxPlayers; i++)
        //                     {
        //                         string playerName = Utils.Helpers.AdvertisementFiltering(player.PlayerName);
        //                         CCSPlayerController? p = Utilities.GetPlayerFromIndex(i);
        //                         if (p == null || !p.IsValid || p.IsBot || p.TeamNum != player.TeamNum) continue;
        //                         p.PrintToChat(Colors.Tags(
        //                             $" {deadStatus}{Utils.Helpers.setTeamName(player.TeamNum)}{ChatColors.Default}\u200e{Prefix}{NickColor}{playerName}{ChatColors.Default}: {MessageColor}{info.GetArg(1)}",
        //                             player.TeamNum));
        //                         
        //                         if (Config?["Logs"]?["ChatMessages"]?["Active"]?.ToString().ToLower() == "true")
        //                         {
        //                             _ = Task.Run(() => Utils.Discord.Send(player, info.GetArg(1), "Message", Config));
        //                         }
        //
        //                     }
        //                     
        //                     return HookResult.Handled;
        //
        //                 }
        //                 
        //             }
        //
        //         }
        //         
        //     }
        //
        //     if (tagsObject.TryGetValue("everyone", out var everyoneTag) && everyoneTag is JObject)
        //     {
        //         
        //         string Prefix = everyoneTag["Prefix"]?.ToString() ?? "";
        //         string? NickColor = !string.IsNullOrEmpty(everyoneTag?["NickColor"]?.ToString())
        //             ? everyoneTag["NickColor"]?.ToString()
        //             : "{Default}";
        //         string MessageColor = everyoneTag?["MessageColor"]?.ToString() ?? "{Default}";
        //                     
        //
        //         for (int i = 1; i <= Server.MaxPlayers; i++)
        //         {
        //
        //             CCSPlayerController? p = Utilities.GetPlayerFromIndex(i);
        //             if (p == null || !p.IsValid || p.IsBot || p.TeamNum != player.TeamNum) continue;
        //
        //             string playerName = Utils.Helpers.AdvertisementFiltering(player.PlayerName);
        //             
        //             p.PrintToChat(Colors.Tags(
        //                 $"{deadStatus}{Utils.Helpers.setTeamName(player.TeamNum)}{ChatColors.Default}\u200e{Prefix}{NickColor}{playerName}{ChatColors.Default}: {MessageColor}{info.GetArg(1)}",
        //                 player.TeamNum));
        //             
        //             if (Config?["Logs"]?["ChatMessages"]?["Active"]?.ToString().ToLower() == "true")
        //             {
        //                 _ = Task.Run(() => Utils.Discord.Send(player, info.GetArg(1), "Message", Config));
        //             }
        //
        //         }
        //         
        //         return HookResult.Handled;
        //         
        //     }
        //
        // }

        return HookResult.Continue;
        
    }
    
}