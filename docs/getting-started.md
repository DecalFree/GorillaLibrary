# Getting Started

To get started, add GorillaLibrary as a reference to your BepInEx mod's dependencies.

# A basic plugin

```csharp
using BepInEx;
using GorillaLibrary;
using HarmonyLib;

[BepInPlugin("com.example.examplemod", "ExampleMod", "1.0.0")]
[BepInDependency("dev.gorillalibrary")]
[ModdedGamemode("testgamemode", "TEST GAMEMODE", GameModeType.Casual)]
public class Testing : BaseUnityPlugin
{
    public Harmony _harmony;
    
    private void Awake()
    {
        Logger.LogInfo("Hello world!");
        
        // Subscribe to OnGameInitialized so we can run code in OnGameInit when the game is fully initialized.
        Events.Core.OnGameInitialized += OnGameInit;
        
        // Run our harmony patches
        _harmony.PatchAll();
    }

    private void OnEnable()
    {
        Logger.LogInfo("On enable!");
    }

    private void OnDisable()
    {
        Logger.LogInfo("On disable!");
        
        // Remove our harmony patches for cleanup
        _harmony.UnpatchSelf();
    }
    
    // Called via the OnGameInitialized event
    private void OnGameInit()
    {
        Logger.LogInfo("Game has initialized!");
    }

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
}

```

