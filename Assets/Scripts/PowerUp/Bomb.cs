using UnityEngine;

public class Bomb : PowerUpFX {
    public AudioClip bombSound;
    public int damage;

    public override void ActivateEffect() {
        int damageForThisLevel;
        damageForThisLevel = damage + Mathf.FloorToInt(GameContext.control.GetCurrentLevel() * .5f);
        PowerUpFXManager powerUpFXManager = GetPowerUpFXManager();

        powerUpFXManager.DoDamage(damageForThisLevel, Color.red);
        powerUpFXManager.PlayOneShot(bombSound);
        
        Destroy(gameObject, bombSound.length);
    }
}