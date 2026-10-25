using Photon.Pun;

namespace GorillaLibrary.Utilities;

public static class ServerUtility
{
    /// <summary>
    /// Gets the current Photon Cloud region code.
    /// </summary>
    /// <returns>The region code as a string.</returns>
    public static string GetRegionCode() => (PhotonNetwork.CloudRegion ?? NetworkSystem.Instance.CurrentRegion).Replace("/*", "");

    /// <summary>
    /// Gets the display name of the current Photon Cloud region.
    /// </summary>
    /// <returns>The human-readable name of the current region.</returns>
    public static string GetRegionName() => GetRegionNameFromCode(GetRegionCode());
    
    /// <summary>
    /// Gets the display name associated with a Photon Cloud region code.
    /// </summary>
    /// <param name="regionCode">The code of the region to look up.</param>
    /// <returns>The human-readable region name, or the original code if the region is not recognized.</returns>
    public static string GetRegionNameFromCode(string regionCode) => regionCode switch
    {
        "us" => "United States (East)", // Washington DC
        "usw" => "United States (West)", // San Jose
        "eu" => "Europe", // Amsterdam
        "asia" => "Asia", // Singapore
        "au" => "Australia", // Sydney
        "cae" => "Canada", // Montreal
        "hk" => "Hong Kong", // Hong Kong
        "in" => "India", // Chennai
        "jp" => "Japan", // Tokyo
        "za" => "South Africa", // Johannesburg
        "sa" => "South America", // Sao Paulo
        "kr" => "South Korea", // Seoul
        "tr" => "Turkey", // Istanbul
        "uae" => "United Arab Emirates", // Dubai
        "ussc" => "United States (South Central)", // Dallas
        _ => regionCode
    };
}
