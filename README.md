# CS2-ChatManager
This CSSharp plugin allows managing CS2 chat messages.

### Description
This plugin is basically an improvement of the [CS2-Tags](https://github.com/daffyyyy/CS2-Tags) plugin with some minor issues fixed. Since this is my first C# and CSSharp project, I apologize for the bad and unorganized code. If you want to edit and improve the code, feel free to send a pull request.

### Requirments
[CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/) **Tested on v75**

### Features
- **Custom Tags:** To give more visibility to players with special privileges on your server, you can add special tags such as Admin and VIP. This way, other players can immediately recognize them and communicate more effectively to keep the server organized.
- **Name Color and Message Color:** By providing custom color options for each player and their authorization flags, their names and chat messages are displayed in color. This feature makes it easier to distinguish players from the chat and makes communication more effective.
- **Chat Messages Logging:** Instantly transmits chat messages from your server to Discord, creating a log of in-game conversations.
- **Commands Used Logging:** Transmits commands used by players on your server to your discord server via Discord Webhook. This gives server administrators the opportunity to analyze in-game activity and take appropriate action if necessary.
- **Localization Support:** It offers a range of localization features to customize the text inside the plugin and provide multilingual support.
- **Mute Commands:** The plugin offers a special command to mute a specific player for a specified period of time. For example, using the command "!mute [player_name or steamid64] [duration]", the specified player can be removed from the chat for the specified duration. (Command names can be customized)

### Updates&Fixes
- Player silence commands have been added. Command names can be configured in config.json file (!mute !unmute)
- Sending an empty message to the chat is blocked.
- You can log messages and commands sent to the chat via Discord Webhook.

## Usage

### Commands

- **!mute [player_name or steamid64] [duration]** - Ban players from chatting for a certain period of time. (Requires @css/chat)
- **!unmute [player_name or steamid64]** - Unban a player with a chat ban. (Requires @css/chat)

When you define a chat ban for a player, the plugin saves that player in the muted_players.json file. Saved players are checked every 30 seconds by the plugin to see if the ban has expired. If a player's ban has expired, they are automatically deleted from the muted_players.json file. Players with a chat ban cannot send messages to the chat. They can only use commands starting with "!" and "/".

### Default config.json file;
```json
{
  "Tags": {
    "@css/root": {
      "Prefix": "{Red}[FOUNDER]",
      "NickColor": "{Red}",
      "MessageColor": "{Red}",
      "ScoreboardTag": "[FOUNDER]"
    },
    "@css/ban": {
      "Prefix": "{Purple}[ADMIN]",
      "NickColor": "{Purple}",
      "MessageColor": "{Purple}",
      "ScoreboardTag": "[ADMIN]"
    },
    "@css/vip": {
      "Prefix": "{Gold}[VIP]",
      "NickColor": "{Gold}",
      "MessageColor": "{Gold}",
      "ScoreboardTag": "[VIP]"
    },
    "everyone": {
      "Prefix": "",
      "NickColor": "{Green}",
      "MessageColor": "",
      "ScoreboardTag": ""
    }
  },
  "Logs": {
    "ChatMessages": {
      "Active": "false",
      "Discord_Webhook": ""
    },
    "Commands": {
      "Active": "false",
      "Discord_Webhook": ""
    }
  },
  "MuteCommands": {
    "MuteCommand": "mute",
    "UnmuteCommand": "unmute"
  },
  "Localization": {
    "Teams": {
      "Spec": {
        "Tag": "(SPEC)",
        "Color": "{Default}"
      },
      "None": {
        "Tag": "(NONE)",
        "Color": "{Default}"
      },
      "T": {
        "Tag": "(T)",
        "Color": "{Gold}"
      },
      "CT": {
        "Tag": "(CT)",
        "Color": "{Blue}"
      },
      "Dead": {
        "Tag": "*DEAD*",
        "Color": "{Default}"
      }
    },
    "Mute": {
      "AlreadyMuted": "{PLAYERNAME} has already been silenced.",
      "SilencedForSeconds": "{PLAYERNAME} was silenced for {DURATION} second.",
      "MultipleTargetsFound": "Multiple targets found, please provide SteamID64 or the full name of the player.",
      "NoTarget": "No target player found",
      "UnmutePlayer": "Removed the silencing of the player {PLAYERNAME}",
      "UnmutePlayerInfo": "{ADMIN} has lifted your mute.",
      "CouldNotRemove": "{PLAYERNAME} player's chat ban could not be removed!",
      "NotBanned": "{PLAYERNAME}'s ban has ended or has never been silenced.",
      "CanNotSendMessage": "You cannot send messages because you are silenced."
    },
    "Discord": {
      "PlayerImage": "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Steam_icon_logo.svg/2048px-Steam_icon_logo.svg.png",
      "PlayerName": "Player Name",
      "SteamID": "SteamID64",
      "Profile": "Profile",
      "ProfileLink": "Steam Profile"
    }
  }
}
```
