using System.IO;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChatManager;

public abstract class MuteManager
{
    
    public static List<MutedPlayer>? MutedPlayers { get; private set; }
    public static string? _moduleDirectory;
    public static List<MutedPlayer>? MuteManagerLoader(string FilePath)
    {

        _moduleDirectory = FilePath;
        
        if (!File.Exists(FilePath + "/muted_players.json"))
        {

            var mutedPlayersTemplate = new List<MutedPlayer>();
            File.WriteAllText(_moduleDirectory + "/muted_players.json", JsonConvert.SerializeObject(mutedPlayersTemplate));

        }
        
        var mutedPlayersData = File.ReadAllText(_moduleDirectory  + "/muted_players.json");
        MutedPlayers = JsonConvert.DeserializeObject<List<MutedPlayer>>(mutedPlayersData);
        return JsonConvert.DeserializeObject<List<MutedPlayer>>(mutedPlayersData);
        
    }

    public static void CheckMutedPlayers()
    {
        
        if (_moduleDirectory == null) return;
        if (MutedPlayers != null)
        {
            Console.WriteLine("[ChatManager] Checking players whose silence period has expired.");
            var expiredMutes = MutedPlayers.Where(p => p.Duration > 0 && DateTime.Now > p.Date.AddSeconds(p.Duration)).ToList();
            if (expiredMutes.Any())
            {
                Console.WriteLine("[ChatManager] There are players whose mute has expired! Their muting is being removed...");
                foreach (var expiredMute in expiredMutes)
                {
                    Console.WriteLine($"[ChatManager] {expiredMute.Id} muting penalty removed...");
                    MutedPlayers.Remove(expiredMute);
                }
                File.WriteAllText(_moduleDirectory + "/muted_players.json", JsonConvert.SerializeObject(MutedPlayers));
            }
        }
        
    }

    public static List<MutedPlayer>? MutePlayer(CCSPlayerController? player, int duration)
    {
        
        if (_moduleDirectory == null) return null;
        if (player == null || !player.IsValid || player.IsBot) return null;

        if (MutedPlayers == null)
        {
            MutedPlayers = new List<MutedPlayer>();
        }

        string steamId = new SteamID(player.SteamID).SteamId64.ToString();

        MutedPlayers.Add(new MutedPlayer
        {
            Id = steamId,
            Date = DateTime.Now,
            Duration = duration
        });

        Console.WriteLine($"[ChatManager] Player {player.PlayerName} ({steamId}) is silenced for {duration} seconds.");
        File.WriteAllText(_moduleDirectory + "/muted_players.json", JsonConvert.SerializeObject(MutedPlayers));
        return MutedPlayers;
        
    }
    
    public static bool IsPlayerMuted(CCSPlayerController player)
    {
        if (MutedPlayers == null)
        {
            Console.WriteLine("[ChatManager] The list of silenced players is empty.");
            return false;
        }
        else
        {
            
            string steamId = new SteamID(player.SteamID).SteamId64.ToString();
            
            if (steamId == null)
            {
                Console.WriteLine("[ChatManager] Failed to receive a valid SteamID.");
                return false;
            }
            else
            {
                
                if (!MutedPlayers.Exists(p => p.Id == steamId))
                {
                    Console.WriteLine($"[ChatManager] Player {player.PlayerName} ({steamId}) has either completed or has never been silenced.");
                    return false;
                }

                return true;
                
            }
            
        }
        
    }

    public static bool UnmutePlayer(CCSPlayerController? player)
    {
        
        if (player == null || !player.IsValid || player.IsBot) return false;

        if (MutedPlayers != null)
        {
            string steamId = new SteamID(player.SteamID).SteamId64.ToString();

            var removedPlayer = MutedPlayers.FirstOrDefault(p => p.Id == steamId);

            if (removedPlayer != null)
            {
                MutedPlayers.Remove(removedPlayer);
                Console.WriteLine($"[ChatManager] Player {steamId} has been unmuted using the command.");
                File.WriteAllText(_moduleDirectory + "/muted_players.json", JsonConvert.SerializeObject(MutedPlayers));
                return true;
            }

            return false;
        }

        return false;

    }
    
}

public class MutedPlayer
{
    
    public string? Id { get; init; }
    public DateTime Date { get; init; }
    public double Duration { get; init; }
    
}