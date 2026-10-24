using GorillaGameModes;
using GorillaLibrary.Utilities;

namespace GorillaLibrary.Extensions;

public static class GameModeExtensions
{
    /// <summary>
    /// Returns the display name of a GameModeType.
    /// </summary>
    /// <param name="gameMode">GameModeType to get the display name of.</param>
    /// <returns></returns>
    public static string GetName(this GameModeType gameMode)
    {
        return GameModeUtility.GetGameModeName(gameMode);
    }

    /// <summary>
    /// Returns the game mode's manager from a specified GameModeType
    /// </summary>
    /// <param name="gameMode">GameModeType to get the manager of.</param>
    /// <returns></returns>
    public static GorillaGameManager GetGameManager(this GameModeType gameMode)
    {
        return GameModeUtility.GetGameModeInstance(gameMode);
    }
}
