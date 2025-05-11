using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface TriggerTutorialTipUiAnimation { public void TriggerTutorialUiAnimation(List<TutorialTipHandler.TipType> tipTypeList); }

public class TipUi : MonoBehaviour, TriggerTutorialTipUiAnimation
{
    Transform tipTextTransform;
    Transform backGroundUi;

    RectTransform tipRectTransform;

    private void Awake()
    {
        tipTextTransform = transform.Find("Tip(TMP)");
        tipTextTransform.gameObject.SetActive(true);
        tipRectTransform = transform.Find("Tip(Panel)").GetComponent<RectTransform>();
        tipRectTransform.gameObject.SetActive(false);
        backGroundUi = transform.Find("TipBackGround(Panel)");
        backGroundUi.gameObject.SetActive(true);
    }

    private void Start()
    {
        TutorialManager.instance.OnTutorialInitialization += Instance_OnTutorialInitalization;
        TutorialManager.instance.OnNextTutorialStage += TutorialHandler_OnNextTutorialStage;
    }

    #region Event Subscribe
    private void Instance_OnTutorialInitalization(TutorialManager.TutorialInitializationArgs obj)
    {
        if(obj.currentDifficultyLevel != DifficultyLevelManager.DifficultyLevel.Tutorial)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        TriggerTutorialUiAnimation(TutorialTipHandler.tutorialTipDictionary[TutorialManager.instance.StartingTutorialStage]); // this can not run in awake method tutorialTipDictionary is initalized in awake method
    }

    private void TutorialHandler_OnNextTutorialStage(object sender, TutorialManager.TutorialNextStageArgs e)
    {
        if (TutorialManager.instance.CurrentTutorialStage == TutorialObjectiveManager.TutorialStages.TutorialStageTwo) return; // update ui for tutorial stage 2 is inovked in plyaer camera handler after camera animation ends

        TriggerTutorialUiAnimation(TutorialTipHandler.tutorialTipDictionary[TutorialManager.instance.CurrentTutorialStage]); // TutorialManager.instance.CurrentTutorialStage is updated before invoking this event
    }
    #endregion

    public void TriggerTutorialUiAnimation(List<TutorialTipHandler.TipType> tipTypeList)
    {
        float disableAnimationTimer = 1f;
        bool canRunDisableAnimation = false;
        float extraTimeAfterDisableAnimation = 3f;

        foreach (Transform childTransform in transform)
        {
            if (childTransform == tipTextTransform || childTransform == tipRectTransform || childTransform == backGroundUi) continue;

            canRunDisableAnimation = true; // means there are tutorial objective is active and need to run disable animation

            Animator animator = childTransform.GetComponent<Animator>();
            animator.SetTrigger("DisableTip");
            FunctionTimer.Create(() =>
            {
                Destroy(childTransform.gameObject);
            }, disableAnimationTimer + extraTimeAfterDisableAnimation - 2, GetType()); // this value can not be same as below destory gameobject timer;
        }

        if (canRunDisableAnimation)
        {
            FunctionTimer.Create(() =>
            {
                TriggerTutorialUiAnimation(tipTypeList); // recursive
            }, disableAnimationTimer + extraTimeAfterDisableAnimation, GetType());
            return;
        }

        int y = 0;
        float yOffSet = 61.9f;
        float spaceBetweenUi = 45.8f;

        foreach (TutorialTipHandler.TipType tipType in tipTypeList)
        {
            RectTransform spawnedTipRectTransform = Instantiate(tipRectTransform, transform);
            spawnedTipRectTransform.anchoredPosition = new Vector2(spawnedTipRectTransform.anchoredPosition.x, yOffSet - (spaceBetweenUi * y)); // y is going down so its minus

            TextMeshProUGUI tipText = spawnedTipRectTransform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            tipText.text = TutorialTipHandler.GetTipText(tipType);

            float animationTimer = 1; // each animation Timer
            float extraWaitTimer = 0.5f;

            int x = y;
            FunctionTimer.Create(() =>
            {
                spawnedTipRectTransform.gameObject.SetActive(true); // this will invoke animation automatically
            }, (animationTimer + extraWaitTimer) * x, GetType());

            y++;
        }
    }
}
