using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;

namespace ChatManager;

public class Messages
{

    [JsonPropertyName("AlreadyMuted")]
    public string AlreadyMuted { get; set; } = "{PLAYERNAME} has already been silenced.";

    [JsonPropertyName("SilencedForSeconds")]
    public string SilencedForSeconds { get; set; } = "{PLAYERNAME} was silenced for {DURATION} second.";

    [JsonPropertyName("MultipleTargetsFound")]
    public string MultipleTargetsFound { get; set; } = "Multiple targets found, please provide SteamID64 or the full name of the player.";

    [JsonPropertyName("NoTarget")]
    public string NoTarget { get; set; } = "No target player found";

    [JsonPropertyName("UnmutePlayer")]
    public string UnmutePlayer { get; set; } = "Removed the silencing of the player {PLAYERNAME}";

    [JsonPropertyName("UnmutePlayerInfo")]
    public string UnmutePlayerInfo { get; set; } = "{ADMIN} has lifted your mute.";

    [JsonPropertyName("CouldNotRemove")]
    public string CouldNotRemove { get; set; } = "{PLAYERNAME} player's chat ban could not be removed!";

    [JsonPropertyName("NotBanned")]
    public string NotBanned { get; set; } = "{PLAYERNAME}'s ban has ended or has never been silenced.";

    [JsonPropertyName("CanNotSendMessage")]
    public string CanNotSendMessage { get; set; } = "You cannot send messages because you are silenced.";
    
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

public class ChatSyntax
{
    
    [JsonPropertyName("ALL")]
    public string AllSyntax { get; set; } = "{STATUS_DEAD}{PLAYER_TEAM}{PLAYER_NAME}{White}:{Green}{PLAYER_MESSAGE}";
    
    [JsonPropertyName("CT")]
    public string CtSyntax { get; set; } = "{STATUS_DEAD}{PLAYER_TEAM}{PLAYER_NAME}{White}:{Green}{PLAYER_MESSAGE}";
    
    [JsonPropertyName("T")]
    public string Syntax { get; set; } = "{STATUS_DEAD}{PLAYER_TEAM}{PLAYER_NAME}{White}:{Green}{PLAYER_MESSAGE}";
    
}

public class GeneralSettings
{
    
    [JsonPropertyName("Prefix")]
    public string Prefix { get; set; } = "{Purple}[ChatManager]";
    
    [JsonPropertyName("AdBlockingOnChatAndPlayerNames")]
    public bool AdBlockingOnChatAndPlayerNames { get; set; } = true;
    
    [JsonPropertyName("BlockBannedWordsInChat")]
    public bool BlockBannedWordsInChat { get; set; } = false;
    
    [JsonPropertyName("LoggingChatMessagesWithDiscord")]
    public bool LoggingMessages { get; set; } = false;
    
    [JsonPropertyName("LoggingCommandsWithDiscord")]
    public bool LoggingCommands { get; set; } = false;
    
    [JsonPropertyName("MuteCommand")]
    public string MuteCommand { get; set; } = "mute";
    
    [JsonPropertyName("UnmuteCommand")]
    public string UnmuteCommand { get; set; } = "unmute";
    
}

public class DiscordWebhooks
{
    
    [JsonPropertyName("MessagesWebhook")]
    public string MessagesWebhook { get; set; } = "-";
    
    [JsonPropertyName("CommandsWebhook")]
    public string CommandsWebhook { get; set; } = "-";
    
}

public class Config : IBasePluginConfig
{
    
    [JsonPropertyName("General")]
    public GeneralSettings GeneralSettings { get; set; } = new GeneralSettings();
    
    [JsonPropertyName("Tags")]
    public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
    
    [JsonPropertyName("TeamTags")]
    public TeamTags TeamTags { get; set; } = new TeamTags();
    
    [JsonPropertyName("ChatSyntax")]
    public ChatSyntax ChatSyntax { get; set; } = new ChatSyntax();
    
    [JsonPropertyName("DiscordWebhooks")]
    public DiscordWebhooks DiscordWebhooks { get; set; } = new DiscordWebhooks();
    
    [JsonPropertyName("Messages")]
    public Messages Messages { get; set; } = new Messages();
    
    [JsonPropertyName("BannedWords")]
    public List<string> BannedWords { get; set; } = new List<string>();

    [JsonPropertyName("ConfigVersion")] 
    public int Version { get; set; } = 1;

}