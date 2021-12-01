using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.IO;

public class GameContext : MonoBehaviour {

	public static GameContext control;
	public GameConfig[] availableLevels;
	public GameConfig currentLevel;
	// Player
	private int maxLevel;
	private int maxUnlockedLevel;
    private bool taunt = true;
    private int fxVolume = 1;
    private int musicVolume = 1;

	// Data
	private string saveFilePath;
	public LevelData currentLevelData;
	
	public SaveData currentSavedData;

	// Use this for initialization
	void Awake () {
		if (control == null) {
			control = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
		// Set save data path
		saveFilePath = $"{Application.persistentDataPath}/EXTAB.b";
		// Attempt to load data or create a new save file
		currentSavedData = GetSaveData();
		// Get the max id of current available levels
		maxLevel = GetMaxLevel();
		// Get the max unlocked level
		maxUnlockedLevel = GetMaxUnlockedLevelFromSavedData();
		// Set globals
		taunt = currentSavedData.playerData.taunts == 1;
		fxVolume = currentSavedData.playerData.fxVol;
		musicVolume = currentSavedData.playerData.musicVol;
	}

	int GetMaxLevel() {
		int temp = 0;
		foreach (GameConfig config in availableLevels) {
			int level = config.levelNumber;
			if (level > temp) temp = level;
		}
		return temp;
	}

	int GetMaxUnlockedLevelFromSavedData() {
		int temp = 0;
        for (int i = 0; i < currentSavedData.levelsData.Length; i++) {
			LevelData levelData = currentSavedData.levelsData[i];
			if (levelData.hasBeenFinished == 1 && levelData.levelId >= temp) {
				if (levelData.levelId + 1 <= maxLevel) {
					temp = levelData.levelId + 1;
				}
				else temp = levelData.levelId;
			}
		}
		//Debug.Log($"Max level unlocked was {temp}");
		return temp;
    }

	public int GetMaxUnlockedLevel() {
		return GetMaxUnlockedLevelFromSavedData();
	}

	SaveData GetSaveData() {
		if (File.Exists(saveFilePath)) {
			// Load existing savedata
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(saveFilePath, FileMode.Open);
			SaveData data = (SaveData)bf.Deserialize(file);
			file.Close();
			Debug.Log("Game data found and loaded!");
			return data;
		}
		else {
			// Generate playerData for the first time
			long currDate = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
			PlayerData playerData = new PlayerData(currDate, 10, 10, 1, 0);
			LevelData[] levelData = { };
			SaveData data = new SaveData(playerData, levelData);
			SaveGameData(data);
			Debug.Log("Created and saved new data");
			return data;
		}
	}


	public void SaveGameData(SaveData data) {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(saveFilePath);
		bf.Serialize(file, data);
		file.Close();
		Debug.Log($"Game data saved at {saveFilePath}!");
	}

	GameConfig GetLevel(int i) {
		foreach (GameConfig config in availableLevels) {
			if (config.levelNumber == i) return config;
		}
		Debug.LogError($"Level {i} not found");
		return availableLevels[0];
    }

	// TODO: REFACTOR
	public void AddUnlockedLevel(int newLevel){
		if (newLevel > maxUnlockedLevel) {
			maxUnlockedLevel = newLevel;
		}
	}

	public float GetVolume(string setting, bool convert = true)
    {
		float temp;
		if (setting == "music")
			temp = musicVolume;
		else temp = fxVolume;

		if (convert) return temp / 10;
		return temp;
    }

	public bool TauntsEnabled()
    {
		return taunt;
    }

	public int GetCurrentLevel()
    {
		return currentLevel.levelNumber;
    }

	public GameConfig SetNextLevel(int goTolevel)
    {
		AddUnlockedLevel(goTolevel);
		if (goTolevel > maxLevel) {
			Debug.LogWarning("end of the game");
			return null;
        }
        else {
			GameConfig nextLevel = GetLevel(goTolevel);
			currentLevel = nextLevel;
			return nextLevel;
		}
	}

	public GameConfig GetGameConfig() {
		return currentLevel;
    }

	public PlayerData GetGameData() {
		return currentSavedData.playerData;
    }

	public LevelData GetLevelData() {
		LevelData savedLevelData = null;
		//Debug.Log($"Saved leveldata: {currentSavedData.levelsData.Length}");
		for (int i = 0; i < currentSavedData.levelsData.Length; i++) {
			if (currentSavedData.levelsData[i].levelId == currentLevel.levelNumber) {
				savedLevelData = currentSavedData.levelsData[i];
			}
		}
		return savedLevelData;
	}

	public PlayerData GetPlayerData() {
		return currentSavedData.playerData;
    }

	public void SavePlayerData(PlayerData playerData) {
		SaveData saveData = new SaveData(playerData, currentSavedData.levelsData);
		currentSavedData = saveData;
		SaveGameData(saveData);
    }

	public void SaveLevelData(LevelData newLevelData) {
		List<LevelData> levelDatas = currentSavedData.levelsData.ToList();
		bool hasBeenReplaced = false;
        for (int i = 0; i < levelDatas.Count; i++) {
			if(levelDatas[i].levelId == newLevelData.levelId) {
				levelDatas[i] = newLevelData;
				hasBeenReplaced = true;
            }
        }
		if (!hasBeenReplaced) levelDatas.Add(newLevelData);
		Debug.Log($"Level datas has {levelDatas.Count} records");

		SaveData saveData = new SaveData(currentSavedData.playerData, levelDatas.ToArray());

		currentSavedData = saveData;
		currentLevelData = newLevelData;

		SaveGameData(saveData);
    }

}
