using System.Reflection;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static ChatManager.MuteManager;

namespace ChatManager;

public abstract class Commands : BasePlugin
{
    
    [CommandHelper(minArgs: 2, usage: "[SteamID64 or name] <duration>", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    [RequiresPermissions("@css/chat")]
    public static void MuteCommand(CCSPlayerController? player, CommandInfo info)
    {

        if (player == null || !player.IsValid || player.IsBot) return;
        List<CCSPlayerController> players = Utilities.GetPlayers();
        List<CCSPlayerController> matchingPlayers = new List<CCSPlayerController>();

        foreach (CCSPlayerController target in players)
        {
            string targetSteamId = new SteamID(target.SteamID).SteamId64.ToString();
            if (info.ArgByIndex(1) == targetSteamId || target.PlayerName.Contains(info.ArgByIndex(1)))
            {
                matchingPlayers.Add(target);
            }
        }

        if (matchingPlayers.Count == 0)
        {
            player.PrintToChat($"{ChatColors.Purple}[ChatManager] {ChatColors.Darkred}{ChatManager.Config?["Localization"]?["Mute"]?["NoTarget"]}");
        }
        else if (matchingPlayers.Count == 1)
        {

            var muteDuration = info.ArgByIndex(2) != null ? int.Parse(info.ArgByIndex(2)) : (int?)0;
            foreach (CCSPlayerController targetPlayer in matchingPlayers)
            {

                if (IsPlayerMuted(targetPlayer))
                {

                    // {PLAYERNAME} isimli oyuncu zaten susturulmuş.
                    var replace =
                        $"{ChatColors.Purple}[ChatManager] {ChatColors.Darkred}{ChatManager.Config?["Localization"]?["Mute"]?["AlreadyMuted"]}"
                            .Replace("{PLAYERNAME}", targetPlayer.PlayerName);
                    player.PrintToChat(ReplaceTags(replace));
                    
                }
                else
                {
                    
                    var mutedPlayers = MutePlayer(player, (int)muteDuration);
                    if (mutedPlayers != null)
                    {
                        File.WriteAllText( ChatManager._moduleDirectory + "/muted_players.json", JsonConvert.SerializeObject(mutedPlayers));
                        // {PLAYERNAME} isimli oyuncu {DURATION} saniye boyunca susturuldu.
                        var replace =
                            $"{ChatColors.Purple}[ChatManager] {ChatColors.Green}{ChatManager.Config?["Localization"]?["Mute"]?["SilencedForSeconds"]}"
                                .Replace("{PLAYERNAME}", targetPlayer.PlayerName)
                                .Replace("{DURATION}", muteDuration.ToString());
                        player.PrintToChat(ReplaceTags(replace));
                    }
                    
                }
                
            }
            
        }
        else
        {
            player.PrintToChat($"{ChatColors.Purple}[ChatManager] {ChatColors.Darkred}{ChatManager.Config?["Localization"]?["Mute"]?["SilencedForSeconds"]}");
        }

    }
    
    [CommandHelper(minArgs: 1, usage: "[SteamID64 or name]", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    [RequiresPermissions("@css/chat")]
    public static void UnmuteCommand(CCSPlayerController? player, CommandInfo info)
    {

        if (player == null || !player.IsValid || player.IsBot) return;
        List<CCSPlayerController> players = Utilities.GetPlayers();
        List<CCSPlayerController> matchingPlayers = new List<CCSPlayerController>();

        foreach (CCSPlayerController target in players)
        {
            string targetSteamId = new SteamID(target.SteamID).SteamId64.ToString();
            if (info.ArgByIndex(1) == targetSteamId || target.PlayerName.Contains(info.ArgByIndex(1)))
            {
                matchingPlayers.Add(target);
            }
        }

        if (matchingPlayers.Count == 0)
        {
            player.PrintToChat($"{ChatColors.Purple}[ChatManager] {ChatColors.Darkred}{ChatManager.Config?["Localization"]?["Mute"]?["NoTarget"]}");
        }
        else if (matchingPlayers.Count == 1)
        {

            foreach (CCSPlayerController targetPlayer in matchingPlayers)
            {

                var CheckMuted = IsPlayerMuted(targetPlayer);
                if (CheckMuted)
                {

                    var unmuteProcess = UnmutePlayer(targetPlayer);
                    if (unmuteProcess)
                    {
                        // {targetPlayer.PlayerName} isimli oyuncunun susturulması kaldırıldı.
                        var replace =
                            $"{ChatColors.Purple}[ChatManager] {ChatColors.Green}{ChatManager.Config?["Localization"]?["Mute"]?["UnmutePlayer"]}"
                                .Replace("{PLAYERNAME}", targetPlayer.PlayerName);
                        player.PrintToChat(ReplaceTags(replace));
                        
                        if (targetPlayer.IsValid || !player.IsBot)
                        {
                            // {player.PlayerName} isimli yetkili susturmanızı kaldırdı.
                            var replaceTarget =
                                $"{ChatColors.Purple}[ChatManager] {ChatColors.Green}{ChatManager.Config?["Localization"]?["Mute"]?["UnmutePlayerInfo"]}"
                                    .Replace("{ADMIN}", player.PlayerName);
                            targetPlayer.PrintToChat(ReplaceTags(replaceTarget));
                            
                        }
                    }
                    else
                    {
                        // {targetPlayer.PlayerName} oyuncusunun sohbet yasaklaması kaldırılamadı!
                        var replace =
                            $"{ChatColors.Purple}[ChatManager] {ChatColors.Darkred}{ChatManager.Config?["Localization"]?["Mute"]?["CouldNotRemove"]}"
                                .Replace("{PLAYERNAME}", targetPlayer.PlayerName);
                        player.PrintToChat(ReplaceTags(replace));
                        
                    }
                }
                else
                {
                    // {targetPlayer.PlayerName} isimli oyuncunun cezası sonlanmış veya hiç susturulmamış.
                    var replace =
                        $"{ChatColors.Purple}‎[ChatManager] {ChatColors.Darkred}{ChatManager.Config?["Localization"]?["Mute"]?["NotBanned"]}"
                            .Replace("{PLAYERNAME}", targetPlayer.PlayerName);
                    player.PrintToChat(ReplaceTags(replace));
                    
                }
            }
        }
        else
        {
            
            player.PrintToChat($"{ChatColors.Purple}[ChatManager] {ChatColors.Darkred}{ChatManager.Config?["Localization"]?["Mute"]?["MultipleTargetsFound"]}");
            
        }
        
    }
    
    private static string ReplaceTags(string message, int teamNum = 0)
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
        }

        return "\u200e" + message;
    }
    
}