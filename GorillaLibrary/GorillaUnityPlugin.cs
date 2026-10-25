using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Collections.Generic;

namespace GorillaLibrary;

/// <summary>
/// Specialized version of BaseUnityPlugin specifically for Gorilla Tag mods.
/// </summary>
/// <seealso cref="BaseUnityPlugin"/>
public class GorillaUnityPlugin : BaseUnityPlugin
{
    /// <summary>
    /// The mod state. true if enabled and active; otherwise false.
    /// </summary>
    public bool Enabled
    {
        get => _enabled.GetValueOrDefault(true);
        set
        {
            if (_enabled.HasValue && _enabled.Value == value) return;
            _enabled = value;
            _stateEntry.Value = value;
            enabled = value;
        }
    }

    /// <summary>
    /// true if the plugin supports enabling/disabling.
    /// </summary>
    public bool IsStateSupported
    {
        get
        {
            if (!_stateSupported.HasValue)
            {
                List<string> instanceMethods = AccessTools.GetMethodNames(this);
                _stateSupported = instanceMethods.Contains("OnEnable") || instanceMethods.Contains("OnDisable");
            }

            return _stateSupported.Value;
        }
    }

    private bool? _enabled, _stateSupported;
    internal ConfigEntry<bool> _stateEntry;

    /// <summary>
    /// Called when the player is spawned.
    /// </summary>
    public virtual void OnPlayerSpawned() { }

    // these exist in unity but are commented so docFX picks them up and lets users know that you can do this

    /// <summary>
    /// Called when the plugin is enabled. Initialize your mod and its systems.
    /// </summary>
    public virtual void OnEnable() { }

    /// <summary>
    /// Called when the plugin is disabled. Uninitialize your mod here.
    /// </summary>
    public virtual void OnDisable() { }
}
