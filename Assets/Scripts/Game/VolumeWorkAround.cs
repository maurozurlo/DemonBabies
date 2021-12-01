using UnityEngine;

public class VolumeWorkAround : MonoBehaviour {
	public bool musicControl;
	AudioSource audioSource;
	void Start () {
		GameContext gameContext = GameContext.control;
		audioSource = GetComponent<AudioSource>();

		if (audioSource && gameContext)
			audioSource.volume = musicControl ? gameContext.GetVolume("music") : gameContext.GetVolume("fx");
	}
}
