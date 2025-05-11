using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TutorialObjectiveManager : MonoBehaviour
{
    public static TutorialObjectiveManager instance { get; private set; }

    public class TutorialObjectiveData
    {
        public HashSet<TutorialObjective> tutorialObjectiveList = new();
        public Dictionary<TutorialType, TutorialTypeInfo> tutorialTypeInfoDictionary = new Dictionary<TutorialType, TutorialTypeInfo>();
        public HashSet<TutorialType> completedTutorialType = new();
        public bool hasInvasionTriggered = false;
    }
    public TutorialObjectiveData tutorialObjectiveData = new ();
    public HashSet<TutorialObjective> TutorialObjectiveList => tutorialObjectiveData.tutorialObjectiveList;
    public Dictionary<TutorialType, TutorialTypeInfo> TutorialTypeInfoDictionary => tutorialObjectiveData.tutorialTypeInfoDictionary;
    public bool HasInvasionTriggered
    {
        get => tutorialObjectiveData.hasInvasionTriggered;
        set => tutorialObjectiveData.hasInvasionTriggered = value;
    }

    public enum TutorialStages { TutorialStageOne, TutorialStageTwo, TutorialStageThree, TutorialComplete }
    public enum TutorialType
    {
        // TutorialMap One
        FarmMine,  // mines script
        FarmTree, // farm tree script
        GatherThreeWoods, // item inventory script
        GatherThreeStones, // item inventory script
        OpenWorldMap, // uihandler script

        // TutorialMap Two
        CraftAWeapon, // crfat ui script
        KillTwoIslandMonsters, // islandMonsters script
        CraftWoodenWorkShop, // craft ui script
        CraftCampFire, // craft ui script

        // Stage Three
        BuildDefenceWall, // it takes no cost to build defencewall, structureSpawner script
        BuildAttackTower, // it takes no cost to build attackTower, structureSpawner script
        DefenceTheLightBringer, 
    }

    public class TutorialObjectiveCompleteArgs : EventArgs 
    { 
        public TutorialType tutorialType;
        public bool isCompleted; // need this because ui may need to change the current objective completion count
    }
    public event Action<TutorialObjectiveCompleteArgs> OnObjectiveProgressed;

    private void Awake()
    {
        instance = this;

        TutorialObjectiveDataAddition.InitializeTutorialTypeInfoDictionary();
        InitalizeTutorialObjectiveList(); // the order matter this should initalize after initalizing tutorialTypeAndStageDictionary
    }

    private void Start()
    {
        OnObjectiveProgressed += TutorialObjectiveManager_OntutorialObjectiveClear;
    }

    #region Event Subscription
    private void TutorialObjectiveManager_OntutorialObjectiveClear(TutorialObjectiveCompleteArgs obj)
    {
        if (!obj.isCompleted) return; // performance

        if (!MarkTutorialAsCompleted(obj.tutorialType)) return;

        TryToTriggerTutorialInvasion();
    }
    #endregion

    public void RegisterTutorialTypeInfoDictionary(TutorialType tutorialType, string tutorialUiName, TutorialStages tutorialStage, int tutorialObjectiveCount)
    {
        if (!TutorialTypeInfoDictionary.ContainsKey(tutorialType))
            TutorialTypeInfoDictionary[tutorialType] = new TutorialTypeInfo(tutorialUiName, tutorialStage, tutorialObjectiveCount);
    }

    void InitalizeTutorialObjectiveList()
    {
        foreach (TutorialType tutorialType in Enum.GetValues(typeof(TutorialType)))
        {
            TutorialObjective tutorialObjective = new TutorialObjective(tutorialType, TutorialTypeInfoDictionary[tutorialType].tutorialStage, TutorialTypeInfoDictionary[tutorialType].tutorialObjectiveCount);

            TutorialObjectiveList.Add(tutorialObjective);
        }
    }


    bool CheckIfTutorialObjectiveIsComplete(TutorialType tutorialType)
    {
        foreach (TutorialObjective tutorialObjective in TutorialObjectiveList)
            if (tutorialObjective.tutorialType == tutorialType)
                return tutorialObjective.isCompleted;

        Debug.Log("Error");
        return true;
    }

    public void CompleteTutorialObjective(TutorialType tutorialType)
    {
        if (CheckIfTutorialObjectiveIsComplete(tutorialType)) return; // if tutorialobjective is already completed simply return

        foreach (TutorialObjective tutorialObjective in TutorialObjectiveList)
            if (tutorialObjective.tutorialType == tutorialType)
            {
                tutorialObjective.CompleteTutorialObjective();
                break;
            }    
    }

    public TutorialObjective GetTutorialObjective(TutorialType tutorialType)
    {
        foreach (TutorialObjective tutorialObjective in TutorialObjectiveList)
            if (tutorialObjective.tutorialType == tutorialType)
            {
                return tutorialObjective;
            }

        Debug.Log("Error");
        return null;
    }

    bool MarkTutorialAsCompleted(TutorialType type)
    {
        if (tutorialObjectiveData.completedTutorialType.Contains(type))
        {
            Debug.LogWarning($"Tutorial objective '{type}' already marked completed.");
            return false;
        }

        tutorialObjectiveData.completedTutorialType.Add(type);
        return true;
    }

    void TryToTriggerTutorialInvasion()
    {
        if (HasInvasionTriggered) return;

        int i = TutorialManager.instance.InvasionTutorialTypeRequirements.Count;
        int j = 0;
        foreach (TutorialType tutorialType in tutorialObjectiveData.completedTutorialType)
            foreach (TutorialType requiredTutorialType in TutorialManager.instance.InvasionTutorialTypeRequirements)
                if (tutorialType == requiredTutorialType)
                    j++;

        if (j >= i)
        {
            SkeletonInvasionManager.instance.InvokeTutorialInvasion();
            HasInvasionTriggered = true;
        }
    }


    public void InvokeOnObjectiveProgressed(TutorialType tutorialType, bool isCompleted) => OnObjectiveProgressed?.Invoke(new TutorialObjectiveCompleteArgs 
    { 
        tutorialType = tutorialType, 
        isCompleted = isCompleted,
    });

}

