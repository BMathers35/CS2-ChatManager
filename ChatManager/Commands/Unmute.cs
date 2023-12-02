using ChatManager.Utils;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using static ChatManager.MuteManager;

namespace ChatManager.Commands;

public class Unmute
{
    
    [CommandHelper(minArgs: 1, usage: "[SteamID64 or name]", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    [RequiresPermissions("@css/chat")]
    public static void Command(CCSPlayerController? player, CommandInfo info)
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
            player.PrintToChat($"{ChatColors.Purple}[ChatManager] {ChatColors.Darkred}{ChatManager._config?.Messages.NoTarget}");
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
                            $"{ChatColors.Purple}[ChatManager] {ChatColors.Green}{ChatManager._config?.Messages.UnmutePlayer}"
                                .Replace("{PLAYERNAME}", targetPlayer.PlayerName);
                        player.PrintToChat(Colors.Tags(replace));
                        
                        if (targetPlayer.IsValid || !player.IsBot)
                        {
                            // {player.PlayerName} isimli yetkili susturmanızı kaldırdı.
                            var replaceTarget =
                                $"{ChatColors.Purple}[ChatManager] {ChatColors.Green}{ChatManager._config?.Messages.UnmutePlayerInfo}"
                                    .Replace("{ADMIN}", player.PlayerName);
                            targetPlayer.PrintToChat(Colors.Tags(replaceTarget));
                            
                        }
                    }
                    else
                    {
                        // {targetPlayer.PlayerName} oyuncusunun sohbet yasaklaması kaldırılamadı!
                        var replace =
                            $"{ChatColors.Purple}[ChatManager] {ChatColors.Darkred}{ChatManager._config?.Messages.CouldNotRemove}"
                                .Replace("{PLAYERNAME}", targetPlayer.PlayerName);
                        player.PrintToChat(Colors.Tags(replace));
                        
                    }
                }
                else
                {
                    // {targetPlayer.PlayerName} isimli oyuncunun cezası sonlanmış veya hiç susturulmamış.
                    var replace =
                        $"{ChatColors.Purple}‎[ChatManager] {ChatColors.Darkred}{ChatManager._config?.Messages.NotBanned}"
                            .Replace("{PLAYERNAME}", targetPlayer.PlayerName);
                    player.PrintToChat(Colors.Tags(replace));
                    
                }
            }
        }
        else
        {
            
            player.PrintToChat($"{ChatColors.Purple}[ChatManager] {ChatColors.Darkred}{ChatManager._config?.Messages.MultipleTargetsFound}");
            
        }
        
    }
    
}