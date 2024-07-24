using System.Text;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Timers;
using Microsoft.Extensions.Logging;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;

namespace ChatManager;

public static class Advertisements
{
    
    private static readonly List<Timer> AdvertisementTimer = new();

    public static void LoadAdvertisements()
    {
        
        try
        {
            
            if (AdvertisementTimer.Count != 0)
            {
                foreach (var timer in AdvertisementTimer)
                    timer.Kill();
                AdvertisementTimer.Clear();
            }

            var Announcements = Database.Query(@"SELECT * FROM cm_advertisements").ToList();

            if (Announcements.Any())
            {

                for (int i = 0; i < Announcements.Count; i++)
                {

                    var AnnouncementData = Announcements[i];
                    int announcementDuration = AnnouncementData.duration;
                
                    Timer Timer = ChatManager.Instance.AddTimer(announcementDuration, () =>
                    {
                    
                        string content = AnnouncementData.message;
                        StringBuilder message = new(ChatManager.Instance.Config.Prefix.ReplaceColorTags());
                        message.Append(content.ReplaceColorTags());
                        Server.PrintToChatAll(message.ToString());
                    
                    }, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
                
                    AdvertisementTimer.Add(Timer);

                }

            }
            
        }
        catch (Exception e)
        {
            
            ChatManager.Instance.Logger.LogError($"Failed to get the Advertisement List: {e}");
            throw;
            
        }
        
    }
    
}