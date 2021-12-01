using System;

[Serializable]
public class PlayerData {
	public long startTime;
	public int fxVol;
	public int musicVol;
	public int taunts;
	public int hasFinishedTheGame;

	public PlayerData(long startTime, int fxVol, int musicVol, int taunts, int hasFinishedTheGame) {
		this.startTime = startTime;
		this.fxVol = fxVol;
		this.musicVol = musicVol;
		this.taunts = taunts;
		this.hasFinishedTheGame = hasFinishedTheGame;
    }
}
[Serializable]
public class LevelData {
	public int levelId;
	public int defense;
	public int attacks;
	public int powerUps;
	public int maxCombo;
	public int badTrips;
	public int attempts;
	public int finishTime;
	public int hasBeenFinished;

	public LevelData(int levelId, int defense, int attacks, int powerUps, int maxCombo, int badTrips, int finishTime, int attempts, int hasBeenFinished) {
		this.levelId = levelId;
		this.defense = defense;
		this.attacks = attacks;
		this.powerUps = powerUps;
		this.maxCombo = maxCombo;
		this.badTrips = badTrips;
		this.finishTime = finishTime;
		this.attempts = attempts;
		this.hasBeenFinished = hasBeenFinished;
	}
}

[Serializable]
public class SaveData {
	public PlayerData playerData;
	public LevelData[] levelsData;

	public SaveData(PlayerData playerData, LevelData[] levelsData) {
		this.playerData = playerData;
		this.levelsData = levelsData;
    }
}