using GorillaGameModes;
using GorillaLibrary.Behaviours;
using GorillaLibrary.Extensions;
using GorillaLibrary.Models;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GorillaLibrary.Utilities;

public static class GameModeUtility
{
    /// <summary>
    /// The currently active game mode.
    /// </summary>
    public static GameModeWrapper CurrentGameMode { get; internal set; }

    /// <summary>
    /// Finds a game mode from a game mode string.
    /// </summary>
    /// <param name="gmString">The game mode string to search.</param>
    /// <returns>The matching game mode, or null if none is found.</returns>
    public static GameModeWrapper FindGameModeInString(string gmString)
    {
        if (GameModeString.FromString(gmString) is GameModeString gmStringInstance)
        {
            string gameId = gmStringInstance.gameType;
            return GetGameMode(gamemode => gamemode.ID == gameId);
        }

        return GetGameMode(gamemode => gmString.EndsWith(gamemode.ID));
    }

    /// <summary>
    /// Finds a game mode by its ID.
    /// </summary>
    /// <param name="id">The game mode ID.</param>
    /// <returns>The matching game mode, or null if none is found.</returns>
    public static GameModeWrapper FindGameModeFromId(string id) => GetGameMode(gamemode => gamemode.ID == id);

    /// <summary>
    /// Finds the last game mode matching a predicate.
    /// </summary>
    /// <param name="predicate">The condition a game mode must satisfy.</param>
    /// <returns>The matching game mode, or null if none is found.</returns>
    public static GameModeWrapper GetGameMode(Func<GameModeWrapper, bool> predicate)
    {
        return GameModeManager.HasInstance && GameModeManager.Instance.Gamemodes.LastOrDefault(predicate) is GameModeWrapper gameMode ? gameMode : null;
    }

    /// <summary>
    /// Gets the display name of a game mode type.
    /// </summary>
    /// <param name="gameModeType">The game mode type.</param>
    /// <returns>The formatted game mode name.</returns>
    public static string GetGameModeName(GameModeType gameModeType)
    {
        string modeName = GetGameModeInstance(gameModeType) is GorillaGameManager gameManager ? gameManager.GameModeName() : (string)AccessTools.Method(GameMode.GameModeZoneMapping.GetType(), "GetModeName").Invoke(GameMode.GameModeZoneMapping, [gameModeType]);
        return modeName.ToLower() == gameModeType.GetName().ToLower() ? gameModeType.GetName() : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(modeName.ToLower());
    }

    /// <summary>
    /// Gets the active instance of a game mode type.
    /// </summary>
    /// <param name="gameModeType">The game mode type.</param>
    /// <returns>The game mode instance, or null if it does not exist.</returns>
    public static GorillaGameManager GetGameModeInstance(GameModeType gameModeType)
    {
        return GameMode.GetGameModeInstance(gameModeType) is GorillaGameManager gameManager && gameManager.IsObjectExistent() ? gameManager : null;
    }

    /// <summary>
    /// Determines whether a game mode is a super game mode.
    /// </summary>
    /// <param name="gameMode">The game mode type to check.</param>
    /// <returns>True if the game mode is a super game mode.</returns>
    public static bool IsSuperGameMode(this GameModeType gameMode)
    {
        return Enum.IsDefined(typeof(GameModeType), (int)gameMode) && gameMode.GetName().ToLower().StartsWith("super");
    }

    /// <summary>
    /// Gets all tagged players in the current room.
    /// </summary>
    /// <param name="gameManager">The game manager used to check player tag status.</param>
    /// <returns>The tagged players, or an empty collection if not in a room.</returns>
    public static IEnumerable<NetPlayer> GetTaggedPlayers(GorillaGameManager gameManager)
    {
        return NetworkSystem.Instance.InRoom ? NetworkSystem.Instance.AllNetPlayers.Where(player => player.IsTagged(gameManager)) : [];
    }

    /// <summary>
    /// Gets all eligible participants in the current room.
    /// </summary>
    /// <returns>The participants, or an empty collection if not in a room.</returns>
    public static IEnumerable<NetPlayer> GetParticipants()
    {
        return NetworkSystem.Instance.InRoom ? NetworkSystem.Instance.AllNetPlayers.Where(player => player.IsParticipant()) : [];
    }
}
