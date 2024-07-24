using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Utils;

namespace ChatManager.Utils;

public static class Helpers
{

    public static string MessageBuilder(string message, params object[] args)
    {

        StringBuilder builder = new(ChatManager.Instance.Config.Prefix.ReplaceColorTags());
        builder.AppendFormat(ChatManager.Instance.Localizer[message], args);
        return builder.ToString();

    }

    private static string ReplaceTags(string message, CsTeam team)
    {
        
        if (message.Contains('{'))
        {
            string modifiedValue = message;

            foreach (FieldInfo field in typeof(ChatColors).GetFields())
            {
                string pattern = $"{{{field.Name}}}";

                if (message.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    modifiedValue = modifiedValue.Replace(pattern, field.GetValue(null)!.ToString(), StringComparison.OrdinalIgnoreCase);
                }
            }
            return modifiedValue.Replace("{TeamColor}", ChatColors.ForTeam(team).ToString());
        }

        return message;
        
    }
    
    public static string SetTeamName(CsTeam team)
    {
        
        var teamTags = ChatManager.Instance.Config.TeamTags;

        return team switch
        {
            CsTeam.None => ReplaceTags(teamTags.NoneSyntax, CsTeam.None),
            CsTeam.Spectator => ReplaceTags(teamTags.SpecSyntax, CsTeam.Spectator),
            CsTeam.Terrorist => ReplaceTags(teamTags.TeamTSyntax, CsTeam.Terrorist),
            CsTeam.CounterTerrorist => ReplaceTags(teamTags.TeamCtSyntax, CsTeam.CounterTerrorist),
            _ => string.Empty,
        };
        
    }

    public static string TeamName(CsTeam team)
    {

        return team switch
        {
            CsTeam.None => "NONE",
            CsTeam.Spectator => "SPEC",
            CsTeam.Terrorist => "T",
            CsTeam.CounterTerrorist => "CT",
            _ => "UNDEFINED"
        };

    }
    
    public static string ReplaceBannedWords(string target)
    {
        
        var bannedWords = ChatManager.Instance.Config.BannedWords;

        if (!bannedWords.Any() || bannedWords.Count == 0)
        {
            return target;
        }

        var pattern = $@"\b({string.Join("|", bannedWords.Select(Regex.Escape))})\b";
        return Regex.Replace(target, pattern, "****", RegexOptions.IgnoreCase);
        
    }
    
    public static string FilterAds(string target)
    {
        
        var patterns = new[]
        {
            @"\b(?:\d{1,3}\.){3}\d{1,3}\b", // IP address pattern
            @"(?:http(s)?://)?[\w.-]+\.[a-zA-Z]{2,}(?:/\S*)?" // URL pattern
        };

        foreach (var pattern in patterns)
        {
            target = Regex.Replace(target, pattern, "****", RegexOptions.IgnoreCase);
        }

        return target;
        
    }

    public static string CreateChatMessage(string deadStatus, string teamName, string tag, string nameColor, string chatColor, CCSPlayerController player, string message)
    {
        
        string playerName = player.PlayerName;
        
        if (ChatManager.Instance.Config.AdBlock.PlayerNames)
            playerName = FilterAds(player.PlayerName);

        return ReplaceTags(
            $"\u200e{deadStatus}{teamName}{tag} {nameColor}{playerName}{ChatColors.Default}: {chatColor}{message}",
            player.Team);
        
    }
    
}