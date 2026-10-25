using GorillaLibrary.Extensions;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace GorillaLibrary.Utilities;

public static class AssetBundleUtility
{
    /// <summary>
    /// Loads an asset bundle from an embedded resource.
    /// </summary>
    /// <param name="assembly">The assembly containing the resource.</param>
    /// <param name="path">The resource path.</param>
    /// <returns>The loaded asset bundle.</returns>
    public static AssetBundle LoadBundle(Assembly assembly, string path)
    {
        Stream stream = assembly.GetManifestResourceStream(path);
        AssetBundle bundle = AssetBundle.LoadFromStream(stream);
        stream.Close();
        return bundle;
    }

    /// <summary>
    /// Asynchronously loads an asset bundle from an embedded resource.
    /// </summary>
    /// <param name="assembly">The assembly containing the resource.</param>
    /// <param name="path">The resource path.</param>
    /// <returns>A task containing the loaded asset bundle.</returns>
    public static async Task<AssetBundle> LoadBundleAsync(Assembly assembly, string path)
    {
        Stream stream = assembly.GetManifestResourceStream(path);
        AssetBundleCreateRequest request = AssetBundle.LoadFromStreamAsync(stream);
        await request.AsAwaitable();
        stream.Close();
        return request.assetBundle;
    }

    /// <summary>
    /// Asynchronously loads an asset from a bundle.
    /// </summary>
    /// <typeparam name="T">The asset type.</typeparam>
    /// <param name="bundle">The asset bundle to load from.</param>
    /// <param name="name">The asset name.</param>
    /// <returns>A task containing the loaded asset, or null if unavailable.</returns>
    public static async Task<T> LoadAssetAsync<T>(AssetBundle bundle, string name) where T : Object
    {
        AssetBundleRequest request = bundle.LoadAssetAsync<T>(name);
        await request.AsAwaitable();
        return request.asset as T;
    }

    /// <summary>
    /// Asynchronously loads an asset and its sub-assets from a bundle.
    /// </summary>
    /// <typeparam name="T">The asset type.</typeparam>
    /// <param name="bundle">The asset bundle to load from.</param>
    /// <param name="name">The asset name.</param>
    /// <returns>A task containing the loaded assets.</returns>
    public static async Task<T[]> LoadAssetsWithSubAssetsAsync<T>(AssetBundle bundle, string name) where T : Object
    {
        AssetBundleRequest request = bundle.LoadAssetWithSubAssetsAsync<T>(name);
        await request.AsAwaitable();
        return [.. request.allAssets.Cast<T>()];
    }
}
