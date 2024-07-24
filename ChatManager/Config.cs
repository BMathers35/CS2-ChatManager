using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace ChatManager;

public class AdBlock
{

    [JsonPropertyName("PlayerNames")] 
    public bool PlayerNames { get; set; } = true;

    [JsonPropertyName("ChatMessages")] 
    public bool ChatMessages { get; set; } = true;

}

public class Logging
{
    
    [JsonPropertyName("ServerLogs")]
    public bool ServerLogs { get; set; } = false;
    
    [JsonPropertyName("DiscordLogs")]
    public bool DiscordLogs { get; set; } = false;
    
    [JsonPropertyName("DiscordWebhook")]
    public string DiscordWebhook { get; set; } = string.Empty;
    
}

public class TeamTags
{
    
    [JsonPropertyName("DEAD")]
    public string DeadSyntax { get; set; } = "{Red}[DEAD]";
    
    [JsonPropertyName("TEAM_NONE")]
    public string NoneSyntax { get; set; } = "{White}[NONE]";
    
    [JsonPropertyName("TEAM_SPEC")]
    public string SpecSyntax { get; set; } = "{White}[SPEC]";
    
    [JsonPropertyName("TEAM_T")]
    public string TeamTSyntax { get; set; } = "{Gold}[T]";
    
    [JsonPropertyName("TEAM_CT")]
    public string TeamCtSyntax { get; set; } = "{Blue}[CT]";
    
}

public class DatabaseSettings
{
    
    [JsonPropertyName("host")]
    public string Host { get; set; } = string.Empty;
    
    [JsonPropertyName("port")]
    public int Port { get; set; } = 3306;
    
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
    
    [JsonPropertyName("database")]
    public string Database { get; set; } = string.Empty;
    
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
    
}

public class ChatSyntax
{
    
    [JsonPropertyName("ALL")]
    public string AllSyntax { get; set; } = "{STATUS_DEAD}{PLAYER_TEAM}{PLAYER_NAME}{White}:{Green}{PLAYER_MESSAGE}";
    
    [JsonPropertyName("CT")]
    public string CtSyntax { get; set; } = "{STATUS_DEAD}{PLAYER_TEAM}{PLAYER_NAME}{White}:{Green}{PLAYER_MESSAGE}";
    
    [JsonPropertyName("T")]
    public string Syntax { get; set; } = "{STATUS_DEAD}{PLAYER_TEAM}{PLAYER_NAME}{White}:{Green}{PLAYER_MESSAGE}";
    
}

public class CmCommands
{
    
    [JsonPropertyName("AdsReload")]
    public string AdsReload { get; set; } = "adsreload";
    
}

public class Config : IBasePluginConfig
{
    
    [JsonPropertyName("Prefix")]
    public string Prefix { get; set; } = "{Blue}[ChatManager]";
    
    [JsonPropertyName("Commands")]
    public CmCommands Commands { get; set; } = new CmCommands();

    [JsonPropertyName("Database")] 
    public DatabaseSettings Database { get; set; } = new DatabaseSettings();

    [JsonPropertyName("AdBlock")] 
    public AdBlock AdBlock { get; set; } = new AdBlock();

    [JsonPropertyName("Logging")] 
    public Logging Logging { get; set; } = new Logging();
    
    [JsonPropertyName("TeamTags")]
    public TeamTags TeamTags { get; set; } = new TeamTags();
    
    [JsonPropertyName("BannedWords")]
    public List<string> BannedWords { get; set; } = new List<string>();

    [JsonPropertyName("Tags")]
    public Dictionary<string, Tag> Tags { get; set; } = new Dictionary<string, Tag>
    {
        ["Everyone"] = new()
        {
            ChatColor = "{White}",
            ChatTag = "{White}[Player]",
            NameColor = "{TeamColor}"
        }
    };

    [JsonPropertyName("ConfigVersion")] 
    public int Version { get; set; } = 3;

}

public class Tag
{
    
    public string ChatTag { get; set; } = string.Empty;
    public string ChatColor { get; set; } = string.Empty;
    public string NameColor { get; set; } = string.Empty;
    
}