# CS2-ChatManager
This CSSharp plugin allows managing CS2 chat messages.

## Description
It was originally released as an enhanced version of the [CS2-Tags](https://github.com/daffyyyy/CS2-Tags) plugin, but now it has been completely revamped and improvements are being made to fulfill its sole purpose of managing server chat.

With this plugin you can edit the syntax in your server chat, define tags for players, block ads and more.

## Requirments
[CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/) **v88 >=**

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
- [ ] Chat commands (rtv, bet etc.)
- [x] Filter bad words and ads in chat messages
- [x] Filtering player names with predefined words
- [x] Editing syntax in a chat
- [x] General code regulations
- [x] Migration to Config Manager
