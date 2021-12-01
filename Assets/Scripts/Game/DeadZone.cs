using System.Collections;
using UnityEngine;

public class DeadZone : MonoBehaviour {
    public AudioClip[] adrielTaunts;
    private AudioSource AS;
    public int probabilidad;
    public int lastTauntPlayed;
    public int prob;
    int damageForThisLevel;
    public float delay;
    public int tauntToPlay;
    bool tauntsEnabled = true;
    GameContext ctx;

    void Start() {
        ctx = GameContext.control;

        AS = gameObject.AddComponent<AudioSource>();

        tauntsEnabled = ctx.TauntsEnabled();
        AS.volume = ctx.GetVolume("fx");

        probabilidad = tauntsEnabled ? probabilidad : 0;
    }

    void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Enemy")) {
            Baby baby = col.gameObject.GetComponent<Baby>();
            baby.ReachDestination();
            GameController.control.TakeDamage(baby.damage, Color.red, "baby");
            StartCoroutine("TryAndPlay");
        }
        else if (col.CompareTag("PowerUp")) {
            PowerUp currentPowerUp = col.gameObject.GetComponent<PowerUp>();
            currentPowerUp.ActivatePowerUp();
        }
    }

    IEnumerator TryAndPlay() {
        yield return new WaitForSeconds(delay);
        prob = Random.Range(0, 100);
        if (prob < probabilidad) {
            PlayTaunt();
        }
    }

    void PlayTaunt() {
        if (AS.isPlaying) return;
        tauntToPlay = Random.Range(0, adrielTaunts.Length);
        //para que no se repitan los taunts
        if (tauntToPlay == lastTauntPlayed) {
            if (tauntToPlay + 1 < adrielTaunts.Length) {
                lastTauntPlayed = tauntToPlay + 1;
            }
            else if (tauntToPlay - 1 >= 0) {
                lastTauntPlayed = tauntToPlay - 1;
            }
        }
        else {
            lastTauntPlayed = tauntToPlay;
        }
        AS.PlayOneShot(adrielTaunts[lastTauntPlayed]);
    }

}
