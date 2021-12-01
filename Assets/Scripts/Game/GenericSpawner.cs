using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSpawner : MonoBehaviour {
    Dictionary<GameObject, float> enemies;
    Dictionary<GameObject, float> powerups;
    Vector2 timeBetweenSpawns;
    int levelPowerUps, levelEnemies;
    int likehoodOfPowerups;
    GameController control;
    public bool DEBUG_ONLY_POWERUPS = false;
    public bool DEBUG_ONLY_BABIES = false;

    void Start() {
        // Get config
        GameConfig config = GameContext.control.GetGameConfig();
        control = GameController.control;
        // Set dictionaries
        enemies = AddItemsToDictionary(config.enemyPrefabs, config.enemyWeights);
        powerups = AddItemsToDictionary(config.powerupPrefabs, config.powerUpWeights);
        likehoodOfPowerups = config.likehoodOfPowerUps;
        // Get Amounts for level
        levelEnemies = config.enemies;
        levelPowerUps = config.powerUps;
        timeBetweenSpawns = config.timeBetweenSpawns;
    }

    public void StartSpawning() {
        StartCoroutine("Spawner");

        // Debug method
        if (DEBUG_ONLY_BABIES || DEBUG_ONLY_POWERUPS) InvokeRepeating("DebugLogic", 0, .5f);
    }

    void DebugLogic() {
        if (DEBUG_ONLY_BABIES) {
            SpawnEnemy();
        }
        if (DEBUG_ONLY_POWERUPS) {
            SpawnPowerUp();
        }
    }

    IEnumerator Spawner() {
        bool cantSpawnEnemy = control.GetEnemiesSpawned() == levelEnemies;
        bool cantSpawnPowerUp = control.GetPowerUpsSpawned() == levelPowerUps;

        //Debug.Log($"{cantSpawnEnemy} cantSpawnEnemy");
        //Debug.Log($"{cantSpawnPowerUp} cantSpawnPowerUp");
        // if last enemy has been spawned, we shouldn't spawn anything else
        if (cantSpawnEnemy) yield break;
        // Decide if we're spawning an enemy or a powerup
        Dictionary<string, int> enemyOrPowerUp = new Dictionary<string, int> {
            { "enemy", 100 - likehoodOfPowerups },
            { "powerup", likehoodOfPowerups }
        };
        bool isEnemy = enemyOrPowerUp.RandomElementByWeight(e => e.Value).Key == "enemy";

        //Debug.Log($"{isEnemy} isEnemy");

        if (isEnemy && !cantSpawnEnemy || !isEnemy && cantSpawnPowerUp) SpawnEnemy();
        else if (isEnemy && cantSpawnEnemy || !isEnemy && !cantSpawnPowerUp) SpawnPowerUp();

        yield return new WaitForSeconds(Random.Range(timeBetweenSpawns.x, timeBetweenSpawns.y));
        StartCoroutine("Spawner");
    }

    void Spawn(Dictionary<GameObject, float> dict, string tag) {
        GameObject gameObject = Instantiate(GetRandom(dict), transform.position, Quaternion.identity);
        gameObject.name = tag;
        gameObject.tag = tag;
        gameObject.transform.SetParent(this.gameObject.transform);
        if (tag == "Enemy") control.IncrementEnemiesSpawned();
        else control.IncrementPowerUpsSpawned();
    }

    // Helpers
    void SpawnPowerUp() {
        Spawn(powerups, "PowerUp");
    }

    void SpawnEnemy() {
        Spawn(enemies, "Enemy");
    }

    GameObject GetRandom(Dictionary<GameObject, float> dict) {
        return dict.RandomElementByWeight(e => e.Value).Key;
    }

    Dictionary<GameObject, float> AddItemsToDictionary(GameObject[] gameObjects, List<float> list) {
        Dictionary<GameObject, float> _dict = new Dictionary<GameObject, float>();
        int totalLength = gameObjects.Length;
        for (int i = 0; i < totalLength; i++) {
            _dict.Add(gameObjects[i], list[i]);
        }
        return _dict;
    }

    public bool HasChildren() {
        return transform.childCount >= 1;
    }


    public void Pause() {
        foreach (Transform t in transform) {
            t.GetComponent<MovingTarget>().StopMoving();
        }
    }

    public void Resume() {
        foreach (Transform t in transform) {
            t.GetComponent<MovingTarget>().Restart();
        }
    }
}
