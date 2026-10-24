using GorillaLibrary.Utilities;
using Photon.Realtime;
using PlayFab.ClientModels;
using System;

namespace GorillaLibrary.Extensions;

public static class PlayerExtensions
{
    /// <summary>
    /// Returns the NetPlayer of a photon Player
    /// </summary>
    /// <param name="player">Player to get the NetPlayer from.</param>
    /// <returns></returns>
    public static NetPlayer AsNetPlayer(this Player player) => NetworkSystem.Instance.GetPlayer(player.ActorNumber) is NetPlayer netPlayer ? netPlayer : null;

    /// <summary>
    /// Returns the photon Player of a NetPlayer
    /// </summary>
    /// <param name="netPlayer">NetPlayer to get the Player from.</param>
    /// <returns></returns>
    public static Player GetPlayer(this NetPlayer netPlayer) => netPlayer is PunNetPlayer punNetPlayer ? punNetPlayer.PlayerRef : null;

    /// <summary>
    /// Returns the specified NetPlayer's display name.
    /// </summary>
    /// <param name="netPlayer">NetPlayer to get the display name from.</param>
    /// <param name="limitLength">Character limit, if true, it limits the response to the normal 12 characters shown in-game.</param>
    /// <returns></returns>
    public static string GetName(this NetPlayer netPlayer, bool limitLength = true)
    {
        bool isNamePermissionEnabled = KIDManager.CheckFeatureSettingEnabled(EKIDFeatures.Custom_Nametags);
        string nickName = netPlayer.NickName;
        string defaultName = netPlayer.DefaultName;

        string playerName = isNamePermissionEnabled ? string.IsNullOrEmpty(nickName) || string.IsNullOrWhiteSpace(nickName) ? defaultName : nickName : defaultName;
        return limitLength ? playerName.LimitLength(12) : playerName;
    }
    
    /// <summary>
    /// Gets a player's account information from PlayFab.
    /// </summary>
    /// <param name="netPlayer">The NetPlayer whose account information to retrieve.</param>
    /// <param name="callback">The callback invoked when the account information is received.</param>
    /// <param name="maxCacheTime">Maximum age of cached account information in minutes before fetching fresh data.</param>
    /// <returns></returns>
    public static GetAccountInfoResult GetAccountInfo(this NetPlayer netPlayer, Action<GetAccountInfoResult> callback, double maxCacheTime = double.MaxValue)
    {
        return PlayerUtility.GetAccountInfo(netPlayer.UserId, result =>
        {
            callback?.Invoke(result);
        }, maxCacheTime);
    }

    /// <summary>
    /// Returns a DateTime of when the specified NetPlayer's PlayFab account was created.
    /// </summary>
    /// <param name="netPlayer">NetPlayer to get account creation date from</param>
    /// <param name="callback">The callback invoked when the account information is received.</param>
    /// <param name="maxCacheTime">Maximum age of cached account information in minutes before fetching fresh data.</param>
    /// <returns></returns>
    public static DateTime GetAccountCreation(this NetPlayer netPlayer, Action<DateTime> callback, double maxCacheTime = double.MaxValue)
    {
        return PlayerUtility.GetAccountInfo(netPlayer.UserId, result =>
        {
            if (result.AccountInfo?.TitleInfo?.Created is not DateTime created) return;
            callback?.Invoke(created);
        }, maxCacheTime).AccountInfo?.TitleInfo?.Created is DateTime created ? created : DateTime.MinValue;
    }

    /// <summary>
    /// Returns a bool on if the NetPlayer specified has completed the tutorial.
    /// </summary>
    /// <param name="player">NetPlayer to get the data from.</param>
    /// <returns></returns>
    public static bool GetTutorialCompletion(this NetPlayer player)
    {
        return NetworkSystem.Instance.GetPlayerTutorialCompletion(player.ActorNumber);
    }

    /// <summary>
    /// Returns a bool on if the NetPlayer specified is tagged in the current GorillaGameManager.
    /// </summary>
    /// <param name="player">NetPlayer to get the data from.</param>
    /// <returns></returns>
    public static bool IsTagged(this NetPlayer player)
    {
        GorillaGameManager gameManager = GorillaGameManager.instance;
        return gameManager != null && player.IsTagged(gameManager);
    }
    
    /// <summary>
    /// Returns a bool on if the NetPlayer specified is tagged in a specified GorillaGameManager.
    /// </summary>
    /// <param name="player">NetPlayer to get the data from.</param>
    /// <param name="gameManager">GorillaGameManager to get the data from.</param>
    /// <returns></returns>
    public static bool IsTagged(this NetPlayer player, GorillaGameManager gameManager)
    {
        if (gameManager is GorillaHuntManager huntManager)
            return PlayerUtility.IsTagged(player, huntManager);

        if (gameManager is GorillaTagManager tagManager)
            return tagManager.isCurrentlyTag ? PlayerUtility.IsTagger(player, tagManager) : PlayerUtility.IsTagged(player, tagManager);

        return false;
    }

    /// <summary>
    /// Returns a bool on if the specified NetPlayer is a participant.
    /// </summary>
    /// <param name="player">NetPlayer to get the data from.</param>
    /// <returns></returns>
    public static bool IsParticipant(this NetPlayer player)
    {
        return PlayerUtility.IsParticipant(player);
    }
}
