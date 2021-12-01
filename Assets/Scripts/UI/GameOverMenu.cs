using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public GameObject LoadingPanel;
    GameContext ctx;
    LevelData levelData;

    public void ReturnToMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    private void Start() {
        ctx = GameContext.control;
        levelData = ctx.currentLevelData;
    }

    public void GoToNextLevel() {
        Image image = GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0.4745098f);
        image.raycastTarget = true;
        LoadingPanel.SetActive(true);
        ctx.SetNextLevel(levelData.levelId); // Check
        GetComponent<LoadSceneWithProgress>().LoadSceneNow("LevelGeneric");
    }
}
