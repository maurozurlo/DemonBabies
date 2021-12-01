using UnityEngine;
using System.Collections;

public class PowerUp : MovingTarget {
    public AudioClip loopSound;
    public AudioClip deathSound;
    PowerUpFX powerUpFX;

    public GameObject explo;

    public override void Init() {
        powerUpFX = GetComponent<PowerUpFX>();
        if (!powerUpFX) Debug.LogError($"Missing PowerUpFX on {name}");
    }

    public void ActivatePowerUp() {
        StopAllCoroutines();
        DeactivateVisuals();
        PlayOneShot(deathSound, true);
        PowerUpFXManager.control.ActivateFX(powerUpFX); // Add FX to Queue / Execute Immediately
        Destroy(gameObject, deathSound.length);
    }

    public void KillWithSwatter() {
        StopAllCoroutines();
        Instantiate(explo, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        DeactivateVisuals();
        PlayOneShot(deathSound,true);
        Destroy(gameObject, deathSound.length);
    }

    void DeactivateVisuals() {
        foreach (Renderer item in GetComponentsInChildren<Renderer>()) {
            item.enabled = false;
        }
        foreach (Collider item in GetComponentsInChildren<Collider>()) {
            item.enabled = false;
        }
    }

    public override IEnumerator PlaySound() {
        PlaySoundInLoop(loopSound);
        yield break;
    }
}