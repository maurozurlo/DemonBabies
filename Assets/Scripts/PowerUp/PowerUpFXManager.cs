using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFXManager : MonoBehaviour {
    public static PowerUpFXManager control;
    // Access Singleton
    GameController gameController;
    PlayerMovement playerMovement;
    public GameObject _musicController;
    MusicController musicController;

    // Accesible params
    AudioSource AS;
    float originalFOV;
    public GameObject overlay;

    // Queue
    Queue<PowerUpFX> QueueFX = new Queue<PowerUpFX>();

    void Awake() {
        // Singleton Pattern
        if (!control) control = this;
        else Destroy(gameObject);

        // Access other singletons
        gameController = GameController.control;
        playerMovement = PlayerMovement.control;
        musicController = _musicController.GetComponent<MusicController>();
        // Access FX GOs
        AS = GetComponent<AudioSource>();
        originalFOV = Camera.main.fieldOfView;

        if (!AS || !overlay) Debug.LogError("Overlay/AudioSource not setup");
    }

    public void ActivateFX(PowerUpFX powerUpFX) {
        // Check if the FX should be automatic or if it should be added to the queue
        if (!powerUpFX.toBeAddedToQueue) {
            Debug.Log($"Activating effect {powerUpFX.GetType().FullName}");
            powerUpFX.ActivateEffect();
        }
        else if (powerUpFX.toBeAddedToQueue) {
            Debug.Log($"Adding effect {powerUpFX.GetType().FullName} to queue");
            QueueFX.Enqueue(powerUpFX);
            ActivateFXinQueue();
        }
    }

    public void ActivateFXinQueue() {
        if (QueueFX.Count == 0) return;
        PowerUpFX powerUpFX = QueueFX.Peek();
        Debug.Log($"About to start next effect...{powerUpFX.GetType().FullName}");

        // Don't restart FX again
        if (powerUpFX.hasStarted) return;

        Debug.Log($"Activate effect {powerUpFX.GetType().FullName}");
        powerUpFX.ActivateEffect();
        powerUpFX.hasStarted = true;
        StartCoroutine(RemoveAllFXsAndDequeue(powerUpFX));
    }

    IEnumerator RemoveAllFXsAndDequeue(PowerUpFX powerUpFX) {
        Debug.Log($"About to remove effect {powerUpFX.GetType().FullName} in {powerUpFX.duration} seconds");
        yield return new WaitForSeconds(powerUpFX.duration);
        Debug.Log($"Removing effect {powerUpFX.GetType().FullName}");

        ChangeOverlayState(false);
        Camera.main.fieldOfView = originalFOV;
        musicController.ReturnPitchToNormal();
        playerMovement.ReturnToNormalSpeed();
        playerMovement.ReturnVisualsToNormal();
        QueueFX.Dequeue();
        ActivateFXinQueue();
    }


    public void DoDamage(int damage, Color color) {
        gameController.TakeDamage(damage, color, "powerup");
    }

    public void PlayOneShot(AudioClip clip, bool overrideSound = false) {
        if (!overrideSound && AS.isPlaying) return;
        AS.Stop();
        AS.PlayOneShot(clip);
    }

    public bool IsPlaying() {
        return AS.isPlaying;
    }

    public void StopAudio() {
        AS.Stop();
    }


    public void ChangeCameraFOV(float FOV) {
        Camera.main.fieldOfView = FOV;
    }

    // This could be <<a lot>> more abstract
    public void ChangeOverlayState(bool isActive) {
        overlay.GetComponent<UnityEngine.UI.Image>().enabled = isActive;
        overlay.GetComponent<LSDFX>().enabled = isActive;
    }

    public PlayerMovement GetPlayerMovement() { return playerMovement; }

    public void ChangePitch(float pitch) { musicController.ChangePitch(pitch); }
}
