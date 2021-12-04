using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    //Game states
    // New implementation
    public enum GameState {
        idle, play, pause, end
    };
    public GameState gameState;

    //UI
    CameraFunctions cam;
    public Text scoreText;
    public Text babyText;
    public Text pauseText, cuentaRegresiva;
    public Image damageScreen;
    public Button menuBut;
    int delay = 3;

    //Game config
    float enemySpeed;
    int healthPoints;
    int enemiesSpawned, powerUpsSpawned;

    public bool hasCompletedThisLevelBefore;

    GameConfig config;
    GameContext ctx;

    float speedMultiplier;
    float maxSpeed;
    int scoreCombo; // Used to calculate Combos


    
    // Data to save for end of level
    [Header("Level Data to save")]
    [SerializeField]
    int maxCombo;
    [SerializeField]
    int enemiesDefeated;
    [SerializeField]
    int attacksReceived;
    [SerializeField]
    int badTrips;
    [SerializeField]
    int powerUpsActivated;
    [SerializeField]
    int attempts = 1;
    [SerializeField]
    float levelTime;
    

    //Singleton
    public static GameController control;

    public GameObject _genericSpawner;
    GenericSpawner genericSpawner;
    //Audio
    AudioSource AS;
    public AudioClip[] hurtSound;
    public AudioClip youWon;
    public bool upsideDownWorld;

    void Awake() {
        // Singleton Pattern
        if (!control) control = this;
        else Destroy(gameObject);

        // Get config
        ctx = GameContext.control;
        config = ctx.GetGameConfig();
        LevelData levelData = ctx.GetLevelData();

        if (levelData != null) {
            attempts = levelData.attempts;
            hasCompletedThisLevelBefore = levelData.hasBeenFinished == 1;
        }

        healthPoints = config.health;
        enemySpeed = config.baseSpeed;
        speedMultiplier = config.speedMultiplier;
        maxSpeed = config.maxSpeed;

        // Init Level
        AS = gameObject.AddComponent<AudioSource>();
        AS.volume = ctx.GetVolume("fx");
        cam = Camera.main.GetComponent<CameraFunctions>();
        if (!cam) Debug.LogError("Camera functions not set up");
        // Countdown + GameState
        gameState = GameState.idle;
        // Spawner
        genericSpawner = _genericSpawner.GetComponent<GenericSpawner>();
        UpdateUI();
        StartCoroutine("SetUpStuff");
    }

    void Update() {
        if(gameState == GameState.play) levelTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseRoutine();
    }
    public void PauseRoutine() {
        if (gameState == GameState.play) {
            Pause();
            return;

        }
        if (gameState == GameState.pause) {
            Resume();
            return;
        }
    }

    public void Pause() {
        gameState = GameState.pause;
        genericSpawner.Pause();
        Time.timeScale = 0;
        pauseText.gameObject.SetActive(true);
        menuBut.gameObject.SetActive(true);
    }

    public void Resume() {
        gameState = GameState.play;
        genericSpawner.Resume();
        Time.timeScale = 1;
        pauseText.gameObject.SetActive(false);
        menuBut.gameObject.SetActive(false);
    }

    IEnumerator SetUpStuff() {
        for (int i = 0; i < delay; i++) {
            cuentaRegresiva.text = (delay - i).ToString();
            yield return new WaitForSeconds(1);
        }
        cuentaRegresiva.text = "DALEE!!!";
        yield return new WaitForSeconds(1);
        cuentaRegresiva.text = "";
        gameState = GameState.play;
        genericSpawner.StartSpawning();
        InvokeRepeating("IncreaseSpeed", 0, 5);
    }

    public void UpdateUI() {
        babyText.text = $"{enemiesSpawned}/{config.enemies}";
        scoreText.text = "= " + healthPoints.ToString();
    }

    public void AddScorePoints(int healthToAdd) {
        enemiesDefeated++;

        healthPoints += healthToAdd;
        scoreCombo++;
        if (scoreCombo > maxCombo) maxCombo = scoreCombo;


        UpdateUI();
        CheckForEndOfLevel();
    }

    public void TakeDamage(int dmgToTake, Color col, string origin) {
        if(origin == "baby") attacksReceived++;

        healthPoints -= dmgToTake;
        scoreCombo = 0; // Reset combo
        UpdateUI();
        // Check if dead
        if (healthPoints <= 0) Die();


        if (!AS.isPlaying) {
            AS.PlayOneShot(hurtSound[Random.Range(0, hurtSound.Length)]);
        }
        // Camera shake
        if (dmgToTake > 0) StartCoroutine(cam.Shake(.4f, .2f));
        // Damage Screen
        StartCoroutine(DamageScreen(col));
        CheckForEndOfLevel();
    }

    IEnumerator DamageScreen(Color col) {
        damageScreen.color = col;
        damageScreen.gameObject.SetActive(true);
        damageScreen.GetComponent<ImgHandlers>().replayAnim();
        yield return new WaitForSeconds(1);
        damageScreen.gameObject.SetActive(false);
    }

    void Die() {
        attempts++;
        SendDataToContext();
        gameState = GameState.end;
        SceneManager.LoadScene("GameOver");
    }

    public void CheckForEndOfLevel() {
        if (enemiesSpawned != config.enemies) return; // Check if we spawned all the enemies for this level
        if (genericSpawner.HasChildren()) return;
        StartCoroutine("WinRoutine");
    }

    IEnumerator WinRoutine() {
        cuentaRegresiva.text = "YOU WIN";
        AS.PlayOneShot(youWon);
        attempts++;
        SendDataToContext(true);
        yield return new WaitForSeconds(youWon.length + 1);
        SceneManager.LoadScene("YouWon");
    }

    public void IncreaseSpeed() {
        if(enemySpeed * speedMultiplier >= maxSpeed) {
            CancelInvoke("IncreaseSpeed");
        }
        else {
            enemySpeed *= speedMultiplier;
        }
    }

    public void IncrementEnemiesSpawned() {
        enemiesSpawned++;
        UpdateUI();
    }

    public void IncrementPowerUpsSpawned() {
        powerUpsSpawned++;
    }

    public int GetEnemiesSpawned() {
        return enemiesSpawned;
    }

    public int GetPowerUpsSpawned() {
        return powerUpsSpawned;
    }
    public GameState GetGameState() {
        return gameState;
    }

    public float GetEnemySpeed() {
        return enemySpeed;
    }

    public int GetScoreCombo() {
        return scoreCombo;
    }

    public void AddToBadTrips() {
        badTrips++;
    }

    public void AddToPowerUps() {
        powerUpsActivated++;
    }

    void SendDataToContext(bool changeWinStatus = false) {
        // Check if level has been completed before,
        int completedThisLevelBefore = hasCompletedThisLevelBefore ? 1 : 0;
        int won = changeWinStatus ? 1 : completedThisLevelBefore;

        int levelId = config.levelNumber;

        int time = (int)Mathf.Round(levelTime * 100);
        LevelData levelData = new LevelData(levelId, enemiesDefeated, attacksReceived, powerUpsActivated, maxCombo, badTrips, time, attempts, won);
        Debug.Log($"Level data that we are going to save is:{levelData} ${levelData.finishTime}");
        ctx.SaveLevelData(levelData);
    }

    public void GoBackToMenu() {
        Time.timeScale = 1;
        SendDataToContext(); // Save an attemp
        SceneManager.LoadScene("Menu");
    }


}
