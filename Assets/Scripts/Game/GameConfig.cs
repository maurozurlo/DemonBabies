using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Level", menuName = "Create Level")]
public class GameConfig : ScriptableObject {
    [Header("Main")]
    public int levelNumber;
    public int health;
    public int enemies;
    public int powerUps;
    public float baseSpeed;
    public float speedMultiplier;
    public float maxSpeed;
    public Vector2 timeBetweenSpawns;
    [Header("Spawner")]
    [Range(0, 99)]
    public int likehoodOfPowerUps;
    [Header("Enemies")]
    public GameObject[] enemyPrefabs;
    public List<float> enemyWeights = new List<float>();
    [Header("PowerUps")]
    public GameObject[] powerupPrefabs;
    public List<float> powerUpWeights = new List<float>();
    [Header("Music")]
    public AudioClip music;
    [Header("Graphics")]
    public Material grass;
    public Material skybox;
    public GameObject clouds;
    public Vector3 cameraPosition;
    public bool isUpsideDown;
    [Header("Graphics/Fog")]
    public bool hasFog;
    public Color fogColor;
    public FogMode fogMode = FogMode.Linear;
    public float fogDensity;
    [Header("Graphics/ParticleSpawner")]
    public bool hasParticleSpawner;
    public GameObject particleSpawner;
    public Vector3 particleSpawnerPosition;
}
