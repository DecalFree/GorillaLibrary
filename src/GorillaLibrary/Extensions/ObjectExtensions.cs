using UnityEngine;

namespace GorillaLibrary.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// Checks if an object exists.
    /// </summary>
    /// <param name="obj">Object to check.</param>
    /// <returns></returns>
    public static bool IsObjectExistent(this Object obj) => obj != null && obj;

    /// <summary>
    /// Checks if an object is null.
    /// </summary>
    /// <param name="obj">Object to check.</param>
    /// <returns></returns>
    public static bool IsObjectNull(this Object obj) => !obj.IsObjectExistent();
}