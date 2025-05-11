using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface TriggerTutorialUiAnimation { public void TriggerTutorialUiAnimation(); }

public class TutorialObjectiveUi : MonoBehaviour, TriggerTutorialUiAnimation
{
    RectTransform tutorialObjectiveUiRectTransform;
    GameObject objectiveTextGameObject;
    Transform tutorialObjectiveBackground;

    Dictionary<TutorialObjectiveManager.TutorialType, RectTransform> tutorialObjectTypeRectTrasnformUiDictionary;

    private void Awake()
    {
        tutorialObjectiveUiRectTransform = transform.Find("TutorialObjectiveUi(Panel)").GetComponent<RectTransform>();
        tutorialObjectiveUiRectTransform.gameObject.SetActive(false);
        objectiveTextGameObject = transform.Find("ObjectiveText(TMP)").gameObject;
        tutorialObjectiveBackground = transform.Find("TutorialObjectiveBackground(Panel)");
    }

    private void Start()
    {
        TutorialManager.instance.OnTutorialInitialization += Instance_OnTutorialInitalization;
        TutorialManager.instance.OnNextTutorialStage += TutorialHandler_OnNextTutorialStage;
        TutorialObjectiveManager.instance.OnObjectiveProgressed += Instance_OntutorialObjectiveClear;
    }

    private void Instance_OnTutorialInitalization(TutorialManager.TutorialInitializationArgs obj)
    {
        if (obj.currentDifficultyLevel != DifficultyLevelManager.DifficultyLevel.Tutorial) // if tutorial is not on tutorial objective ui is not needed
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        TriggerTutorialUiAnimation();
    }

    private void Instance_OntutorialObjectiveClear(TutorialObjectiveManager.TutorialObjectiveCompleteArgs obj) => UpdateTutorialUi(obj.tutorialType, obj.isCompleted);

    private void TutorialHandler_OnNextTutorialStage(object sender, EventArgs e)
    {
        if (TutorialManager.instance.CurrentTutorialStage == TutorialObjectiveManager.TutorialStages.TutorialStageTwo) return; // second tutorial stages Ui is invoked after camera animation ends so invoked by playerCameraHandler

        TriggerTutorialUiAnimation();
    }

    public void TriggerTutorialUiAnimation() // this is for both active and disable animation, also update ui information as well
    {
        float disableAnimationTimer = 1f;
        bool canRunDisableAnimation = false;
        float extraTimeAfterDisableAnimation = 3f;

        foreach (Transform childTransform in transform) // destroy previous ui's
        {
            if (childTransform == tutorialObjectiveUiRectTransform || childTransform == objectiveTextGameObject.transform || childTransform == tutorialObjectiveBackground) continue;

            canRunDisableAnimation = true; // means there are tutorial objective is active and need to run disable animation

            Animator animator = childTransform.GetComponent<Animator>();
            animator.SetTrigger("DisableTutorialObjective");
            FunctionTimer.Create(() =>
            {
                Destroy(childTransform.gameObject);
            }, disableAnimationTimer + extraTimeAfterDisableAnimation - 2, GetType()); // this timer can not be same as below destory gameobject timer, need to destroy game ojbect first in order not to go in infinite loop
        }

        if (canRunDisableAnimation)
        {
            FunctionTimer.Create(() =>
            {
                TriggerTutorialUiAnimation(); // recursive
            }, disableAnimationTimer + extraTimeAfterDisableAnimation, GetType());
            return;
        }

        List<TutorialObjectiveManager.TutorialType> tutorialObjectiveList = new List<TutorialObjectiveManager.TutorialType>();

        foreach (TutorialObjectiveManager.TutorialType tutorialType in Enum.GetValues(typeof(TutorialObjectiveManager.TutorialType)))
            if (TutorialObjectiveManager.instance.TutorialTypeInfoDictionary[tutorialType].tutorialStage == TutorialManager.instance.CurrentTutorialStage)
                tutorialObjectiveList.Add(tutorialType);

        tutorialObjectTypeRectTrasnformUiDictionary = new Dictionary<TutorialObjectiveManager.TutorialType, RectTransform>();

        int y = 0;
        float yOffset = 118.9f;
        float yDistanceBetweenTutorialObjectiveUi = 40.3f;
        foreach (TutorialObjectiveManager.TutorialType tutorialType in tutorialObjectiveList)
        {
            RectTransform tutorialObjectiveRectTransform = Instantiate(tutorialObjectiveUiRectTransform, transform);
            //tutorialObjectiveRectTransform.gameObject.SetActive(true);
            tutorialObjectiveRectTransform.anchoredPosition = new Vector2(0, yOffset + -(y * yDistanceBetweenTutorialObjectiveUi)); // only y position changes , its going up to down so distancebetween should be negative value

            tutorialObjectTypeRectTrasnformUiDictionary[tutorialType] = tutorialObjectiveRectTransform;

            TextMeshProUGUI textMeshProUGUI = tutorialObjectiveRectTransform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = $"{TutorialObjectiveManager.instance.TutorialTypeInfoDictionary[tutorialType].tutorialUiName} {TutorialObjectiveManager.instance.GetTutorialObjective(tutorialType).currentObjectiveCount}/{TutorialObjectiveManager.instance.GetTutorialObjective(tutorialType).maxObjectiveCount}";

            if (TutorialObjectiveManager.instance.GetTutorialObjective(tutorialType).isCompleted) // if tutorial objective is completed
            {
                GameObject completeTMP = tutorialObjectiveRectTransform.Find("Complete (TMP)").gameObject;
                completeTMP.SetActive(true);
            }

            int x = y;
            float animationTimer = 1f;
            float extraWaitTimer = 0.5f;
            FunctionTimer.Create(() =>
            {
                tutorialObjectiveRectTransform.gameObject.SetActive(true); // if it setatctive it will automatically play appearing animation
            }, (animationTimer + extraWaitTimer) * x, GetType());

            y++;
        }
    }

    void UpdateTutorialUi(TutorialObjectiveManager.TutorialType tutorialType, bool isCompleted)
    {
        // this means player is trying to complete the mission ahead of its turn. For example they are at training ground 1 and they are trying to complete training ground 2 like craft sword or craft campfire something like this, it will still complete the objective
        if (!tutorialObjectTypeRectTrasnformUiDictionary.ContainsKey(tutorialType)) return; 

        TextMeshProUGUI textMeshProUGUI = tutorialObjectTypeRectTrasnformUiDictionary[tutorialType].Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = $"{TutorialObjectiveManager.instance.TutorialTypeInfoDictionary[tutorialType].tutorialUiName} {TutorialObjectiveManager.instance.GetTutorialObjective(tutorialType).currentObjectiveCount}/{TutorialObjectiveManager.instance.GetTutorialObjective(tutorialType).maxObjectiveCount}";

        if (!isCompleted) return; // if tutorial objective is completed

        GameObject completeTMP = tutorialObjectTypeRectTrasnformUiDictionary[tutorialType].Find("Complete (TMP)").gameObject;
        completeTMP.SetActive(true);
    }
}
