using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {
    public Toggle tauntToggle;
    public Slider musicSlid, fxSlid;
    public AudioClip click;
    public GameObject subtitle, HelpButton, CreditsButton;
    // Start is called before the first frame update
    public GameObject[] panels;
    GameContext gameContext;
    // Audio
    public AudioClip exterminateBabies;
    public AudioClip music;
    public float delay;
    [Header("Audio Sources")]
    public AudioSource AS_M, AS_FX;

    private void Start() {
        gameContext = GameContext.control;

        PlayerData playerData = gameContext.GetGameData();
        // Setup
        // Taunts
        bool taunt = System.Convert.ToBoolean(playerData.taunts);
        tauntToggle.isOn = taunt;
        // FX
        fxSlid.value = gameContext.GetVolume("fx",false);
        musicSlid.value = gameContext.GetVolume("music",false);

        StartCoroutine("playSound");
        
        AS_FX.volume = gameContext.GetVolume("fx");
        AS_M.volume = gameContext.GetVolume("music");
    }

    IEnumerator playSound() {
        yield return new WaitForSeconds(delay);
        AS_FX.PlayOneShot(exterminateBabies);
        yield return new WaitForSeconds(exterminateBabies.length + delay);
        PlayMusic();
    }

    void PlayMusic() {
        AS_M.volume = gameContext.GetVolume("music");
        AS_M.loop = true;
        AS_M.clip = music;
        AS_M.Play();
    }

    void HideAllPanels() {
        foreach (GameObject panel in panels) {
            panel.SetActive(false);
        }
    }

    public void DisplayPanel(string panelToEnable) {
        ButtonClick();
        HideAllPanels();
        GameObject panel = GetPanel(panelToEnable);
        panel.SetActive(true);
    }

    public GameObject GetPanel(string panelToEnable) {
        GameObject selectedPanel = null;
        foreach (GameObject panel in panels) {
            if (panel.name == panelToEnable) {
                selectedPanel = panel;
            }
        }
        return selectedPanel;
    }

    public void LoadLevel(int levelToLoad) {
        ButtonClick();
        HideAllPanels();
        GameObject panel = GetPanel("Loading");
        panel.SetActive(true);
        gameContext.SetNextLevel(levelToLoad);
        panel.GetComponent<LoadSceneWithProgress>().LoadSceneNow("LevelGeneric");
    }

    public void Continue()
    {
        LoadLevel(gameContext.GetMaxUnlockedLevel());
    }

    public void ButtonClick() {
        if (!AS_FX.isPlaying && click != null) {
            AS_FX.PlayOneShot(click);
        }
    }

    public void SaveConfig() {
        PlayerData _playerData = gameContext.GetGameData();
        long startTime = _playerData.startTime;
        int hasFinishedTheGame = _playerData.hasFinishedTheGame;
        int hasSeenLoseCutscene = _playerData.hasSeenLoseCutscene;
        int taunt = tauntToggle.isOn ? 1 : 0;
        PlayerData newPlayerData = new PlayerData(startTime, (int)fxSlid.value, (int)musicSlid.value, taunt, hasFinishedTheGame, hasSeenLoseCutscene);
        gameContext.SavePlayerData(newPlayerData);

        AS_FX.volume = fxSlid.value / 10;
        AS_M.volume = musicSlid.value / 10;

        HideAllPanels();
        DisplayPanel("Main");
    }

    public void ActivateMenu() {
        subtitle.SetActive(true);
        DisplayPanel("Main");
        HelpButton.SetActive(true);
        CreditsButton.SetActive(true);
    }

    public void QuitApp() {
        Application.Quit();
    }

}
