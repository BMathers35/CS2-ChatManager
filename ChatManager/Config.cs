using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;

namespace ChatManager;

public class Discord
{
    
    [JsonPropertyName("PlayerImage")]
    public string PlayerImage { get; set; } = "-";
    
    [JsonPropertyName("PlayerName")]
    public string PlayerName { get; set; } = "Player Name";
    
    [JsonPropertyName("SteamId")]
    public string SteamId { get; set; } = "SteamID64";
    
    [JsonPropertyName("Profile")]
    public string Profile { get; set; } = "Profile";
    
    [JsonPropertyName("ProfileLink")]
    public string ProfileLink { get; set; } = "Steam Profile";
    
}

public class Messages
{

    [JsonPropertyName("Discord")]
    public Discord Discord { get; set; } = new Discord();

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

public class Tags : List<string>
{
    
    [JsonPropertyName("@css/root")]
    public string ExampleTagOne { get; set; } = "{Red}[Founder]";

    [JsonPropertyName("#css/admin")]
    public string ExampleTagTwo { get; set; } = "{Gold}[Admin]";

    [JsonPropertyName("76561199112108514")]
    public string ExampleTagThree { get; set; } = "{Purple}[BMathers]";

    [JsonPropertyName("everyone")]
    public string ExampleTagFour { get; set; } = "{Green}[Player]";

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
    
    [JsonPropertyName("LoggingChatMessagesWithDiscord")]
    public bool LoggingMessages { get; set; } = false;
    
    [JsonPropertyName("LoggingCommandsWithDiscord")]
    public bool LoggingCommands { get; set; } = false;
    
    [JsonPropertyName("MuteCommand")]
    public string MuteCommand { get; set; } = "mute";
    
    [JsonPropertyName("UnmuteCommand")]
    public string UnmuteCommand { get; set; } = "unmute";
    
    [JsonPropertyName("ReloadCommand")]
    public string ReloadCommand { get; set; } = "cm_reload";
    
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
    public List<string> Tags { get; set; } = new Tags();
    
    [JsonPropertyName("TeamTags")]
    public TeamTags TeamTags { get; set; } = new TeamTags();
    
    [JsonPropertyName("ChatSyntax")]
    public ChatSyntax ChatSyntax { get; set; } = new ChatSyntax();
    
    [JsonPropertyName("DiscordWebhooks")]
    public DiscordWebhooks DiscordWebhooks { get; set; } = new DiscordWebhooks();
    
    [JsonPropertyName("Messages")]
    public Messages Messages { get; set; } = new Messages();

    [JsonPropertyName("ConfigVersion")] 
    public int Version { get; set; } = 1;

}