using GorillaLibrary.Models;
using HarmonyLib;
using UnityEngine;
using static GorillaNetworking.CosmeticsController;

namespace GorillaLibrary.Extensions;

public static class RigExtensions
{
    /// <summary>
    /// Returns the scale factor of a VRRig as a float.
    /// </summary>
    /// <param name="rig">VRRig to get the scale factor of.</param>
    /// <returns>Returns a float of the specified VRRig's scale factor.</returns>
    public static float GetScaleFactor(this VRRig rig)
    {
        return (float)AccessTools.Field(typeof(VRRig), "lastScaleFactor").GetValue(rig);
    }

    public static void GetCosmetics(this VRRig rig, out CosmeticSet currentWornSet, out CosmeticSet tryOnSet)
    {
        currentWornSet = rig.cosmeticSet;
        tryOnSet = rig.tryOnSet;
    }

    /// <summary>
    /// Returns a bool of if the specified VRRig is within the try on room boundaries.
    /// </summary>
    /// <param name="rig">VRRig to check.</param>
    /// <returns></returns>
    public static bool InTryOnRoom(this VRRig rig)
    {
        return rig.inTryOnRoom;
    }

    /// <summary>
    /// Gets the currently worn cosmetics of a players VRRig.
    /// </summary>
    /// <param name="rig">VRRig to get the cosmetics of.</param>
    /// <returns>The CosmeticSet of the specified VRRig.</returns>
    public static CosmeticSet GetCosmetics(this VRRig rig)
    {
        rig.GetCosmetics(out CosmeticSet current, out CosmeticSet tryOn);
        return rig.InTryOnRoom() ? tryOn : current;
    }

    /// <summary>
    /// Gets the GorillaIK instance of a VRRig
    /// </summary>
    /// <param name="rig">VRRig to get the GorillaIK instance of.</param>
    /// <returns>The GorillaIK instance of the specified VRRig.</returns>
    public static GorillaIK GetGorilaIK(this VRRig rig)
    {
        GorillaIK ik = (GorillaIK)AccessTools.Field(typeof(VRRig), "myIk").GetValue(rig);
        return ik ?? rig.GetComponent<GorillaIK>();
    }

    /// <summary>
    /// Returns a Transform of a specified players GorillaRigBone in their IK.
    /// </summary>
    /// <param name="rig">VRRig to get the bone of</param>
    /// <param name="bone">GorillaRigBone to get the Transform of.</param>
    /// <returns>The Transform of the GorillaRigBone specified.</returns>
    public static Transform GetBone(this VRRig rig, GorillaRigBone bone)
    {
        GorillaIK ik = rig.GetGorilaIK();
        return bone switch
        {
            GorillaRigBone.Head => ik.headBone ?? rig.headMesh.transform,
            GorillaRigBone.Body => ik.bodyBone.Find("body") ?? ik.bodyBone,
            GorillaRigBone.LeftUpperArm => ik.leftUpperArm ?? ik.bodyBone.Find("shoulder.L"),
            GorillaRigBone.RightUpperArm => ik.rightUpperArm ?? ik.bodyBone.Find("shoulder.R"),
            GorillaRigBone.LeftLowerArm => ik.leftLowerArm ?? ik.bodyBone.Find("shoulder.L/forearm.L"),
            GorillaRigBone.RightLowerArm => ik.rightLowerArm ?? ik.bodyBone.Find("shoulder.R/forearm.R"),
            GorillaRigBone.LeftHand => ik.leftHand,
            GorillaRigBone.RightHand => ik.rightHand,
            _ => null
        };
    }
}
