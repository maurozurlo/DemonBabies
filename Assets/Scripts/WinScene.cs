using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinScene : MonoBehaviour
{
    public GameObject LoadingPanel;
    public TextMeshProUGUI defense, attacks, powerups, maxcombo, badtrips, totalTime;
    GameContext ctx;
    LevelData levelData;

    public void ReturnToMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    private void Start() {
        ctx = GameContext.control;
        levelData = ctx.currentLevelData;

        defense.text = levelData.defense.ToString();
        attacks.text = levelData.attacks.ToString();
        powerups.text = levelData.powerUps.ToString();
        maxcombo.text = levelData.maxCombo.ToString();
        badtrips.text = levelData.badTrips.ToString();
        System.TimeSpan interval = new System.TimeSpan(0, 0, 0, 0, levelData.finishTime * 10);
        totalTime.text = interval.ToString(@"mm\:ss\.ff");

    }

    public void GoToNextLevel() {
        Image image = GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0.4745098f);
        image.raycastTarget = true;
        LoadingPanel.SetActive(true);
        GameConfig cfg = ctx.SetNextLevel(levelData.levelId + 1); // Check
        if(cfg != null) {
            GetComponent<LoadSceneWithProgress>().LoadSceneNow("LevelGeneric");
        } else if(cfg == null) {
            GetComponent<LoadSceneWithProgress>().LoadSceneNow("Ending");
        }
    }
}
