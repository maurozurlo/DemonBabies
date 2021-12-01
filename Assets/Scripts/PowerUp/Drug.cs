using UnityEngine;

public class Drug : PowerUpFX {
    public Color explosionColor;
    public float pitchOverride;
    public float FOV = 17;

    public override void ActivateEffect() {
        PowerUpFXManager powerUpFXManager = GetPowerUpFXManager();

        powerUpFXManager.StopAudio();
        powerUpFXManager.DoDamage(0, explosionColor);
        powerUpFXManager.ChangeOverlayState(true);
        powerUpFXManager.ChangeCameraFOV(FOV);
        powerUpFXManager.ChangePitch(pitchOverride);

        GameController.control.AddToBadTrips();
    }

}