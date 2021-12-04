using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public GameObject LoadingPanel;
    GameContext ctx;
    LevelData levelData;

    public GameObject CanvasAnim, CanvasSkip, TouchPanel;

    public AudioSource AS;

    public float timeToEndAnimation = 5;

    bool canSkipCutscene = true;
    
    public void ReturnToMenu() {
        SavePlayerData();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    private void Start() {
        ctx = GameContext.control;
        levelData = ctx.currentLevelData;
        StartCoroutine("AllowSkip");
    }

    IEnumerator AllowSkip()
    {
        yield return new WaitForSecondsRealtime(timeToEndAnimation);
        TouchPanel.SetActive(false);
        canSkipCutscene = false;
    }


    public void GoToNextLevel() {
        SavePlayerData();
        Image image = GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0.4745098f);
        image.raycastTarget = true;
        LoadingPanel.SetActive(true);
        ctx.SetNextLevel(levelData.levelId); // Check
        GetComponent<LoadSceneWithProgress>().LoadSceneNow("LevelGeneric");
    }

    public void SkipCutscene()
    {
        if (ctx.currentSavedData.playerData.hasSeenLoseCutscene == 1 && canSkipCutscene)
        {
            CanvasAnim.SetActive(false);
            CanvasSkip.SetActive(true);
            AS.Stop();
        }
    }

    void SavePlayerData()
    {
        PlayerData _playerData = ctx.GetGameData();
        PlayerData newPlayerData = _playerData;
        newPlayerData.hasSeenLoseCutscene = 1;
        ctx.SavePlayerData(newPlayerData);
    }



}
