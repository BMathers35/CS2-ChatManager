using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;

namespace ChatManager;

[MinimumApiVersion(247)]
public class ChatManager : BasePlugin, IPluginConfig<Config>
{

    public override string ModuleName => "ChatManager";
    public override string ModuleAuthor => "BMathers";
    public override string ModuleVersion => "2.0.0";
    
    public static ChatManager Instance { get; set; } = new ChatManager();
    public Config Config { get; set; } = new Config();
    
    
    public override void Load(bool hotReload)
    {

        Instance = this;

        Events.Events.Load();
        Commands.Commands.Load();

        Task.Run(async () =>
        {

            await Database.CreateDatabase();

        });
        
        Advertisements.LoadAdvertisements();
        
    }

    public override void Unload(bool hotReload)
    {
        
        Events.Events.Unload();
        Commands.Commands.Unload();
        
    }
    
    public void OnConfigParsed(Config config)
    {

        Config = config;

    }

}