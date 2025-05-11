using System.Collections.Generic;
using System;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance { get; private set; }

    class TutorialConstantData
    {
        public readonly TutorialObjectiveManager.TutorialStages startingTutorialStage;
        public readonly List<TutorialObjectiveManager.TutorialType> invasionTutorialTypeRequirements = new List<TutorialObjectiveManager.TutorialType>()
        {
            TutorialObjectiveManager.TutorialType.BuildAttackTower,
            TutorialObjectiveManager.TutorialType.BuildDefenceWall,
        };
        public readonly float timerForAfterTasteAfterCompletingAllTutorialObjective = 3f; // switching scene right after tutorial complete is werid
    }
    TutorialConstantData tutorialConstantData = new TutorialConstantData();
    public List<TutorialObjectiveManager.TutorialType> InvasionTutorialTypeRequirements => tutorialConstantData.invasionTutorialTypeRequirements;
    public TutorialObjectiveManager.TutorialStages StartingTutorialStage => tutorialConstantData.startingTutorialStage;

    class TutorialRunTimeData
    {
        public TutorialObjectiveManager.TutorialStages currentTutorialStage;
    }
    TutorialRunTimeData tutorialRunTimeData = new TutorialRunTimeData();
    public TutorialObjectiveManager.TutorialStages CurrentTutorialStage
    {
        get => tutorialRunTimeData.currentTutorialStage;
        set => tutorialRunTimeData.currentTutorialStage = value;
    }

    public class TutorialInitializationArgs : EventArgs { public DifficultyLevelManager.DifficultyLevel currentDifficultyLevel; }
    public event Action<TutorialInitializationArgs> OnTutorialInitialization;


    public class TutorialNextStageArgs : EventArgs
    {
        //for entering stage two
        public float playerPauseTimer => 0.5f;
        public float cameraTransitionTimer => 1f; // after this value camera will transition into tutorial one teleport camera

    }
    public event EventHandler<TutorialNextStageArgs> OnNextTutorialStage;

    ITutorialStartupController tutorialStartupController = new TutorialStartupController();

    private void Awake()
    {
        instance = this;

        OnTutorialInitialization += TutorialManager_OnTutorialInitalization;
    }


    private void Start()
    {
        OnNextTutorialStage += TutorialHandler_OnNextTutorialStage;

        NextFrame.Create(() => OnTutorialInitialization?.Invoke(new TutorialInitializationArgs { currentDifficultyLevel = DifficultyLevelManager.GetCurrentGameLevel() })); // need next frame because some class might subscribe in start mehtod
    }
    private void OnDestroy() => OnNextTutorialStage -= TutorialHandler_OnNextTutorialStage; // static

    private void TutorialManager_OnTutorialInitalization(TutorialInitializationArgs e)
    {
        if (DifficultyLevelManager.GetCurrentGameLevel() == DifficultyLevelManager.DifficultyLevel.Tutorial)
            tutorialStartupController.ActivateTutorialItems();
        else
            tutorialStartupController.DisableTutorialItems();

        CurrentTutorialStage = tutorialConstantData.startingTutorialStage;
    }

    private void TutorialHandler_OnNextTutorialStage(object sender, EventArgs e)
    {
        if (CurrentTutorialStage != TutorialObjectiveManager.TutorialStages.TutorialComplete) return;

        TutorialComplete();
    }

    void TutorialComplete()
    {
        AudioHandler.PlayBackGroundMusic(AudioHandler.Sound.mainSceneBGM);
        FunctionTimer.Create(() => Loader.Load(Loader.Scene.StartMenu), tutorialConstantData.timerForAfterTasteAfterCompletingAllTutorialObjective, GetType());
    }


    public void InvokeOnNextTutorialStage(TutorialObjectiveManager.TutorialStages tutorialStage)
    {
        int nextTutorialStageInt = (int)++tutorialStage; // need to use pre increment not post increment ( not (int)tutorialStage++) this is post increment and nextTutorialStageInt value is not incremented by one because order of operation
        CurrentTutorialStage = (TutorialObjectiveManager.TutorialStages)nextTutorialStageInt; // this must be called before invokeing OnNextTutorialStage

        OnNextTutorialStage.Invoke(null, new TutorialNextStageArgs { });
    }
}

// tutorial should include, craft, structure building
