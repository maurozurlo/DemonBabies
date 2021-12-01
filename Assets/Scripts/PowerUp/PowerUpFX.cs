using UnityEngine;

public abstract class PowerUpFX : MonoBehaviour
{
    public float duration;
    PowerUpFXManager powerUpFXManager;
    public bool toBeAddedToQueue = true;
    public bool hasStarted;

    public void Start() {
        powerUpFXManager = PowerUpFXManager.control;
    }

    public PowerUpFXManager GetPowerUpFXManager() { return powerUpFXManager; }
    public abstract void ActivateEffect();

}
