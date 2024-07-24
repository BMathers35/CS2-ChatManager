using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using Newtonsoft.Json;

namespace ChatManager.Commands;

public abstract class Commands
{

    public static void Load()
    {
        
        ChatManager.Instance.AddCommand($"css_{ChatManager.Instance.Config.Commands.AdsReload}", "Reload advertising data", AdsReload);
        
    }

    public static void Unload()
    {
        
        ChatManager.Instance.RemoveCommand($"css_{ChatManager.Instance.Config.Commands.AdsReload}", AdsReload);
        
    }

    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    [RequiresPermissions("@css/chat")]
    private static void AdsReload(CCSPlayerController? player, CommandInfo info)
    {
        
        Advertisements.LoadAdvertisements();
        
        if(player != null && player.IsValid)
            player.PrintToChat(Utils.Helpers.MessageBuilder("Reloaded Advertising Data"));
        
    }
    
}