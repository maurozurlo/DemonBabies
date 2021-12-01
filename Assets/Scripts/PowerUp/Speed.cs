using UnityEngine;

public class Speed : PowerUpFX {
    public Sprite overrideSprite;
    public float overrideSpeed;
    public Color explosionColor;
    public float pitchOverride;

    public override void ActivateEffect() {
        PowerUpFXManager powerUpFXManager = GetPowerUpFXManager();

        powerUpFXManager.StopAudio();
        powerUpFXManager.DoDamage(0, explosionColor);
        powerUpFXManager.ChangePitch(pitchOverride);

        PlayerMovement playerMovement = powerUpFXManager.GetPlayerMovement();
        playerMovement.ChangeSpeed(overrideSpeed);
        playerMovement.ChangeVisuals(overrideSprite);

        GameController.control.AddToPowerUps();
    }
}