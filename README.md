# CS2-ChatManager
This CSSharp plugin allows managing CS2 chat messages.

> [!NOTE]
> The plugin can partially process commands sent to the chat and allows you to use commands with tags. However, it is recommended to use SilentChatTrigger ("/") for commands that require complex arguments.

> [!NOTE]
> Advertising messages must be added manually to the database for the time being. In the future, a command will be added to create ad messages.

## Description
It was originally released as an enhanced version of the [CS2-Tags](https://github.com/daffyyyy/CS2-Tags) plugin, but now it has been completely revamped and improvements are being made to fulfill its sole purpose of managing server chat.

With this plugin you can create custom tags in your server chat, block ads in player names and messages sent to chat, and create advertising messages.

## Requirments
[CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/)

## Features
- **Chat tags**
- **Chat Ads with MySQL support**
- **Log messages sent to the server (Discord Webhook and Server Logs)**
- **Filter ads and bad words in chat messages**
- **Filter ads and bad words in player names**

## Usage

### Commands

| Command                                 | Flag      | Description                                             |
|-----------------------------------------|-----------|---------------------------------------------------------|
| !adsreload                              | @css/chat | Reload Ads Messages                                     |
