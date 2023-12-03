using System.Text.RegularExpressions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Entities;
using Newtonsoft.Json.Linq;

namespace ChatManager.Utils;

public class Helpers
{

    public static string setTeamName(int teamNum)
    {

        string teamName = "";

        switch (teamNum)
        {
            case 0:
                teamName = $"{ChatManager._config?.TeamTags.NoneSyntax}";
                break;
            case 1:
                teamName = $"{ChatManager._config?.TeamTags.SpecSyntax}";
                break;
            case 2:
                teamName = $"{ChatManager._config?.TeamTags.TeamTSyntax}";
                break;
            case 3:
                teamName = $"{ChatManager._config?.TeamTags.TeamCtSyntax}";
                break;
        }

        return teamName;

    }
    
    public static bool IsNumeric(string input)
    {
        return input.All(char.IsDigit);
    }
    
    public static string AdvertisementFiltering(string playerName)
    {
        
        string ipRegexPattern = @"\b(?:\d{1,3}\.){3}\d{1,3}\b";
        string urlRegexPattern = @"(?:http(s)?://)?[\w.-]+\.[a-zA-Z]{2,}(?:/\S*)?";

        playerName = Regex.Replace(playerName, ipRegexPattern, "****", RegexOptions.IgnoreCase);
        return Regex.Replace(playerName, urlRegexPattern, "****", RegexOptions.IgnoreCase);
        
    }
    
}