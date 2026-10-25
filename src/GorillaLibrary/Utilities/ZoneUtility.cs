using GorillaLibrary.Extensions;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace GorillaLibrary.Utilities;

public static class ZoneUtility
{
    /// <summary>
    /// Returns the instance of ZoneManagement.
    /// </summary>
    public static ZoneManagement ZoneManagement
    {
        get
        {
            if (ZoneManagement.instance == null) ZoneManagement.FindInstance();
            return ZoneManagement.instance;
        }
    }

    /// <summary>
    /// Returns all zones in a ZoneData[] dictionary.
    /// </summary>
    public static ZoneData[] Zones
    {
        get
        {
            ZoneManagement instance = ZoneManagement;
            return (ZoneData[])AccessTools.Field(typeof(ZoneManagement), "zones").GetValue(instance);
        }
    }

    public static GameObject[] Objects => ZoneManagement.GetField<GameObject[]>("allObjects");

    /// <summary>
    /// Get a specified zones ZoneData.
    /// </summary>
    /// <param name="zone">Zone to get ZoneData from.</param>
    /// <returns>Specified zones ZoneData.</returns>
    public static ZoneData GetZoneData(GTZone zone)
    {
        ZoneManagement instance = ZoneManagement;
        MethodInfo method = AccessTools.Method(typeof(ZoneManagement), "GetZoneData");
        return (ZoneData)method.Invoke(instance, [zone]);
    }
}
