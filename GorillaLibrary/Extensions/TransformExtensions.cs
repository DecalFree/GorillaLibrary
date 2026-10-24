using UnityEngine;

namespace GorillaLibrary.Extensions;

public static class TransformExtensions
{
    /// <summary>
    /// Searches for a child object recursively.
    /// </summary>
    /// <param name="parent">Transform of the parent that will be searched within.</param>
    /// <param name="name">GameObject name of the child being searched for.</param>
    /// <returns>Transform of the searched object, if not found, it will return null.</returns>
    public static Transform FindChildRecursive(this Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.name.Contains(name)) return child;
            Transform transform = child.FindChildRecursive(name);
            if (transform.IsObjectExistent()) return transform;
        }

        return null;
    }
}
