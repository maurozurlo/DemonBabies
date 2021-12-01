using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLevels : MonoBehaviour {
	public GameObject[] levels;
	// Use this for initialization
	void Start () {
		GameContext gameContext = GameContext.control;
			if (gameContext.GetMaxUnlockedLevel() <= levels.Length) {

			int lenght = gameContext.GetMaxUnlockedLevel() + 1; // 0 based...
				for (int i = 0; i < lenght; i++) {
					levels [i].GetComponent<UnityEngine.UI.Button> ().interactable = true;
				}
			}
	}
}