public struct TutorialTypeInfo
{
    public readonly string tutorialUiName;
    public readonly TutorialObjectiveManager.TutorialStages tutorialStage;
    public readonly int tutorialObjectiveCount;
    public TutorialTypeInfo(string tutorialUiName, TutorialObjectiveManager.TutorialStages tutorialStage, int tutorialObjectiveCount)
    {
        this.tutorialUiName = tutorialUiName;
        this.tutorialStage = tutorialStage;
        this.tutorialObjectiveCount = tutorialObjectiveCount;
    }
}

public static class TutorialObjectiveDataAddition
{
    public static void InitializeTutorialTypeInfoDictionary()
    {
        TutorialObjectiveManager.TutorialStages tutorialStageOne = TutorialObjectiveManager.TutorialStages.TutorialStageOne;

        TutorialObjectiveManager.instance.RegisterTutorialTypeInfoDictionary(TutorialObjectiveManager.TutorialType.FarmMine, "Mine a rock", tutorialStageOne, 1);
        TutorialObjectiveManager.instance.RegisterTutorialTypeInfoDictionary(TutorialObjectiveManager.TutorialType.FarmTree, "Cut down a tree", tutorialStageOne, 1);
        TutorialObjectiveManager.instance.RegisterTutorialTypeInfoDictionary(TutorialObjectiveManager.TutorialType.GatherThreeWoods, "Gather woods", tutorialStageOne, 3);
        TutorialObjectiveManager.instance.RegisterTutorialTypeInfoDictionary(TutorialObjectiveManager.TutorialType.GatherThreeStones, "Gather stones", tutorialStageOne, 3);
        TutorialObjectiveManager.instance.RegisterTutorialTypeInfoDictionary(TutorialObjectiveManager.TutorialType.OpenWorldMap, "Open world map", tutorialStageOne, 1);

        TutorialObjectiveManager.TutorialStages tutorialStageTwo = TutorialObjectiveManager.TutorialStages.TutorialStageTwo;

        TutorialObjectiveManager.instance.RegisterTutorialTypeInfoDictionary(TutorialObjectiveManager.TutorialType.CraftAWeapon, "Craft a weapon", tutorialStageTwo, 1);
        TutorialObjectiveManager.instance.RegisterTutorialTypeInfoDictionary(TutorialObjectiveManager.TutorialType.KillTwoIslandMonsters, "Defeat monsters", tutorialStageTwo, 2);
        TutorialObjectiveManager.instance.RegisterTutorialTypeInfoDictionary(TutorialObjectiveManager.TutorialType.CraftWoodenWorkShop, "Craft a wooden workshop", tutorialStageTwo, 1);
        TutorialObjectiveManager.instance.RegisterTutorialTypeInfoDictionary(TutorialObjectiveManager.TutorialType.CraftCampFire, "Craft a campfire", tutorialStageTwo, 1);

        TutorialObjectiveManager.TutorialStages tutorialStageThree = TutorialObjectiveManager.TutorialStages.TutorialStageThree;

        TutorialObjectiveManager.instance.RegisterTutorialTypeInfoDictionary(TutorialObjectiveManager.TutorialType.BuildDefenceWall, "Construct defense walls", tutorialStageThree, 3);
        TutorialObjectiveManager.instance.RegisterTutorialTypeInfoDictionary(TutorialObjectiveManager.TutorialType.BuildAttackTower, "Construct attack towers", tutorialStageThree, 4);
        TutorialObjectiveManager.instance.RegisterTutorialTypeInfoDictionary(TutorialObjectiveManager.TutorialType.DefenceTheLightBringer, "Defend the LightBringer", tutorialStageThree, 1);
    }
}
