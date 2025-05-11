using UnityEngine;


public interface ITutorialStartupController
{
    public void ActivateTutorialItems();
    public void DisableTutorialItems();
}
public class TutorialStartupController : ITutorialStartupController
{
    public void ActivateTutorialItems() // interface
    {
        ActivateTutorialAttackTowerPreviewArray();
        ActivateTutorialDefenceWallPreviewArray();

        EnableTutorialGameObjects();
    }
    public void DisableTutorialItems()  // interface
    {
        DisableTutorialAttackTowerPreviewArray();
        DisableTutorialDefenceWallPreviewArray();

        DisableTutorialGameObjects();
    }


    void ActivateTutorialAttackTowerPreviewArray()
    {
        foreach (var item in GameAssetManager.instance.tutorialAsset.tutorialAttackTowerPreviewArray)
            item.SetActive(true);
    }
    void DisableTutorialAttackTowerPreviewArray()
    {
        foreach (var item in GameAssetManager.instance.tutorialAsset.tutorialAttackTowerPreviewArray)
            item.SetActive(false);
    }

    void ActivateTutorialDefenceWallPreviewArray()
    {
        foreach (var item in GameAssetManager.instance.tutorialAsset.tutorialDefenceWallPreviewArray)
            item.SetActive(true);
    }
    void DisableTutorialDefenceWallPreviewArray()
    {
        foreach (var item in GameAssetManager.instance.tutorialAsset.tutorialDefenceWallPreviewArray)
            item.SetActive(false);
    }

    void EnableTutorialGameObjects()
    {
        GameAssetManager.instance.tutorialAsset.tutorialVisualTemple.SetActive(true);
        GameAssetManager.instance.tutorialAsset.playerTeleportScrollTarget.SetActive(true);
    }

    void DisableTutorialGameObjects()
    {
        GameAssetManager.instance.tutorialAsset.tutorialVisualTemple.SetActive(false);
        GameAssetManager.instance.tutorialAsset.playerTeleportScrollTarget.SetActive(false);
    }
}
