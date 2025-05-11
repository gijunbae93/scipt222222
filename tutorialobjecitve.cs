using System;

public class TutorialObjective
{
    public bool isCompleted { get; private set; } // also get in event handler
    public TutorialObjectiveManager.TutorialType tutorialType { get; } // also get in event handler
    TutorialObjectiveManager.TutorialStages tutorialStages { get; }

    public int currentObjectiveCount { get; private set; }
    public int maxObjectiveCount { get; private set; }

    public TutorialObjective(TutorialObjectiveManager.TutorialType tutorialType, TutorialObjectiveManager.TutorialStages tutorialStages, int maxObjectiveCount)
    {
        isCompleted = false;

        this.tutorialType = tutorialType;
        this.tutorialStages = tutorialStages;

        currentObjectiveCount = 0;
        this.maxObjectiveCount = maxObjectiveCount;
    }

    public void CompleteTutorialObjective()
    {
        currentObjectiveCount++;

        if (currentObjectiveCount < maxObjectiveCount) // not ready complete the objective
        {
            TutorialObjectiveManager.instance.InvokeOnObjectiveProgressed(tutorialType, isCompleted);

            return; // this bracket means the tutorial is not completed so exit this method
        }

        isCompleted = true; // this must be set to true before running below foreach loop, if not even if tutorial was suceessful this methdo will return
        TutorialObjectiveManager.instance.InvokeOnObjectiveProgressed(tutorialType, isCompleted);

        foreach (TutorialObjective tutorialObjective in TutorialObjectiveManager.instance.TutorialObjectiveList) // check if other stage tutori object is completed 
        {
            if (tutorialObjective.tutorialStages == tutorialStages)
            {
                if (tutorialObjective.isCompleted)
                    continue; // check if other same tutorial stage can be completed
                else
                    return; // tutorial stage can not be completed return its method
            }
        }

        TutorialManager.instance.InvokeOnNextTutorialStage(tutorialStages); // this will invoke next stage of tutorialStages value
    }
}
