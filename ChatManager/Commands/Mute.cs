using ChatManager.Utils;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using Newtonsoft.Json;
using static ChatManager.MuteManager;

namespace ChatManager.Commands;

public class Mute
{
    
    [CommandHelper(minArgs: 2, usage: "[SteamID64 or name] <duration>", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
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

            var muteDuration = info.ArgByIndex(2) != null ? int.Parse(info.ArgByIndex(2)) : (int?)0;
            foreach (CCSPlayerController targetPlayer in matchingPlayers)
            {

                if (IsPlayerMuted(targetPlayer))
                {

                    // {PLAYERNAME} isimli oyuncu zaten susturulmuş.
                    var replace =
                        $"{ChatColors.Purple}[ChatManager] {ChatColors.Darkred}{ChatManager._config?.Messages.AlreadyMuted}"
                            .Replace("{PLAYERNAME}", targetPlayer.PlayerName);
                    player.PrintToChat(Colors.Tags(replace));
                    
                }
                else
                {
                    
                    var mutedPlayers = MutePlayer(player, (int)muteDuration);
                    if (mutedPlayers != null)
                    {
                        File.WriteAllText( ChatManager._moduleDirectory + "/muted_players.json", JsonConvert.SerializeObject(mutedPlayers));
                        // {PLAYERNAME} isimli oyuncu {DURATION} saniye boyunca susturuldu.
                        var replace =
                            $"{ChatColors.Purple}[ChatManager] {ChatColors.Green}{ChatManager._config?.Messages.SilencedForSeconds}"
                                .Replace("{PLAYERNAME}", targetPlayer.PlayerName)
                                .Replace("{DURATION}", muteDuration.ToString());
                        player.PrintToChat(Colors.Tags(replace));
                    }
                    
                }
                
            }
            
        }
        else
        {
            player.PrintToChat($"{ChatColors.Purple}[ChatManager] {ChatColors.Darkred}{ChatManager._config?.Messages.SilencedForSeconds}");
        }

    }
    
}