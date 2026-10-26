using GorillaGameModes;
using HarmonyLib;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GorillaLibrary.Utilities;

[Serializable]
public class PlayerProfileResult
{
    public string PlayerId;
    public DateTime? Created;
    public string DisplayName;
}

public static class PlayerUtility
{
    private static readonly Dictionary<string, (PlayerProfileResult playerProfile, DateTime cacheTime)> playerProfileCache = [];

    /// <summary>
    /// Gets a player's PlayFab account information.
    /// </summary>
    /// <param name="userId">The PlayFab ID of the player whose account information to retrieve.</param>
    /// <param name="onPlayerProfileReceived">The callback invoked when account information is received.</param>
    /// <param name="maxCacheTime">The maximum age of cached account information in minutes before fetching new data.</param>
    /// <returns>A PlayerProfileResult of the user requested.</returns>
    public static PlayerProfileResult GetPlayerProfile(string userId, Action<PlayerProfileResult> onPlayerProfileReceived, double maxCacheTime = double.MaxValue)
    {
        if (playerProfileCache.ContainsKey(userId) && (DateTime.Now - playerProfileCache[userId].cacheTime).TotalMinutes < maxCacheTime)
            return playerProfileCache[userId].playerProfile;

        if (!PlayFabClientAPI.IsClientLoggedIn())
            throw new InvalidOperationException("PlayFab Client must be logged in to post the player profile request");

        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest
        {
            PlayFabId = userId,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowAvatarUrl = false,
                ShowBannedUntil = false,
                ShowCampaignAttributions = false,
                ShowContactEmailAddresses = false,
                ShowCreated = true,
                ShowDisplayName = true,
                ShowExperimentVariants = false,
                ShowLastLogin = false,
                ShowLinkedAccounts = false,
                ShowLocations = false,
                ShowMemberships = false,
                ShowOrigination = false,
                ShowPushNotificationRegistrations = false,
                ShowStatistics = false,
                ShowTags = false,
                ShowTotalValueToDateInUsd = false,
                ShowValuesToDate = false
            }
        }, playerProfile =>
        {
            if (playerProfile.PlayerProfile == null)
                return;

            PlayerProfileResult playerProfileResult = new()
            {
                PlayerId = playerProfile.PlayerProfile.PlayerId,
                Created = playerProfile.PlayerProfile.Created,
                DisplayName = playerProfile.PlayerProfile.DisplayName
            };
            playerProfileCache[userId] = (playerProfileResult, DateTime.Now);

            onPlayerProfileReceived?.Invoke(playerProfileResult);
        }, error =>
        {
            Plugin.Logger.LogError(error.GenerateErrorReport());
        });

        return null;
    }

    /// <summary>
    /// Retrieves a player's PlayFab account information.
    /// </summary>
    /// <param name="userId">The PlayFab ID of the player whose account information to retrieve.</param>
    /// <param name="maxCacheTime">The maximum age of cached account information in minutes before fetching new data.</param>
    /// <returns>A task containing the player's account information.</returns>
    public static Task<PlayerProfileResult> GetPlayerProfile(string userId, double maxCacheTime = double.MaxValue)
    {
        TaskCompletionSource<PlayerProfileResult> completionSource = new();

        PlayerProfileResult result = GetPlayerProfile(userId, newResult => {
            if (completionSource.Task.IsCompleted)
                return;
            completionSource.SetResult(newResult);
        }, maxCacheTime);

        if (result != null)
            completionSource.SetResult(result);

        return completionSource.Task;
    }

    /// <summary>
    /// Determines whether a network player is tagged in Infection.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <param name="tagManager">The Infection game mode manager.</param>
    /// <returns></returns>
    public static bool IsTagged(NetPlayer player, GorillaTagManager tagManager)
    {
        return tagManager.currentInfected?.Contains(player) ?? false;
    }

    /// <summary>
    /// Determines whether a network player is the tagger in Infection.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <param name="tagManager">The Infection game mode manager.</param>
    /// <returns></returns>
    public static bool IsTagger(NetPlayer player, GorillaTagManager tagManager)
    {
        return tagManager.currentIt == player;
    }

    /// <summary>
    /// Determines whether a network player is tagged in Hunt.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <param name="huntManager">The Hunt game mode manager.</param>
    /// <returns></returns>
    public static bool IsTagged(NetPlayer player, GorillaHuntManager huntManager)
    {
        return huntManager.currentHunted?.Contains(player) ?? false;
    }

    /// <summary>
    /// Determines whether a network player is eligible to participate in the current game mode.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <returns></returns>
    public static bool IsParticipant(NetPlayer player)
    {
        return (bool)AccessTools.Method(typeof(GameMode), "CanParticipate").Invoke(null, [player]);
    }
}
