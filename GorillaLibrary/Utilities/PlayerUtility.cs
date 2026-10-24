using GorillaGameModes;
using HarmonyLib;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GorillaLibrary.Utilities;

public static class PlayerUtility
{
    private static readonly Dictionary<string, (GetAccountInfoResult accountInfo, DateTime cacheTime)> accountInfoCache = [];

    /// <summary>
    /// Gets a player's PlayFab account information.
    /// </summary>
    /// <param name="userId">The PlayFab ID of the player whose account information to retrieve.</param>
    /// <param name="onAccountInfoRecieved">The callback invoked when account information is received.</param>
    /// <param name="maxCacheTime">The maximum age of cached account information in minutes before fetching new data.</param>
    /// <returns>A GetAccountInfoResult of the user requested.</returns>
    public static GetAccountInfoResult GetAccountInfo(string userId, Action<GetAccountInfoResult> onAccountInfoRecieved, double maxCacheTime = double.MaxValue)
    {
        if (accountInfoCache.ContainsKey(userId) && (DateTime.Now - accountInfoCache[userId].cacheTime).TotalMinutes < maxCacheTime)
            return accountInfoCache[userId].accountInfo;

        if (!PlayFabClientAPI.IsClientLoggedIn())
            throw new InvalidOperationException("PlayFab Client must be logged in to post the account info request");

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
        {
            PlayFabId = userId
        }, accountInfo =>
        {
            if (accountInfoCache.ContainsKey(userId)) accountInfoCache[userId] = (accountInfo, DateTime.Now);
            else accountInfoCache.Add(userId, (accountInfo, DateTime.Now));
            onAccountInfoRecieved?.Invoke(accountInfo);
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
    public static Task<GetAccountInfoResult> GetAccountInfo(string userId, double maxCacheTime = double.MaxValue)
    {
        TaskCompletionSource<GetAccountInfoResult> completionSource = new();

        GetAccountInfoResult result = GetAccountInfo(userId, newResult =>
        {
            if (completionSource.Task.IsCompleted) return;
            completionSource.SetResult(newResult);
        }, maxCacheTime);

        if (result != null) completionSource.SetResult(result);

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
