# CS2-ChatManager
This CSSharp plugin allows managing CS2 chat messages.

> [!NOTE]
> I don't have time to update the plugin due to my own busy schedule, so feel free to send a pr if you want to contribute. I will update the plugin as soon as I am available.

## Description
It was originally released as an enhanced version of the [CS2-Tags](https://github.com/daffyyyy/CS2-Tags) plugin, but now it has been completely revamped and improvements are being made to fulfill its sole purpose of managing server chat.

With this plugin you can edit the syntax in your server chat, define tags for players, block ads and more.

## Requirments
[CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/) **v115 >=**

## Features
- **Custom Chat Tags**
- **Editing chat syntax**
- **Logging messages and commands with Discord Webhook**
- **Player Mute/Unmute**
- **Filter ads and bad words in chat messages**
- **Filter ads and bad words in player names**

## Usage

### Commands

| Command                                 | Flag      | Description                                             |
|-----------------------------------------|-----------|---------------------------------------------------------|
| css_mute [steamid64 or name] [duration] | @css/chat | Ban players from chatting for a certain period of time. |
| css_unmute [steamid64 or name]          | @css/chat | Unban a player with a chat ban.                         |

### Working Logic of the Player Muting System
When you define a chat ban for a player, the plugin saves that player in the muted_players.json file.

Saved players are checked every 30 seconds by the plugin to see if the ban has expired. If a player's ban has expired, they are automatically deleted from the muted_players.json file.

Players with a chat ban cannot send messages to the chat. They can only use commands starting with "!" and "/".

> [!NOTE]
> Now that Config Manager is used, the configuration file for the plugin can be found in /addons/counterstrikesharp/configs/plugins/ChatManager/ChatManager/ChatManager.json.
### Sample Tag Creation
```json
  ... Other Settings
  "Tags": {
    "#css/founder": "{Red}[Founder]",
    "@css/ban": "{Purple}[Admin]",
    "@css/vip": "{Gold}[Vip]",
    "everyone": ""
  },
  Other Settings ... 
```

## Roadmap
- [ ] MYSql support for log and other operations
- [ ] Chat commands (rtv, bet etc.)
- [ ] Mute commands will be removed as other plugins do this better.
- [ ] A bug that made messages sent to the team visible to everyone will be fixed.
- [ ] The plugin will be made compatible with the latest CSSharp release.


**The features in the list below are already provided by different plugins, but perhaps it makes more sense to combine them into a single plugin. For this reason I am undecided whether to add them or not.**
- [ ] (maybe) A simple web panel to list saved chat messages
- [ ] (maybe) Create simple chat commands with predefined responses
- [ ] (maybe) Chat announcements running with MYSql
