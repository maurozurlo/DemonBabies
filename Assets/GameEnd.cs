using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameEnd : MonoBehaviour
{
    VideoPlayer videoPlayer;
    public GameObject Canvas;
    
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        GameContext gameContext = GameContext.control;
        if(gameContext) videoPlayer.SetDirectAudioVolume(0, gameContext.GetVolume("music"));

        // Save data
        PlayerData playerData = gameContext.GetPlayerData();
        playerData.hasFinishedTheGame = 1;

        gameContext.SavePlayerData(playerData);
    }

    void Update()
    {
        if (videoPlayer.frame > 0 && videoPlayer.isPlaying == false) { 
            OpenMenu();
        }
    }

    void OpenMenu() {
        Canvas.SetActive(true);
    }

    public void GoBackToMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
