using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace ChatManager.Commands;

public class Reload
{
    
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    [RequiresPermissions("@css/config")]
    public static void Command(CCSPlayerController? player, CommandInfo info)
    {

        if (player != null || !player.IsValid || player.IsBot) return;
        player.PrintToChat($"\u200e{ChatColors.Purple}[ChatManager] {ChatColors.Green}Config Reloaded!");
        Server.PrintToConsole($"{ChatColors.Purple}[ChatManager] Config Reloaded!");
        
    }
    
}