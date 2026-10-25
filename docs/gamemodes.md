---
uid: gamemodes
title: Custom Gamemodes
author: Lapis
---

# Creating a new gamemode

To create a custom gamemode, you can add the ``ModdedGamemode`` attribute below your BepInEx attributes in your main plugin file.

```csharp
[ModdedGamemode("testgamemode", "TEST GAMEMODE", GameModeType.Casual)]
```

# Detecting when a player is in a custom gamemode

To detect when a player is in a custom gamemode, you can use the ModdedGamemodeJoin and ModdedGamemodeLeave attributes.

```csharp
[ModdedGamemodeJoin]
private void OnModdedGamemodeJoin()
{
    Logger.LogInfo("Joined modded gamemode!");
}
    
[ModdedGamemodeLeave]
private void OnModdedGamemodeLeave()
{
    Logger.LogInfo("Left modded gamemode!");
}
```

For API documention for custom gamemodes, see @GorillaLibrary.Models.GameModeWrapper
