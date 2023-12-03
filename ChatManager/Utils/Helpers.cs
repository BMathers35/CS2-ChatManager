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

    public static string ReplaceBannedWords(string target)
    {

        List<string>? bannedWords = ChatManager._config?.BannedWords;

        if (bannedWords != null && bannedWords.Any())
        {
            
            foreach (var bannedWord in bannedWords)
            {
                string pattern = $@"\b{Regex.Escape(bannedWord)}\b";
                if (Regex.IsMatch(target, pattern, RegexOptions.IgnoreCase))
                {
                    target = Regex.Replace(target, pattern, "****", RegexOptions.IgnoreCase);
                }
            }
            
        }

        return target;

    }
    
    public static string FilterAds(string target)
    {
        
        string ipRegexPattern = @"\b(?:\d{1,3}\.){3}\d{1,3}\b";
        string urlRegexPattern = @"(?:http(s)?://)?[\w.-]+\.[a-zA-Z]{2,}(?:/\S*)?";

        target = Regex.Replace(target, ipRegexPattern, "****", RegexOptions.IgnoreCase);
        return Regex.Replace(target, urlRegexPattern, "****", RegexOptions.IgnoreCase);
        
    }
    
}