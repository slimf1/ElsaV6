# Elsa V6

A .NET Core PS! Bot

Work in progress ! 

## Install & Run

### Windows 

1. Open the solution with Visual Studio 2019 (or later)
2. Rename `config-example.json` in the `ElsaV6/Resource` folder to `config.json` and fill in the relevant fields
3. Set "ElsaV6" as the default project and run

### Linux 

1. Install the `dotnet` package [(Installation docs)](https://docs.microsoft.com/en-us/dotnet/core/install/)
2. Rename `config-example.json` in the `ElsaV6/Resource` folder to `config.json` and fill in the relevant fields
3. Run `dotnet build`
4. Move to the `bin` folder and run the `ElsaV6` executable

## Configuration

|Field|Value|
|-|-|
|Host|The address of the server|
|Port|The port of the connection|
|Name|The name of the bot|
|Password|The password of the user name used|
|Log|Print or not messages in the console|
|Rooms|The rooms the bot will join|
|Whitelist|Admin users|
|Trigger|The string used to indicate a bot command|
|Blacklist|The users ignored by the bot|
|RoomBlacklist|Ignored chat rooms|
|DefaultRoom|Default chat room for PM features that need a room|
