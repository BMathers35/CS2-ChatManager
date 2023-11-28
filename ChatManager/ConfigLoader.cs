using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChatManager;

public static class ConfigLoader
{

    // public static JObject LoadMutedPlayers(string FilePath)
    // {
    //
    //     if (!File.Exists(FilePath))
    //     {
    //
    //         var mutedPlayersTemplate = new JObject { };
    //         File.WriteAllText(FilePath, mutedPlayersTemplate.ToString());
    //         var mutedPlayersData = File.ReadAllText(FilePath);
    //         return JObject.Parse(mutedPlayersData);
    //         
    //     }
    //     else
    //     {
    //         
    //         var mutedPlayersData = File.ReadAllText(FilePath);
    //         return JObject.Parse(mutedPlayersData);
    //         
    //     }
    //     
    // }

    public static JObject LoadConfig(string FilePath)
    {
        
        if (!File.Exists(FilePath))
        {

            var configTemplate = new JObject
            {

                ["Tags"] = new JObject
                {

                    ["@css/root"] = new JObject
                    {
                        ["Prefix"] = "{Red}[FOUNDER]",
                        ["NickColor"] = "{Red}",
                        ["MessageColor"] = "{Red}",
                        ["ScoreboardTag"] = "[FOUNDER]"
                    },

                    ["@css/ban"] = new JObject
                    {
                        ["Prefix"] = "{Purple}[ADMIN]",
                        ["NickColor"] = "{Purple}",
                        ["MessageColor"] = "{Purple}",
                        ["ScoreboardTag"] = "[ADMIN]"
                    },

                    ["@css/vip"] = new JObject
                    {
                        ["Prefix"] = "{Gold}[VIP]",
                        ["NickColor"] = "{Gold}",
                        ["MessageColor"] = "{Gold}",
                        ["ScoreboardTag"] = "[VIP]"
                    },

                    ["everyone"] = new JObject
                    {
                        ["Prefix"] = "",
                        ["NickColor"] = "{Green}",
                        ["MessageColor"] = "",
                        ["ScoreboardTag"] = ""
                    }

                },
                
                ["Logs"] = new JObject
                {
                    
                    ["ChatMessages"] = new JObject
                    {
                        ["Active"] = "false",
                        ["Discord_Webhook"] = ""
                    },
                    
                    ["Commands"] = new JObject
                    {
                        ["Active"] = "false",
                        ["Discord_Webhook"] = ""
                    },
                    
                },
                
                ["MuteCommands"] = new JObject
                {
                    
                    ["MuteCommand"] = "mute",
                    ["UnmuteCommand"] = "unmute"
                    
                },
                
                ["Localization"] = new JObject
                {
                    
                    ["Teams"] = new JObject
                    {
                        
                        ["Spec"] = new JObject
                        {
                            ["Tag"] = "(SPEC)",
                            ["Color"] = "{Default}"
                        },
                        
                        ["None"] = new JObject
                        {
                            ["Tag"] = "(NONE)",
                            ["Color"] = "{Default}"
                        },
                        
                        ["T"] = new JObject
                        {
                            ["Tag"] = "(T)",
                            ["Color"] = "{Gold}"
                        },
                        
                        ["CT"] = new JObject
                        {
                            ["Tag"] = "(CT)",
                            ["Color"] = "{Blue}"
                        },
                        
                        ["Dead"] = new JObject
                        {
                            ["Tag"] = "*DEAD*",
                            ["Color"] = "{Default}"
                        }
                        
                    },
                    
                    ["Mute"] = new JObject
                    {
                        ["AlreadyMuted"] = "{PLAYERNAME} has already been silenced.",
                        ["SilencedForSeconds"] = "{PLAYERNAME} was silenced for {DURATION} second.",
                        ["MultipleTargetsFound"] = "Multiple targets found, please provide SteamID64 or the full name of the player.",
                        ["NoTarget"] = "No target player found",
                        ["UnmutePlayer"] = "Removed the silencing of the player {PLAYERNAME}",
                        ["UnmutePlayerInfo"] = "{ADMIN} has lifted your mute.",
                        ["CouldNotRemove"] = "{PLAYERNAME} player's chat ban could not be removed!",
                        ["NotBanned"] = "{PLAYERNAME}'s ban has ended or has never been silenced.",
                        ["CanNotSendMessage"] = "You cannot send messages because you are silenced."
                    },
                    
                    ["Discord"] = new JObject
                    {
                        ["PlayerImage"] = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Steam_icon_logo.svg/2048px-Steam_icon_logo.svg.png",
                        ["PlayerName"] = "Player Name",
                        ["SteamID"] = "SteamID64",
                        ["Profile"] = "Profile",
                        ["ProfileLink"] = "Steam Profile"
                    }
                    
                }

            };

            File.WriteAllText(FilePath, configTemplate.ToString());
            var ConfigData = File.ReadAllText(FilePath);
            return JObject.Parse(ConfigData);

        }
        else
        {
            
            var ConfigData = File.ReadAllText(FilePath);
            return JObject.Parse(ConfigData);

        }
        
    }
    
}