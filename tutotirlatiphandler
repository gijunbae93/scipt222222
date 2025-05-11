using System.Collections.Generic;
using UnityEngine;

public class TutorialTipHandler : MonoBehaviour
{
    public enum TipType
    {
        // Tutorial Stage One
        RunExplanation,
        PickUpExplanation,
        InventoryKeyExplanation,
        HarvestExplanation,
        WorldMapExplanation,

        // TutorialStage Two
        PortalExplanation,
        CraftKeyExplanation,
        WorkShopExplanation,
        DirectionExplanation,

        // Tutorial Stage Three
        PortalExplanationTwo,
        CraftDefenceItemExplanation,
        DefenceItemExplanation,
    }

    public static Dictionary<TutorialObjectiveManager.TutorialStages, List<TipType>> tutorialTipDictionary;

    private void Awake()
    {
        InitalizeTutorialTipDictionary();
    }

    void InitalizeTutorialTipDictionary()
    {
        tutorialTipDictionary = new Dictionary<TutorialObjectiveManager.TutorialStages, List<TipType>>();

        tutorialTipDictionary[TutorialObjectiveManager.TutorialStages.TutorialStageOne] = new List<TipType>
        {
            TipType.RunExplanation,
            TipType.PickUpExplanation,
            TipType.InventoryKeyExplanation,
            TipType.HarvestExplanation,
            TipType.WorldMapExplanation,
        };

        tutorialTipDictionary[TutorialObjectiveManager.TutorialStages.TutorialStageTwo] = new List<TipType>
        {
            TipType.PortalExplanation,
            TipType.CraftKeyExplanation,
            TipType.WorkShopExplanation,
        };

        tutorialTipDictionary[TutorialObjectiveManager.TutorialStages.TutorialStageThree] = new List<TipType>
        {
            TipType.PortalExplanationTwo,
            TipType.DirectionExplanation,
            TipType.CraftDefenceItemExplanation,
            TipType.DefenceItemExplanation,
        };
        tutorialTipDictionary[TutorialObjectiveManager.TutorialStages.TutorialComplete] = new List<TipType> // show nothing when tutorial is complete, just the tutorial three ui disable animation
        {
        };
    }

    public static string GetTipText(TipType tipType)
    {
        switch (tipType)
        {
            default: Debug.Log("Error"); return " ";
            case TipType.RunExplanation: return "Press Shift to run.";
            case TipType.PickUpExplanation: return "Press E to pick up nearby items.";
            case TipType.InventoryKeyExplanation: return "Access your inventory by pressing I";
            case TipType.WorldMapExplanation: return "Press M to open world map";
            case TipType.HarvestExplanation: return "Harvest item using pickaxe and axe from inventory";

            case TipType.PortalExplanation: return "Enter portal to move to next map.";
            case TipType.CraftKeyExplanation: return "Open the crafting menu by pressing C.";
            case TipType.WorkShopExplanation: return "Some structures can only be crafted near a workshop";

            case TipType.PortalExplanationTwo: return "Enter portal to move to next map.";
            case TipType.DirectionExplanation: return "Keep heading South until you see the LightBringer";
            case TipType.CraftDefenceItemExplanation: return "Craft defense structures in the 'Defense' tab.";
            case TipType.DefenceItemExplanation: return "Place defense structures around the tower.";
        }
    }
}
