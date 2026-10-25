# Getting Started

To get started, add GorillaLibrary as a reference to your BepInEx mod's dependencies.

# A basic plugin

```csharp
using BepInEx;
using GorillaLibrary;
using HarmonyLib;

[BepInPlugin("com.example.examplemod", "ExampleMod", "1.0.0")]
[BepInDependency("dev.gorillalibrary")]
public class Testing : BaseUnityPlugin
{
    public Harmony _harmony;
    
    private void Awake()
    {
        Logger.LogInfo("Hello world!");
        Events.Core.OnGameInitialized += OnGameInit;
        _harmony.PatchAll();
    }

    private void OnEnable()
    {
        Logger.LogInfo("On enable!");
    }

    private void OnDisable()
    {
        Logger.LogInfo("On disable!");
        _harmony.UnpatchSelf();
    }

    private void OnGameInit()
    {
        Logger.LogInfo("Game has initialized!");
    }   
}

```

