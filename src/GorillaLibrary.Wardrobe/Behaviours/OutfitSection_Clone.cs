using GorillaLibrary.Extensions;
using System.Linq;
using UnityEngine;
using static CosmeticWardrobe;
using static GorillaNetworking.CosmeticsController;

namespace GorillaLibrary.Wardrobe.Behaviours;

internal class OutfitSection_Clone : WardrobeCategory
{
    public override string Title => "Clone";

    public void Awake()
    {
        Events.Cosmetics.OnWornCosmeticsUpdated += UpdateCosmetics;
    }

    public override void ApplyCosmetic(CosmeticWardrobeSelection selection, int index)
    {
        CosmeticSet[] outfits = instance.savedOutfits;
        CosmeticSet outfit = index != SelectedOutfit ? outfits[index] : instance.currentWornSet;

        selection.displayHead.SetCosmeticActiveArray([.. outfit.items.Select(item => item.displayName)], outfit.ToOnRightSideArray());
        selection.selectButton.enabled = true;
        selection.selectButton.isOn = index == SelectedOutfit;
        selection.selectButton.UpdateColor();
    }

    public override void SelectCosmetic(int index)
    {
        CosmeticSet[] outfits = instance.savedOutfits;
        outfits[index].CopyItems(instance.currentWornSet);

        Vector3[] colours = instance.savedColors;
        colours[index] = new Vector3(VRRig.LocalRig.playerColor.r, VRRig.LocalRig.playerColor.g, VRRig.LocalRig.playerColor.b);

        instance.SetField("selectedOutfit", SelectedOutfit);
        instance.LoadSavedOutfit(index);

        UpdateCosmetics();
    }

    public override int GetSize()
    {
        return maxOutfits;
    }

    public override void OnActivated(bool hasActivated)
    {
        if (hasActivated)
        {
            int size = GetSize();

            for (int i = 0; i < size; i++)
            {
                CosmeticSet[] outfits = instance.GetField<CosmeticSet[]>("savedOutfits");
                outfits[i].CopyItems(instance.currentWornSet);

                Vector3[] colours = instance.GetField<Vector3[]>("savedColors");
                colours[i] = new Vector3(VRRig.LocalRig.playerColor.r, VRRig.LocalRig.playerColor.g, VRRig.LocalRig.playerColor.b);
            }

            instance.SetField("selectedOutfit", SelectedOutfit == 0 ? 1 : 0);
            instance.LoadSavedOutfit(0);

            UpdateCosmetics();
        }
    }
}
