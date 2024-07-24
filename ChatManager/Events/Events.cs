using ChatManager.Utils;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;
using Helpers = ChatManager.Utils.Helpers;

namespace ChatManager.Events;

public abstract class Events
{

    public static void Load()
    {

        ChatManager.Instance.AddCommandListener("say", OnPlayerChat);
        ChatManager.Instance.AddCommandListener("say_team", OnPlayerChat);

    }

    public static void Unload()
    {

        ChatManager.Instance.RemoveCommandListener("say", OnPlayerChat, HookMode.Pre);
        ChatManager.Instance.RemoveCommandListener("say_team", OnPlayerChat, HookMode.Pre);

    }

    private static HookResult OnPlayerChat(CCSPlayerController? player, CommandInfo info)
    {

        string message = info.GetArg(1);
        if (player == null || !player.IsValid || player.IsBot || message.Length == 0) return HookResult.Continue;

        if (CoreConfig.SilentChatTrigger.Any(p => message.StartsWith(p)))
        {
            
            return HookResult.Continue;

        }

        if (CoreConfig.PublicChatTrigger.Any(p => message.StartsWith(p)))
        {
            
            player.ExecuteClientCommandFromServer($"css_{message.Substring(1)}");
            
        }
        
        string deadStatus = player.PawnIsAlive ? string.Empty : ChatManager.Instance.Config.TeamTags.DeadSyntax;
        string playerName = player.PlayerName;

        if (ChatManager.Instance.Config.AdBlock.ChatMessages)
            message = Helpers.FilterAds(Helpers.ReplaceBannedWords(message));

        if (ChatManager.Instance.Config.Logging.DiscordLogs)
        {
            
            Server.NextFrame(() =>
            {
                _ = Discord.SendAsync(player, message);
            });
            
        }

        if (ChatManager.Instance.Config.Logging.ServerLogs)
        {
            
            string date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            ChatManager.Instance.Logger.LogInformation($"[{date}][{Helpers.TeamName(player.Team)}][{player.SteamID.ToString()}][{playerName}]: {message}");
            
        }

        if (ChatManager.Instance.Config.Tags.Any())
        {

            foreach (var tag in ChatManager.Instance.Config.Tags)
            {
                
                Tag playerData = tag.Value;
                string replace;

                if (tag.Key.Contains(player.SteamID.ToString()))
                {
                    
                    replace = Helpers.CreateChatMessage(deadStatus,
                        info.GetArg(0) == "say_team" ? Helpers.SetTeamName(player.Team) : string.Empty,
                        playerData.ChatTag, playerData.NameColor, playerData.ChatColor, player, message);
                    
                    if (info.GetArg(0) == "say_team")
                    {

                        foreach (CCSPlayerController target in Utilities.GetPlayers().Where(target => target.Team == player.Team && !target.IsBot))
                        {
                            
                            target.PrintToChat(replace);
                            return HookResult.Handled;

                        }
                        
                    }
                    else
                    {
                        
                        Server.PrintToChatAll(replace);
                        return HookResult.Handled;
                        
                    }
                    
                }

                if (tag.Key.StartsWith("#"))
                {

                    if (AdminManager.PlayerInGroup(player, tag.Key))
                    {
                        
                        replace = Helpers.CreateChatMessage(deadStatus,
                            info.GetArg(0) == "say_team" ? Helpers.SetTeamName(player.Team) : string.Empty,
                            playerData.ChatTag, playerData.NameColor, playerData.ChatColor, player, message);
                    
                        if (info.GetArg(0) == "say_team")
                        {

                            foreach (CCSPlayerController target in Utilities.GetPlayers().Where(target => target.Team == player.Team && !target.IsBot))
                            {
                            
                                target.PrintToChat(replace);
                                return HookResult.Handled;

                            }
                        
                        }
                        else
                        {
                        
                            Server.PrintToChatAll(replace);
                            return HookResult.Handled;
                        
                        }
                        
                    }
                    
                }

                if (tag.Key.StartsWith("@"))
                {

                    if (AdminManager.PlayerHasPermissions(player, tag.Key))
                    {
                        
                        replace = Helpers.CreateChatMessage(deadStatus,
                            info.GetArg(0) == "say_team" ? Helpers.SetTeamName(player.Team) : string.Empty,
                            playerData.ChatTag, playerData.NameColor, playerData.ChatColor, player, message);
                    
                        if (info.GetArg(0) == "say_team")
                        {

                            foreach (CCSPlayerController target in Utilities.GetPlayers().Where(target => target.Team == player.Team && !target.IsBot))
                            {
                            
                                target.PrintToChat(replace);
                                return HookResult.Handled;

                            }
                        
                        }
                        else
                        {
                        
                            Server.PrintToChatAll(replace);
                            return HookResult.Handled;
                        
                        }
                        
                    }
                    
                }

                if (tag.Key.Contains("Default"))
                {
                    
                    replace = Helpers.CreateChatMessage(deadStatus,
                        info.GetArg(0) == "say_team" ? Helpers.SetTeamName(player.Team) : string.Empty,
                        playerData.ChatTag, playerData.NameColor, playerData.ChatColor, player, message);
                    
                    if (info.GetArg(0) == "say_team")
                    {

                        foreach (CCSPlayerController target in Utilities.GetPlayers().Where(target => target.Team == player.Team && !target.IsBot))
                        {
                            
                            target.PrintToChat(replace);
                            return HookResult.Handled;

                        }
                        
                    }
                    else
                    {
                        
                        Server.PrintToChatAll(replace);
                        return HookResult.Handled;
                        
                    }
                    
                    
                }
                
            }
            
        }
        
        return HookResult.Continue;

    }
    
}