﻿using ChatManager.Utils;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;

namespace ChatManager.Events;

public class OnPlayerTeamChat
{
    
    public static HookResult Run(CCSPlayerController? player, CommandInfo info)
    {
        
        string message = info.GetArg(1);
        
        if (player == null || !player.IsValid || player.IsBot) return HookResult.Handled;
        if (string.IsNullOrEmpty(message)) return HookResult.Handled;

        string steamId = new SteamID(player.SteamID).SteamId64.ToString();
        
        if (message.StartsWith("!") || message.StartsWith("/"))
        {
            if ((bool)ChatManager._config?.GeneralSettings.LoggingCommands)
            {
                Task.Run(() =>
                {
                    Server.NextFrame(() => Discord.Send(player, message, "Command"));
                });  
            }
            return HookResult.Continue;
        }

        if (MuteManager.IsPlayerMuted(player))
        {
            player.PrintToChat(Colors.Tags($"\u200e{ChatColors.Purple}{ChatManager._config?.GeneralSettings.Prefix} {ChatColors.Darkred}{ChatManager._config?.Messages.CanNotSendMessage}"));
            return HookResult.Handled;
        }
        
        string deadStatus = !player.PawnIsAlive ? ChatManager._config.TeamTags.DeadSyntax : "";
        string playerTeam = Utils.Helpers.setTeamName(player.TeamNum);
        string playerName = player.PlayerName;
        string teamMessage = $"\u200e{ChatManager._config.ChatSyntax.AllSyntax}";

        if (player.TeamNum == 2)
        {
            teamMessage = $"\u200e{ChatManager._config.ChatSyntax.Syntax}";
        }
        else if (player.TeamNum == 3)
        {
            teamMessage = $"\u200e{ChatManager._config.ChatSyntax.CtSyntax}";
        }
            
        if (ChatManager._config.GeneralSettings.AdBlockingOnChatAndPlayerNames)
        {
            playerName = Utils.Helpers.FilterAds(playerName);
        }
            
        if (ChatManager._config.GeneralSettings.BlockBannedWordsInChat)
        {
            message = Utils.Helpers.ReplaceBannedWords(message);
        }
        
        if (ChatManager._config.GeneralSettings.LoggingMessages)
        {
                
            Task.Run(() =>
            {
                Server.NextFrame(() => Discord.Send(player, message, "Message"));
            });  
            
        }

        if (ChatManager._config != null && ChatManager._config.Tags.Any())
        {
            
            foreach (var tag in ChatManager._config.Tags)
            {
                
                if (tag.Key.Contains(steamId))
                {

                    string prefix = tag.Value;
                    
                    var replace = teamMessage
                        .Replace("{STATUS_DEAD}", deadStatus)
                        .Replace("{PLAYER_NAME}", $"{prefix}{playerName}")
                        .Replace("{PLAYER_MESSAGE}", message)
                        .Replace("{PLAYER_TEAM}", playerTeam);
                    
                    Server.PrintToChatAll(Colors.Tags(replace));

                    return HookResult.Handled;

                }
                
                if (tag.Key.StartsWith("#"))
                {

                    string group = tag.Key;
                    bool hasPermission = AdminManager.PlayerInGroup(player, group);

                    if (hasPermission)
                    {
                        
                        string prefix = tag.Value;
                    
                        var replace = teamMessage
                            .Replace("{STATUS_DEAD}", deadStatus)
                            .Replace("{PLAYER_NAME}", $"{prefix}{playerName}")
                            .Replace("{PLAYER_MESSAGE}", message)
                            .Replace("{PLAYER_TEAM}", playerTeam);
                    
                        Server.PrintToChatAll(Colors.Tags(replace));

                        return HookResult.Handled;
                        
                    }

                }

                if (tag.Key.StartsWith("@"))
                {

                    string permission = tag.Key;
                    bool hasPermission = AdminManager.PlayerHasPermissions(player, permission);

                    if (hasPermission)
                    {
                        
                        string prefix = tag.Value;
                    
                        var replace = teamMessage
                            .Replace("{STATUS_DEAD}", deadStatus)
                            .Replace("{PLAYER_NAME}", $"{prefix}{playerName}")
                            .Replace("{PLAYER_MESSAGE}", message)
                            .Replace("{PLAYER_TEAM}", playerTeam);
                    
                        Server.PrintToChatAll(Colors.Tags(replace));

                        return HookResult.Handled;
                        
                    }

                }
                
                if (tag.Key.Contains("everyone"))
                {

                    string prefix = tag.Value;
                    
                    var replace = teamMessage
                        .Replace("{STATUS_DEAD}", deadStatus)
                        .Replace("{PLAYER_NAME}", $"{prefix}{playerName}")
                        .Replace("{PLAYER_MESSAGE}", message)
                        .Replace("{PLAYER_TEAM}", playerTeam);
                    
                    Server.PrintToChatAll(Colors.Tags(replace));

                    return HookResult.Handled;

                }
                
            }

        }
        else
        {
                    
            var replace = $"\u200e{ChatManager._config?.ChatSyntax.AllSyntax}"
                .Replace("{STATUS_DEAD}", deadStatus)
                .Replace("{PLAYER_NAME}", $"{playerName}")
                .Replace("{PLAYER_MESSAGE}", message)
                .Replace("{PLAYER_TEAM}", playerTeam);
                    
            Server.PrintToChatAll(Colors.Tags(replace));
            return HookResult.Handled;
            
        }
        
        return HookResult.Continue;
        
    }
    
}