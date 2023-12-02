using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using static ChatManager.MuteManager;
using Helpers = ChatManager.Utils.Helpers;

namespace ChatManager;

[MinimumApiVersion(75)]
public sealed class ChatManager : BasePlugin, IPluginConfig<Config>
{

    public override string ModuleName => "ChatManager";
    public override string ModuleAuthor => "BMathers";
    public override string ModuleVersion => "1.1.0";
    public static List<MutedPlayer>? MutedPlayers { get; private set; }
    private int ModuleConfigVersion => 1;
    public required Config Config { get; set; }
    public static string? _moduleDirectory;
    public static Config? _config;
    
    public override async void Load(bool hotReload)
    {
        
        base.Load(hotReload);
        
        Console.WriteLine(" ");
        Console.WriteLine("   _____ _           _   __  __                                   ");
        Console.WriteLine("  / ____| |         | | |  \\/  |                                  ");
        Console.WriteLine(" | |    | |__   __ _| |_| \\  / | __ _ _ __   __ _  __ _  ___ _ __ ");
        Console.WriteLine(" | |    | '_ \\ / _` | __| |\\/| |/ _` | '_ \\ / _` |/ _` |/ _ \\ '__|");
        Console.WriteLine(" | |____| | | | (_| | |_| |  | | (_| | | | | (_| | (_| |  __/ |   ");
        Console.WriteLine("  \\_____|_| |_|\\__,_|\\__|_|  |_|\\__,_|_| |_|\\__,_|\\__, |\\___|_|   ");
        Console.WriteLine("                                                   __/ |          ");
        Console.WriteLine("                                                  |___/           ");
        Console.WriteLine("			>> Version: " + ModuleVersion);
        Console.WriteLine("			>> Author: " + ModuleAuthor);
        Console.WriteLine(" ");
        
        MutedPlayers = MuteManagerLoader(ModuleDirectory);
        _moduleDirectory = ModuleDirectory;
        
        AddCommandListener("say", Events.OnPlayerChat.Run);
        AddCommandListener("say_team", Events.OnPlayerTeamChat.Run);
        
        AddCommand($"css_{Config.GeneralSettings.MuteCommand}", "Mute a player.", Commands.Mute.Command);
        AddCommand($"css_{Config.GeneralSettings.UnmuteCommand}", "Unmute a player.", Commands.Unmute.Command);
        AddCommand($"css_{Config.GeneralSettings.ReloadCommand}", "Reload configuration.", Commands.Reload.Command);
        
        while (true)
        {
            CheckMutedPlayers();
            await Task.Delay(30000);
        }

    }
    
    public void OnConfigParsed(Config config)
    {

        if (config.Version < ModuleConfigVersion)
        {
            Console.WriteLine($"[ChatManager] You are using an old configuration file. Version you are using:{config.Version} - New Version: {ModuleConfigVersion}");
        }

        Config = config;
        _config = config;

    }

}